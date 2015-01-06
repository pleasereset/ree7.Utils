using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace ree7.Utils.Controls
{
	internal class ApplyStatusBarInfo
	{
		public Thickness OriginalMargin;
	}

	public class PageExtensions
	{
		/* On Windows Runtime, visual elements can be unloaded but still in cache (when the 
		 * containing page is the in the navigation back stack)
		 * So when the a target component is unloaded, I will subscribe to its OnLoaded event
		 * to restore its state if reloaded through the WeakEventManager, to avoid preventing
		 * the CG to reclaim it if necessary
		 */

		// Repository containing informations about controls when ApplyStatusBarMargin is applied
		private static Dictionary<DependencyObject, ApplyStatusBarInfo> asbmRepository = new Dictionary<DependencyObject, ApplyStatusBarInfo>();

		// Have we already hooked ScreenOrientationChanged ?
		private static bool orientationChangedHooked = false;

		#region public bool ApplyStatusBarMargin (AttachedProperty)
		public static bool GetApplyStatusBarMargin(FrameworkElement obj)
		{
			return (bool)obj.GetValue(ApplyStatusBarMarginProperty);
		}

		public static void SetApplyStatusBarMargin(FrameworkElement obj, bool value)
		{
			obj.SetValue(ApplyStatusBarMarginProperty, value);
		}

		// Using a DependencyProperty as the backing store for ApplyStatusBarMargin.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ApplyStatusBarMarginProperty =
			DependencyProperty.RegisterAttached("ApplyStatusBarMargin", typeof(bool), typeof(PageExtensions), new PropertyMetadata(false, OnApplyStatusBarMarginChanged));
		#endregion

		private static void OnApplyStatusBarMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Update the margin when the screen orientation changed
			if (orientationChangedHooked == false)
			{
				ApplyStatusBarHookOrientationChanged();
			}

			var fe = ((FrameworkElement)d);
			if((bool)e.NewValue)
			{
				ApplyStatusBarMargin_Subscribe(fe);
			}
			else
			{
				if(asbmRepository.ContainsKey(fe))
				{
					ApplyStatusBarMargin_Unsubscribe(fe);
				}
			}
		}

		private static void UpdateStatusbarMargin(FrameworkElement e)
		{
			// Compute the margin
			var infos = asbmRepository[e];
			var applicationView = ApplicationView.GetForCurrentView();
			var coreWindow = CoreWindow.GetForCurrentThread();

			if (applicationView.Orientation == ApplicationViewOrientation.Portrait)
			{
				// StatusBar on top
				// ApplicationBar on bottom : we don't care
				var top = Math.Abs(coreWindow.Bounds.Top - applicationView.VisibleBounds.Top);

				var newMargin = new Thickness(
					infos.OriginalMargin.Left,
					infos.OriginalMargin.Top + top,
					infos.OriginalMargin.Right,
					infos.OriginalMargin.Bottom);
				e.Margin = newMargin;
			}
			else if (applicationView.Orientation == ApplicationViewOrientation.Landscape)
			{
				// StatusBar on the left
				// ApplicationBar on the right
				// or reverse
				var left = Math.Abs(coreWindow.Bounds.Left - applicationView.VisibleBounds.Left);
				var right = Math.Abs(coreWindow.Bounds.Right - applicationView.VisibleBounds.Right);

				var newMargin = new Thickness(
					infos.OriginalMargin.Left + left,
					infos.OriginalMargin.Top,
					infos.OriginalMargin.Right + right,
					infos.OriginalMargin.Bottom);
				e.Margin = newMargin;
			}
		}

		private static void ApplyStatusBarMargin_Subscribe(FrameworkElement e)
		{
			// Add the element to the managed controls repository
			asbmRepository[e] = new ApplyStatusBarInfo()
			{
				OriginalMargin = e.Margin
			};

			// Hook unloaded event
			e.Unloaded += ApplyStatusBarMargin_Target_Unloaded;

			UpdateStatusbarMargin(e);
		}

		private static void ApplyStatusBarMargin_Unsubscribe(FrameworkElement e)
		{
			if (asbmRepository.ContainsKey(e))
			{
				// Unhook events
				e.Unloaded -= ApplyStatusBarMargin_Target_Unloaded;

				// Restore original margins
				e.Margin = asbmRepository[e].OriginalMargin;

				// Remove reference from repository
				asbmRepository.Remove(e);
			}
		}

		private static void ApplyStatusBarMargin_Target_Unloaded(object sender, RoutedEventArgs e)
		{
			ApplyStatusBarMargin_Unsubscribe((FrameworkElement)sender);

			// Listen (weakly) to the Loaded event in case
			// the element is restored
			// TODO !!!
			((FrameworkElement)sender).Loaded += ApplyStatusBarMargin_Target_Loaded;
		}

		private static void ApplyStatusBarMargin_Target_Loaded(object sender, RoutedEventArgs e)
		{
			var fe = ((FrameworkElement)sender);
			
			fe.Loaded -= ApplyStatusBarMargin_Target_Loaded;
			ApplyStatusBarMargin_Subscribe(fe);
		}

		private static void ApplyStatusBarHookOrientationChanged()
		{
			orientationChangedHooked = true;
			ApplicationView.GetForCurrentView().VisibleBoundsChanged += OnApplicationVisibleBoundsChanged;
		}

		private static void OnApplicationVisibleBoundsChanged(ApplicationView sender, object args)
		{
			// Update all margins
			foreach (FrameworkElement e in asbmRepository.Keys)
			{
				UpdateStatusbarMargin(e);
			}
		}		
	}
}
