using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace ree7.Utils.Controls
{
	/// <summary>
	/// A TextBlock that slide its content when the container size cannot fit
	/// the whole text
	/// Note : Only supports LeftToRight layout so far.
	/// </summary>
	public sealed partial class SlidingTextBlock : UserControl
	{
		private Storyboard Animation;

		public SlidingTextBlock()
		{
			this.InitializeComponent();
			this.Loaded += SlidingTextBlock_Loaded;
			this.SizeChanged += SlidingTextBlock_SizeChanged;
			this.Tapped += SlidingTextBlock_Tapped;
		}

		void SlidingTextBlock_Loaded(object sender, RoutedEventArgs e)
		{
			if(AnimateOnLoaded) Animate(2000);
		}

		void SlidingTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
		{
			Animate();
		}

		void SlidingTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			UpdateControl();
		}

		public bool AnimateOnLoaded { get; set; }

		#region public string Padding
		public new Thickness Padding
		{
			get { return (Thickness)GetValue(PaddingProperty); }
			set { SetValue(PaddingProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty PaddingProperty =
			DependencyProperty.Register("Padding", typeof(Thickness), typeof(SlidingTextBlock), new PropertyMetadata(new Thickness(), OnPaddingChanged));

		private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SlidingTextBlock stb = (SlidingTextBlock)d;
			stb.InnerTextBlock.Margin = (Thickness)e.NewValue;
			stb.UpdateControl();
		}
		#endregion

		#region public int ScrollSpeed
		/// <summary>
		/// Text sliding speed, in milliseconds per pixel
		/// </summary>
		public int ScrollSpeed
		{
			get { return (int)GetValue(ScrollSpeedProperty); }
			set { SetValue(ScrollSpeedProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ScrollSpeed.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ScrollSpeedProperty =
			DependencyProperty.Register("ScrollSpeed", typeof(int), typeof(SlidingTextBlock), new PropertyMetadata(15));
		#endregion
		
		#region public string Text
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(SlidingTextBlock), new PropertyMetadata(String.Empty, OnTextChanged));

		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SlidingTextBlock stb = (SlidingTextBlock)d;
			stb.InnerTextBlock.Text = (string)e.NewValue;
			stb.UpdateControl();
		}
		#endregion

		public async void Animate(int delay = 0)
		{
			if(delay > 0) await Task.Delay(delay);
			if (Animation != null) Animation.Begin();
		}

		private void UpdateControl()
		{
			var controlWidth = this.ActualWidth;
			var textWidth = InnerTextBlock.DesiredSize.Width;

			if(textWidth > controlWidth)
			{
				double delta = textWidth - controlWidth;
				double duration = delta * ScrollSpeed;

				DoubleAnimation da = new DoubleAnimation()
				{
					From = 0,
					To = -delta,
					Duration = TimeSpan.FromMilliseconds(duration)
				};
				Storyboard.SetTarget(da, InnerTextBlock);
				Storyboard.SetTargetProperty(da, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");

				Storyboard sb = new Storyboard();
				sb.Children.Add(da);
				sb.AutoReverse = true;

				Animation = sb;
			}
			else
			{
				((CompositeTransform)InnerTextBlock.RenderTransform).TranslateX = 0.0;
				Animation = null;
			}
		}
	}
}
