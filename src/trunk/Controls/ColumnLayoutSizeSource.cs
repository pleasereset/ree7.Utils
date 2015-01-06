using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ree7.Utils.Controls
{
	/// <summary>
	/// Provides item height and width for items arranged in columns
	/// depending of the container width
	/// </summary>
	public class ColumnLayoutSizeSource : DependencyObject, INotifyPropertyChanged
	{
		#region public double ItemHeight
		private double _ItemHeight;
		public double ItemHeight
		{
			get
			{
				return _ItemHeight;
			}
			set
			{
				if (_ItemHeight != value)
				{
					_ItemHeight = value;
					RaisePropertyChanged("ItemHeight");
				}
			}
		}
		#endregion
		#region public double ItemWidth
		private double _ItemWidth;
		public double ItemWidth
		{
			get
			{
				return _ItemWidth;
			}
			set
			{
				if (_ItemWidth != value)
				{
					_ItemWidth = value;
					RaisePropertyChanged("ItemWidth");
				}
			}
		}
		#endregion

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion

		public FrameworkElement Container
		{
			get { return (FrameworkElement)GetValue(ContainerProperty); }
			set { SetValue(ContainerProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Container.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ContainerProperty =
			DependencyProperty.Register("Container", typeof(FrameworkElement), typeof(ColumnLayoutSizeSource), new PropertyMetadata(null, OnContainerChanged));

		private static void OnContainerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ColumnLayoutSizeSource)d).OnContainerChangedImpl(d,e);
		}

		private void OnContainerChangedImpl(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if(e.OldValue != null)
			{
				((FrameworkElement)e.OldValue).SizeChanged -= OnContainerSizeChanged;
			}
			((FrameworkElement)e.NewValue).SizeChanged += OnContainerSizeChanged;
		}

		private void OnContainerSizeChanged(object sender, SizeChangedEventArgs e)
		{
			ComputeSize();
		}

		public int Columns
		{
			get { return (int)GetValue(ColumnsProperty); }
			set { SetValue(ColumnsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ColumnsProperty =
			DependencyProperty.Register("Columns", typeof(int), typeof(ColumnLayoutSizeSource), new PropertyMetadata(2, OnColumnsChanged));

		private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ColumnLayoutSizeSource)d).ComputeSize();
		}
		
		public double AspectRatio { get; set; }

		public double GutterSize { get; set; }

		private void ComputeSize()
		{
			if (Container == null) return;

			double containerReferenceSize = Container.ActualWidth;

			double availableWidth = (containerReferenceSize - (Columns * GutterSize)) - 1; // -1 to avoid rounding errors
			ItemWidth = availableWidth / Columns;
			ItemHeight = ItemWidth * AspectRatio;
		}
	}
}
