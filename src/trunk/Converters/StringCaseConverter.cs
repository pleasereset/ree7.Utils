using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ree7.Utils.Converters
{
	public class StringCaseConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is string && parameter is string)
			{
				string arg = (string)parameter;
				string val = (string)value;

				if (arg.ToLowerInvariant() == "lower") return val.ToLowerInvariant();
				if (arg.ToLowerInvariant() == "upper") return val.ToUpperInvariant();
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
