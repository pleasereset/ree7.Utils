using System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace ree7.Utils.Controls
{
	/// <summary>
	/// Provides backwards compatibility for apps relying on ApplicationBarVerticalPadding
	/// that has been renamed to SystemChromeVerticalPadding
	/// </summary>
	[Obsolete]
	public sealed class ApplicationBarVerticalPadding : SystemChromeVerticalPadding
	{

	}

	/// <summary>
	/// Creates an element whose height is the same as the Bottom ApplicationBar + the 
	/// eventual navigation bar (software buttons).
	/// Useful for padding a ListView/GridView.
	/// </summary>
	public class SystemChromeVerticalPadding : FrameworkElement
	{
		public SystemChromeVerticalPadding()
		{
			this.Loaded += OnLoaded;
			this.Unloaded += OnUnloaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			ApplicationView.GetForCurrentView().VisibleBoundsChanged += OnVisibleBoundsChanged;
			Update();
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			ApplicationView.GetForCurrentView().VisibleBoundsChanged -= OnVisibleBoundsChanged;
		}

		private void OnVisibleBoundsChanged(ApplicationView sender, object args)
		{
			Update();
		}

		private void Update()
		{
			var applicationView = ApplicationView.GetForCurrentView();
			var coreWindow = CoreWindow.GetForCurrentThread();

			if (applicationView.Orientation == ApplicationViewOrientation.Portrait)
			{
				// ApplicationBar/chrome on bottom 
				var height = Math.Abs(coreWindow.Bounds.Bottom - applicationView.VisibleBounds.Bottom);

				this.Width = 1;
				this.Height = height;
			}
			else if (applicationView.Orientation == ApplicationViewOrientation.Landscape)
			{
				// ApplicationBar/chrome on the left/right, no vertical padding needed
				this.Height = 0;
				this.Width = 0;
			}
		}
	}
}
