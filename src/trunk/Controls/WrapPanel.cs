﻿using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace ree7.Utils.Controls
{
    /// <summary>
    /// Panel that flows its children and wraps then once reached the maximum width.
    /// No grid-like layout like in the native ItemsWrapPanel.
    /// </summary>
    public class WrapPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            // Just take up all of the width
            Size finalSize = new Size { Width = availableSize.Width };

            double x = 0;
            double rowHeight = 0d;
            foreach (var child in Children)
            {
                // Tell the child control to determine the size needed
                child.Measure(availableSize);

                x += child.DesiredSize.Width;
                if (x > availableSize.Width)
                {
                    // this item will start the next row
                    x = child.DesiredSize.Width;

                    // adjust the height of the panel
                    finalSize.Height += rowHeight;
                    rowHeight = child.DesiredSize.Height;
                }
                else
                {
                    // Get the tallest item
                    rowHeight = Math.Max(child.DesiredSize.Height, rowHeight);
                }
            }

            // Just in case we only had one row
            if (finalSize.Height == 0)
            {
                finalSize.Height = rowHeight;
            }
            return finalSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect finalRect = new Rect(0, 0, finalSize.Width, finalSize.Height);

            double rowHeight = 0;
            foreach (var child in Children)
            {
                if ((child.DesiredSize.Width + finalRect.X) > finalSize.Width)
                {
                    // next row!
                    finalRect.X = 0;
                    finalRect.Y += rowHeight;
                    rowHeight = 0;
                }
                // Place the item
                child.Arrange(finalRect);

                // adjust the location for the next items
                finalRect.X += child.DesiredSize.Width;
                rowHeight = Math.Max(child.DesiredSize.Height, rowHeight);
            }
            return finalSize;
        }
    }
}