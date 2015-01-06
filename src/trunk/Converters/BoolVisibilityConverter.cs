using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ree7.Utils.Converters
{
	public class BoolVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (parameter is string && String.Equals(parameter, "inverse")) value = !(bool)value;
			return (bool)value ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
