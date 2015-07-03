using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ree7.Utils.UI
{
	public class XamlExtensions
	{
		// Can be applied on any control. Will use animations/transitions 
		// between states.
		#region public static string VisualState
		public static string GetVisualState(Control obj)
		{
			return (string)obj.GetValue(VisualStateProperty);
		}

		public static void SetVisualState(Control obj, string value)
		{
			obj.SetValue(VisualStateProperty, value);
		}

		// Using a DependencyProperty as the backing store for VisualState.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty VisualStateProperty =
			DependencyProperty.RegisterAttached("VisualState", typeof(string), typeof(XamlExtensions), new PropertyMetadata(null, OnVisualStateChanged));

		private static void OnVisualStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				Control c = d as Control;
				VisualStateManager.GoToState(c, (string)e.NewValue, true);
			}
			catch { }
		}

		#endregion public string VisualState
	}

	public static class XamlExtensionsStatic
	{
		public static void ApplyVisualState(this Control c)
		{
			string state = c.GetValue(XamlExtensions.VisualStateProperty) as string;
			if (state != null)
			{
				VisualStateManager.GoToState(c, state, false);
			}
		}
	}
}
