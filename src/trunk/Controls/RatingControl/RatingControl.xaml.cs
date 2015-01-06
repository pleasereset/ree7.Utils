using ree7.Utils.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace ree7.Utils.Controls
{
	public sealed partial class RatingControl : UserControl
	{
		public RatingControl()
		{
			this.InitializeComponent();
		}

		#region public double SymbolSize
		public double SymbolSize
		{
			get { return (double)GetValue(SymbolSizeProperty); }
			set { SetValue(SymbolSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SymbolSizeProperty =
			DependencyProperty.Register("Value", typeof(double), typeof(RatingControl), new PropertyMetadata(20.0, OnSymbolSizeChanged));

		private static void OnSymbolSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RatingControl)d).UpdateSymbolSize();
		}
		#endregion

		#region public double Value
		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(double), typeof(RatingControl), new PropertyMetadata(0, OnValueChanged));

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RatingControl)d).UpdateValue();
		}
		#endregion

		private int GetStarsCount()
		{
			return HollowContainer.Children.Count;
		}

		private void UpdateSymbolSize()
		{
			foreach(Path p in HollowContainer.Children)
			{
				p.Height = SymbolSize;
				p.Width = SymbolSize;
			}
			foreach (Path p in FilledContainer.Children)
			{
				p.Height = SymbolSize;
				p.Width = SymbolSize;
			}

			UpdateValue();
		}

		private void UpdateValue()
		{
			int starsCount = GetStarsCount();
			double value = this.Value / starsCount; // percentage

			double fullWidth = starsCount * SymbolSize;
			RectangleGeometry clip = new RectangleGeometry()
			{
				Rect = new Rect(0, 0, fullWidth * value, SymbolSize)
			};
			FilledContainer.Clip = clip;
		}
	}
}
