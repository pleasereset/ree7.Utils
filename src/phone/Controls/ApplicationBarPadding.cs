using System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace ree7.Utils.Controls
{
	/// <summary>
	/// Creates an element whose height is the same as the Bottom ApplicationBar.
	/// Useful for padding a ListView/GridView.
	/// </summary>
	public sealed class ApplicationBarVerticalPadding : FrameworkElement
	{
		public ApplicationBarVerticalPadding()
		{
			this.Loaded += ApplicationBarPadding_Loaded;
			this.Unloaded += ApplicationBarPadding_Unloaded;
		}

		private void ApplicationBarPadding_Loaded(object sender, RoutedEventArgs e)
		{
			ApplicationView.GetForCurrentView().VisibleBoundsChanged += ApplicationBarPadding_VisibleBoundsChanged;
			Update();
		}

		private void ApplicationBarPadding_Unloaded(object sender, RoutedEventArgs e)
		{
			ApplicationView.GetForCurrentView().VisibleBoundsChanged -= ApplicationBarPadding_VisibleBoundsChanged;
		}

		private void ApplicationBarPadding_VisibleBoundsChanged(ApplicationView sender, object args)
		{
			Update();
		}

		private void Update()
		{
			var applicationView = ApplicationView.GetForCurrentView();
			var coreWindow = CoreWindow.GetForCurrentThread();

			if (applicationView.Orientation == ApplicationViewOrientation.Portrait)
			{
				// ApplicationBar on bottom 
				var height = Math.Abs(coreWindow.Bounds.Bottom - applicationView.VisibleBounds.Bottom);

				this.Width = 1;
				this.Height = height;
			}
			else if (applicationView.Orientation == ApplicationViewOrientation.Landscape)
			{
				// ApplicationBar on the left/right, no vertical padding needed
				this.Height = 0;
				this.Width = 0;
			}
		}
	}
}
