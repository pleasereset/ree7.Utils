using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ree7.Utils.Controls
{
    public sealed partial class RatingControl : UserControl
	{
        private bool loaded;


        #region public bool IsReadOnly
        /// <summary>
        /// Is the value modifiable (by touching) by the user ?
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(RatingControl), new PropertyMetadata(true, OnIsReadOnlyChanged));

        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RatingControl control = (RatingControl)d;
            control.IsHitTestVisible = !(bool)e.NewValue;
        }
        #endregion

        #region public Brush Hollow
        /// <summary>
        /// Brush for the hollow (un-filled) symbols
        /// </summary>
        public Brush Hollow
        {
            get { return (Brush)GetValue(HollowProperty); }
            set { SetValue(HollowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hollow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HollowProperty =
            DependencyProperty.Register("Hollow", typeof(Brush), typeof(RatingControl), new PropertyMetadata(null, OnHollowChanged));

        private static void OnHollowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RatingControl)d).UpdateSymbol();
        }
        #endregion

        #region public string Symbol
        /// <summary>
        /// Use a PathGeometry syntax here. 
        /// Dev note : Can't use a PathGeometry here as those can't be shared across multiple Paths.
        /// </summary>
        public string Symbol
        {
            get { return (string)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Symbol.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register("Symbol", typeof(Geometry), typeof(RatingControl), new PropertyMetadata(null, OnSymbolChanged));

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RatingControl)d).UpdateSymbol();
        }
        #endregion

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
			((RatingControl)d).UpdateSymbol();
		}
        #endregion

        #region public double SymbolSpacing
        /// <summary>
        /// Space between two symbols
        /// </summary>
        public double SymbolSpacing
        {
            get { return (double)GetValue(SymbolSpacingProperty); }
            set { SetValue(SymbolSpacingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SymbolSpacing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SymbolSpacingProperty =
            DependencyProperty.Register("SymbolSpacing", typeof(double), typeof(RatingControl), new PropertyMetadata(0));

        #endregion

        #region public double Value
        public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(double), typeof(RatingControl), new PropertyMetadata((double)0.0, OnValueChanged));

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            RatingControl control = (RatingControl)d;
            control.UpdateValue();
            if (control.ValueChanged != null) control.ValueChanged(control, EventArgs.Empty);
		}
        #endregion

        public event EventHandler ValueChanged;

        public RatingControl()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
            UpdateSymbol();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            loaded = false;
        }

        private int GetStarsCount()
		{
			return HollowContainer.Children.Count;
		}

		private void UpdateSymbol()
		{
            if (!loaded) return;

            // Used to generate multiple PathGeometry
            PathGeometryConverter pconv = new PathGeometryConverter();

            for(int i = 0; i < HollowContainer.Children.Count; i++)
            {
                Path p = (Path)HollowContainer.Children[i];
                p.Height = SymbolSize;
                p.Width = SymbolSize;
                if(i != HollowContainer.Children.Count - 1) p.Margin = new Thickness(0, 0, SymbolSpacing, 0);
                if (Hollow != null) p.Fill = Hollow;
                if (Symbol != null) p.Data = pconv.Convert(Symbol);
            }
            for (int i = 0; i < FilledContainer.Children.Count; i++)
            {
                Path p = (Path)FilledContainer.Children[i];
                p.Height = SymbolSize;
                p.Width = SymbolSize;
                if (i != HollowContainer.Children.Count - 1) p.Margin = new Thickness(0, 0, SymbolSpacing, 0);
                if (Symbol != null) p.Data = pconv.Convert(Symbol);
            }

            UpdateValue();
		}

		private void UpdateValue()
		{
            if (!loaded) return;

            try
            {
                int starsCount = GetStarsCount();
                double value = this.Value / starsCount; // percentage

                double fullWidth = FilledContainer.ActualWidth;
                RectangleGeometry clip = new RectangleGeometry()
                {
                    Rect = new Rect(0, 0, fullWidth * value, SymbolSize)
                };
                FilledContainer.Clip = clip;
            }
            catch { }
		}

        private void OnSymbolTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Path source = (Path)sender;
            Panel container = (Panel)source.Parent;

            int index = container.Children.IndexOf(source);
            Value = index + 1;
        }
    }
}
