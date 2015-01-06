using System.Collections;
using System.Collections.Specialized;
using Windows.UI.Xaml;

namespace ree7.Utils.UI
{
	/// <summary>
	/// Attached property for XAML FrameworkElement that once binded to an ObservableCollection
	/// (or any INotifyCollectionChanged class) will trigger the element's visibility ON when
	/// the collection is empty and OFF otherwise.
	/// </summary>
	public static class DisplayOnCollection
	{
		public static object GetEmpty(FrameworkElement obj)
		{
			return (object)obj.GetValue(EmptyProperty);
		}

		public static void SetEmpty(FrameworkElement obj, object value)
		{
			obj.SetValue(EmptyProperty, value);
		}

		// Using a DependencyProperty as the backing store for Collection.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EmptyProperty =
			DependencyProperty.RegisterAttached("Empty", typeof(object), typeof(DisplayOnCollection), new PropertyMetadata(null, OnEmptyPropertyChanged));

		private static void OnEmptyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement fe = d as FrameworkElement;

			NotifyCollectionChangedEventHandler onCollectionChanged = (sender, collectionChangedEventArgs) =>
			{
				fe.Visibility = GetVisibilityForEmpty(e.NewValue as ICollection);
			};

			RoutedEventHandler onElementUnloaded = null;
			onElementUnloaded = (sender, eventArgs) =>
			{
				((FrameworkElement)sender).Unloaded -= onElementUnloaded;
				((INotifyCollectionChanged)e.NewValue).CollectionChanged -= onCollectionChanged;
			};

			if (e.OldValue != null && e.OldValue is INotifyCollectionChanged)
			{
				((INotifyCollectionChanged)e.OldValue).CollectionChanged -= onCollectionChanged;
			}
			if (e.NewValue != null && e.NewValue is INotifyCollectionChanged)
			{
				((INotifyCollectionChanged)e.NewValue).CollectionChanged += onCollectionChanged;
				fe.Unloaded += onElementUnloaded;
			}

			fe.Visibility = GetVisibilityForEmpty(e.NewValue as IEnumerable);
		}

		private static Visibility GetVisibilityForEmpty(IEnumerable collection)
		{
			if (collection == null) return Visibility.Visible;

			var iterator = collection.GetEnumerator();
			if(iterator.MoveNext())
			{
				// has at least 1 element
				return Visibility.Collapsed;
			}

			return Visibility.Visible;
		}

		public static object GetFilled(DependencyObject obj)
		{
			return (object)obj.GetValue(FilledProperty);
		}

		public static void SetFilled(DependencyObject obj, object value)
		{
			obj.SetValue(FilledProperty, value);
		}

		// Using a DependencyProperty as the backing store for Filled.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty FilledProperty =
			DependencyProperty.RegisterAttached("Filled", typeof(object), typeof(DisplayOnCollection), new PropertyMetadata(null, OnFilledPropertyChanged));

		private static void OnFilledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement fe = d as FrameworkElement;

			NotifyCollectionChangedEventHandler onCollectionChanged = (sender, collectionChangedEventArgs) =>
			{
				((FrameworkElement)sender).Visibility = GetVisibilityForFilled(e.NewValue as ICollection);
			};

			RoutedEventHandler onElementUnloaded = null;
			onElementUnloaded = (sender, eventArgs) =>
			{
				((FrameworkElement)sender).Unloaded -= onElementUnloaded;
				((INotifyCollectionChanged)sender).CollectionChanged -= onCollectionChanged;
			};

			if (e.OldValue != null && e.OldValue is INotifyCollectionChanged)
			{
				((INotifyCollectionChanged)e.OldValue).CollectionChanged -= onCollectionChanged;
			}
			if (e.NewValue != null && e.NewValue is INotifyCollectionChanged)
			{
				((INotifyCollectionChanged)e.NewValue).CollectionChanged += onCollectionChanged;
				((FrameworkElement)e.NewValue).Unloaded += onElementUnloaded;
			}

			fe.Visibility = GetVisibilityForFilled(e.NewValue as IEnumerable);
		}

		private static Visibility GetVisibilityForFilled(IEnumerable collection)
		{
			if (collection == null) return Visibility.Collapsed;

			var iterator = collection.GetEnumerator();
			if (iterator.MoveNext())
			{
				// has at least 1 element
				return Visibility.Visible;
			}

			return Visibility.Collapsed;
		}
	}
}
