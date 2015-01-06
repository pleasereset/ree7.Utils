using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace ree7.Utils.Controls
{
	/// <summary>
	/// FlipViewIndicator is a companion control to be used exclusively with a FlipView control. It serves
	/// the purpose of providing some hinting UI to the user where they are in the navigation of items
	/// within a FlipView. This is similar UI as seen in the Windows Store application when viewing the 
	/// screenshots.
	/// </summary>
	/// <remarks>
	/// The best usage is to be immediately underneath a FlipView and this is easily accomplished by using 
	/// a <see cref="StackPanel"/> as demonstrated below in Usage. When done this way the margins of the 
	/// FlipViewIndicator are set correctly. If using in other means, you may need to adjust margins on
	/// the indicator.
	/// </remarks>
	public sealed class FlipViewIndicator : ListBox
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FlipViewIndicator"/> class.
		/// </summary>
		public FlipViewIndicator()
		{
			this.DefaultStyleKey = typeof(FlipViewIndicator);
			this.IsHitTestVisible = false;
		}

		/// <summary>
		/// Gets or sets the flip view.
		/// </summary>
		public FlipView FlipView
		{
			get { return (FlipView)GetValue(FlipViewProperty); }
			set { SetValue(FlipViewProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="FlipView"/> dependency property
		/// </summary>
		public static readonly DependencyProperty FlipViewProperty =
			DependencyProperty.Register("FlipView", typeof(FlipView), typeof(FlipViewIndicator), new PropertyMetadata(null, (depobj, args) =>
			{
				FlipViewIndicator fvi = (FlipViewIndicator)depobj;
				FlipView fv = (FlipView)args.NewValue;

				// this is a special case where ItemsSource is set in code
				// and the associated FlipView's ItemsSource may not be available yet
				// if it isn't available, let's listen for SelectionChanged 
				if (fv.ItemsSource == null && fv.Items == null)
				{
					SelectionChangedEventHandler sceh = null;
					sceh =  (s, e) =>
					{
						if (fvi.ItemsSource == null)
						{
							fvi.ItemsSource = fv.ItemsSource;
							fv.SelectionChanged -= sceh;
						}
					};
					fv.SelectionChanged += sceh;					
				}

				fv.SelectionChanged += (s, e) =>
				{
					if (fv.SelectedIndex >= 0) fvi.SelectedIndex = fv.SelectedIndex;
				};

				if (fv.ItemsSource != null) 
				{ 
					fvi.ItemsSource = fv.ItemsSource;
				}
				else if (fv.Items != null) 
				{ 
					foreach(var item in fv.Items)
					{
						fvi.Items.Add("placeholder");
					}
				}

				if (fv.SelectedIndex >= 0) fvi.SelectedIndex = fv.SelectedIndex;
			}));
	}
}