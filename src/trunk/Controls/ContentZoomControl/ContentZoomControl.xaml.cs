using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace ree7.Utils.Controls
{
	public enum ContentZoomControlState
	{
		Still,
		FreeMoving
	}

	/// <summary>
	/// Provides touch "zoomability" to a UIElement
	/// </summary>
	public sealed partial class ContentZoomControl : UserControl
	{
		double minScale = 0.9;
		double previousScale = 1.0;
		bool manipulationEnabled = false;

		public ContentZoomControl()
		{
			this.InitializeComponent();

			Container.IsHitTestVisible = true;
			Container.ManipulationStarted += Container_ManipulationStarted;
			Container.ManipulationDelta += Container_ManipulationDelta;
			Container.ManipulationCompleted += Container_ManipulationCompleted;
			Container.RenderTransformOrigin = new Point(0.5, 0.5);
			Container.Tapped += Container_Tapped;
		}

		#region public object Content
		public new object Content
		{
			get { return (object)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
		public new static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register("Content", typeof(object), typeof(ContentZoomControl), new PropertyMetadata(null, OnContentChanged));

		private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentZoomControl c = (ContentZoomControl)d;
			c.Container.Children.Clear();
			c.Container.Children.Add((UIElement)e.NewValue);
		}
		#endregion

		public ContentZoomControlState State
		{
			get
			{
				return manipulationEnabled ? ContentZoomControlState.FreeMoving : ContentZoomControlState.Still;
			}
		}
		public event EventHandler StateChanged;

		public void SetManipulationEnabled(bool value)
		{
			if (value)
			{
				Container.ManipulationMode = ManipulationModes.Scale
					| ManipulationModes.TranslateY
					| ManipulationModes.TranslateInertia
					| ManipulationModes.TranslateX;
			}
			else
			{
				Container.ManipulationMode = ManipulationModes.System;
				previousScale = 1.0;
			}
			manipulationEnabled = value;

			if (StateChanged != null) StateChanged(this, EventArgs.Empty);
		}

		void Container_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			CompositeTransform transform = (CompositeTransform)Container.RenderTransform;

			if (transform.ScaleX < 1)
			{
				SetManipulationEnabled(false);
			}
			else
			{
				previousScale = transform.ScaleX;
			}
		}

		void Container_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			CompositeTransform transform = (CompositeTransform)Container.RenderTransform;

			double scale = (previousScale + e.Cumulative.Scale) - 1;
			if (scale < minScale) scale = minScale;	// Can only zoom in

			transform.ScaleX = scale;
			transform.ScaleY = scale;

			transform.TranslateX += e.Delta.Translation.X;
			transform.TranslateY += e.Delta.Translation.Y;

			// Bound to box
			double objectHeight = Container.ActualHeight * scale;
			double objectWidth = Container.ActualWidth * scale;
			double controlHeight = this.ActualHeight;
			double controlWidth = this.ActualWidth;

			double dX = Math.Max(objectWidth - controlWidth, 0) / 2;
			double dY = Math.Max(objectHeight - controlHeight, 0) / 2;

			if (transform.TranslateX > dX) transform.TranslateX = dX;
			else if (transform.TranslateX < -dX) transform.TranslateX = -dX;

			if (transform.TranslateY > dY) transform.TranslateY = dY;
			else if (transform.TranslateY < -dY) transform.TranslateY = -dY;
		}

		void Container_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
		{

		}

		void Container_Tapped(object sender, TappedRoutedEventArgs e)
		{
			if (!manipulationEnabled)
			{
				SetManipulationEnabled(true);
			}
			else
			{
				ScaleBack.Begin();
				SetManipulationEnabled(false);
			}
		}
	}
}
