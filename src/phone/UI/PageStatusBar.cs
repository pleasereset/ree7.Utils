using System;
using System.Windows.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ree7.Utils.UI
{
	public class PageStatusBar
	{
		#region ProgressBarVisible
		public static bool GetProgressBarVisible(DependencyObject obj)
		{
			return (bool)obj.GetValue(ProgressBarVisibleProperty);
		}

		public static void SetProgressBarVisible(DependencyObject obj, bool value)
		{
			obj.SetValue(ProgressBarVisibleProperty, value);
		}

		// Using a DependencyProperty as the backing store for ProgressBarVisible.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ProgressBarVisibleProperty =
			DependencyProperty.RegisterAttached("ProgressBarVisible", typeof(bool), typeof(PageStatusBar), new PropertyMetadata(false, ProgressBarVisiblePropertyChanged));

		private static async void ProgressBarVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
			bool value = (bool)e.NewValue;
			if (value)
				await StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();
			else
				await StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
		}
		#endregion ProgressBarVisible

		#region ProgressBarMessage

		public static string GetProgressBarMessage(DependencyObject obj)
		{
			return (string)obj.GetValue(ProgressBarMessageProperty);
		}

		public static void SetProgressBarMessage(DependencyObject obj, string value)
		{
			obj.SetValue(ProgressBarMessageProperty, value);
		}

		// Using a DependencyProperty as the backing store for ProgressBarMessage.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ProgressBarMessageProperty =
			DependencyProperty.RegisterAttached("ProgressBarMessage", typeof(string), typeof(PageStatusBar), new PropertyMetadata(null, ProgressBarMessagePropertyChanged));

		private static void ProgressBarMessagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
			string value = (string)e.NewValue;
			StatusBar.GetForCurrentView().ProgressIndicator.Text = value;
		}
		#endregion ProgressBarMessage		
	}
}
