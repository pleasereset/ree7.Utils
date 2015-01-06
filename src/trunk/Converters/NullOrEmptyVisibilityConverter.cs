using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ree7.Utils.Converters
{
	/// <summary>
	/// Default behavior : when value is not null, output Visible, when value is null, output Collapsed.
	/// Behavior is inversed if the parameter is 'inverse'.
	/// </summary>
	public class NullOrEmptyVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			bool visible = true;

			if (value == null)
			{
				visible = false;
			}
			else if(value is string)
			{
				if(String.IsNullOrWhiteSpace((string)value))
					visible = false;
			}

			if(parameter is string && ((string)parameter) == "inverse")
			{
				visible = !visible;
			}

			return visible ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
