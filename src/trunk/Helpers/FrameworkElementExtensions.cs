
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ree7.Utils.Helpers
{
	public static class FrameworkElementExtensions
	{
		public static FrameworkElement FindDescendantByName(this FrameworkElement element, string name)
		{
			if (element == null || string.IsNullOrWhiteSpace(name)) { return null; }

			if (name.Equals(element.Name, StringComparison.OrdinalIgnoreCase))
			{
				return element;
			}
			var childCount = VisualTreeHelper.GetChildrenCount(element);
			for (int i = 0; i < childCount; i++)
			{
				var result = (VisualTreeHelper.GetChild(element, i) as FrameworkElement).FindDescendantByName(name);
				if (result != null) { return result; }
			}
			return null;
		}

		public static T FindDescendantByType<T>(this FrameworkElement element) where T : class
		{
			if (element == null) { return null; }

			if (element is T)
			{
				return element as T;
			}
			var childCount = VisualTreeHelper.GetChildrenCount(element);
			for (int i = 0; i < childCount; i++)
			{
				var result = (VisualTreeHelper.GetChild(element, i) as FrameworkElement).FindDescendantByType<T>();
				if (result != null) { return result; }
			}
			return null;
		}

		public static T FindParentByType<T>(this FrameworkElement element) where T : class
		{
			if (element.Parent == null) return null;
			if (element.Parent is T) return element.Parent as T;

			return FindParentByType<T>(element.Parent as FrameworkElement);
		}
	}
}
