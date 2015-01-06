using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace ree7.Utils.Converters
{
	/// <summary>
	/// Converts a DateTime into a UI friendly day name.
	/// If input is today, tomorrow or yesterday, will display as such. Else will display the day name and number.
	/// Formatting can be customized with the converter parameter, which accepts a DateTime format string.
	/// </summary>
	/// <author>Pierre Belin - pierre@ree7.fr</author>
	public class DayRelativeConverter : IValueConverter
	{
		// Resw file keys can be customized
		public static string ResourceDayToday = "DayToday";
		public static string ResourceDayTomorrow = "DayTomorrow";

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string formatString = parameter as string ?? "dddd dd";
			return DayRelativeConverter.Convert((DateTime)value, formatString);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		public static string Convert(DateTime date, string formatString = "dddd dd")
		{
			ResourceLoader R = new Windows.ApplicationModel.Resources.ResourceLoader("ree7.Utils/Resources");

			DateTime today = DateTime.Now.Date;

			if (date.Date == today.Date)
				return R.GetString(ResourceDayToday);
			else if (date.Date == today.AddDays(1).Date)
				return R.GetString(ResourceDayTomorrow);
			else
				return date.ToString(formatString);
		}
	}
}
