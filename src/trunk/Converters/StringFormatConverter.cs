using System;
using Windows.UI.Xaml.Data;

namespace ree7.Utils.Converters
{
	public class StringFormatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			// No format provided.
			if (parameter == null)
			{
				return value;
			}

			if (value is DateTime)
			{
				return ((DateTime)value).ToString((string)parameter);
			}
			else
			{
				return String.Format((String)parameter, value);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return value;
		}
	}
}
