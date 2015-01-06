using System;
using Windows.UI.Xaml.Data;

namespace ree7.Utils.Converters
{
	public class BoolOpacityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			double opacity = parameter != null ? double.Parse((string)parameter, System.Globalization.CultureInfo.InvariantCulture) : 0.0;
			bool b = (bool)value;

			return b ? opacity : 1.0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
