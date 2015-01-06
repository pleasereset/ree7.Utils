using System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace ree7.Utils.Controls
{
	/// <summary>
	/// Creates an element whose height is the same as the StatusBar.
	/// Useful for padding a ListView/GridView.
	/// </summary>
	public sealed class StatusBarVerticalPadding : FrameworkElement
	{
		public StatusBarVerticalPadding()
		{
			this.Loaded += StatusBarPadding_Loaded;
			this.Unloaded += StatusBarPadding_Unloaded;
		}

		private void StatusBarPadding_Loaded(object sender, RoutedEventArgs e)
		{
			ApplicationView.GetForCurrentView().VisibleBoundsChanged += StatusBarPadding_VisibleBoundsChanged;
			Update();
		}

		private void StatusBarPadding_Unloaded(object sender, RoutedEventArgs e)
		{
			ApplicationView.GetForCurrentView().VisibleBoundsChanged -= StatusBarPadding_VisibleBoundsChanged;
		}

		private void StatusBarPadding_VisibleBoundsChanged(ApplicationView sender, object args)
		{
			Update();
		}

		private void Update()
		{
			var StatusView = ApplicationView.GetForCurrentView();
			var coreWindow = CoreWindow.GetForCurrentThread();

			if (StatusView.Orientation == ApplicationViewOrientation.Portrait)
			{
				// StatusBar on top 
				var height = Math.Abs(coreWindow.Bounds.Top - StatusView.VisibleBounds.Top);

				this.Width = 1;
				this.Height = height;
			}
			else if (StatusView.Orientation == ApplicationViewOrientation.Landscape)
			{
				// StatusBar on the left/right, no vertical padding needed
				this.Height = 0;
				this.Width = 0;
			}
		}
	}
}
