using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ree7.Utils.Converters
{
	public class DoubleVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			double d = (double)value;
			return Double.IsNaN(d) ? Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
