using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ree7.Utils.Controls
{
	/// <summary>
	/// Container keeping a square aspect-ratio
	/// </summary>
	public class SquareBorder : ContentControl
	{
		private bool rendering = false;

		public SquareBorder()
		{
			this.SizeChanged += OnSizeChanged;
		}

		public Orientation MasterDimension
		{
			get { return (Orientation)GetValue(MasterDimensionProperty); }
			set { SetValue(MasterDimensionProperty, value); }
		}

		// Using a DependencyProperty as the backing store for MasterDimension.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MasterDimensionProperty =
			DependencyProperty.Register("MasterDimension", typeof(Orientation), typeof(SquareBorder), new PropertyMetadata(Orientation.Horizontal, OnMasterDimensionChanged));

		private static void OnMasterDimensionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((SquareBorder)d).Render();
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.Render();
		}

		private void Render()
		{
			if (rendering) return;

			rendering = true;

			if (MasterDimension == Orientation.Horizontal)
			{
				this.Height = this.ActualWidth;
			}
			else
			{
				this.Width = this.ActualHeight;
			}

			rendering = false;
		}
	}
}