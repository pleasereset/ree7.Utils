using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;

namespace ree7.Utils.Collections
{
    public class GroupedCollectionAdapter : DependencyObject, INotifyPropertyChanged
    {
        // The type that will be used to group the Collection
        // This is where the grouping algorithm is implemented
        protected Type grouperGenericType = typeof(CollectionGrouper<>);

        public object GeneratedItems { get; protected set; }
		public const string GeneratedItemsPropertyName = "GeneratedItems";

        #region public object ItemsSource
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(GroupedCollectionAdapter), new PropertyMetadata(null, OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GroupedCollectionAdapter)d).AdaptCollection();
        }
        #endregion

        #region public string PropertyName
        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(GroupedCollectionAdapter), new PropertyMetadata(null, OnPropertyNameChanged));

        private static void OnPropertyNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GroupedCollectionAdapter)d).AdaptCollection();
        }
        #endregion

        protected virtual void AdaptCollection()
        {
            if(this.ItemsSource == null || this.PropertyName == null)
            {
                return; // invalid state
            }

            Type t = ItemsSource.GetType();
            if (t.GenericTypeArguments != null && t.GenericTypeArguments.Length == 1)
            {
                Type grouperDefinedType = grouperGenericType.MakeGenericType(t.GenericTypeArguments);
                ICollectionGrouper grouperInstance = (ICollectionGrouper)Activator.CreateInstance(grouperDefinedType);
                
				GeneratedItems = grouperInstance.GroupCollection(ItemsSource, PropertyName);
				RaisePropertyChanged(GeneratedItemsPropertyName);
            }
            else
            {
                GroupedCollectionHelper.Log("GrouperCollectionAdapter : Items source is not a generic with one type parameter.");
            }
        }

		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}

    internal static class GroupedCollectionHelper
    {
        [Conditional("DEBUG")]
        internal static void Log(string message, params string[] args)
        {
            Debug.WriteLine(message, args);
        }
    }

    internal interface ICollectionGrouper
    {
        object GroupCollection(object collection, string propertyName);
    }

    internal class CollectionGrouper<T> : ICollectionGrouper
    {
        protected string UnknownKey = "#";

        public virtual object GroupCollection(object collection, string propertyName)
        {
            IEnumerable<T> typedCollection = collection as IEnumerable<T>;
            if(typedCollection == null)
            {
                GroupedCollectionHelper.Log("GrouperCollectionAdapter : Items source type mismatch.");
                return null;
            }

			return (from item in typedCollection
				   group item by GetKeyForItem(item, propertyName) into newGroup
				   orderby newGroup.Key
				   select newGroup).ToList();
        }

        protected virtual string GetKeyForItem(T obj, string propertyName)
        {
            Type t = obj.GetType();
            PropertyInfo prop = t.GetRuntimeProperty(propertyName);
            if(prop == null)
            {
                GroupedCollectionHelper.Log("GrouperCollectionAdapter : Specified object has no property '{0}'", propertyName);
                return UnknownKey;
            }
            return prop.GetMethod.Invoke(obj, null).ToString();
        }
    }
}
