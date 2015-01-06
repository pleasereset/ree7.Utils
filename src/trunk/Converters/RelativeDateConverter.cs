// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.
//
// Ported from Silverlight to Windows Runtime by Pierre BELIN
// <pierre@ree7.fr>

using Microsoft.Phone.Controls;
using System;
using System.Globalization;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace ree7.Utils.Converters
{
	/// <summary>
	/// Time converter to display elapsed time relatively to the present.
	/// </summary>
	/// <QualityBand>Preview</QualityBand>
	public class RelativeTimeConverter : IValueConverter
	{
		/// <summary>
		/// A minute defined in seconds.
		/// </summary>
		private const double Minute = 60.0;

		/// <summary>
		/// An hour defined in seconds.
		/// </summary>
		private const double Hour = 60.0 * Minute;

		/// <summary>
		/// A day defined in seconds.
		/// </summary>
		private const double Day = 24 * Hour;

		/// <summary>
		/// A week defined in seconds.
		/// </summary>
		private const double Week = 7 * Day;

		/// <summary>
		/// A month defined in seconds.
		/// </summary>
		private const double Month = 30.5 * Day;

		/// <summary>
		/// A year defined in seconds.
		/// </summary>
		private const double Year = 365 * Day;

		/// <summary>
		/// Abbreviation for the default culture used by resources files.
		/// </summary>
		private const string DefaultCulture = "en-US";

		/// <summary>
		/// Four different strings to express hours in plural.
		/// </summary>
		private string[] PluralHourStrings;

		/// <summary>
		/// Four different strings to express minutes in plural.
		/// </summary>
		private string[] PluralMinuteStrings;

		/// <summary>
		/// Four different strings to express seconds in plural.
		/// </summary>
		private string[] PluralSecondStrings;

		/// <summary>
		/// Provides access to the text resources used by this converter
		/// </summary>
		private ResourceLoader R = new Windows.ApplicationModel.Resources.ResourceLoader("ree7.Utils/RelativeDateConverter");

		/// <summary>
		/// Resources use the culture in the system locale by default.
		/// The converter must use the culture specified the ConverterCulture.
		/// The ConverterCulture defaults to en-US when not specified.
		/// Thus, change the resources culture only if ConverterCulture is set.
		/// </summary>
		/// <param name="culture">The culture to use in the converter.</param>
		private void SetLocalizationCulture(CultureInfo culture)
		{
			if (!culture.Name.Equals(DefaultCulture, StringComparison.Ordinal))
			{
				//ControlResources.Culture = culture;
			}

			PluralHourStrings = new string[4] { 
                  R.GetString("XHoursAgo_2To4"), 
                  R.GetString("XHoursAgo_EndsIn1Not11"), 
                  R.GetString("XHoursAgo_EndsIn2To4Not12To14"), 
                  R.GetString("XHoursAgo_Other") 
              };

			PluralMinuteStrings = new string[4] { 
                  R.GetString("XMinutesAgo_2To4"), 
                  R.GetString("XMinutesAgo_EndsIn1Not11"), 
                  R.GetString("XMinutesAgo_EndsIn2To4Not12To14"), 
                  R.GetString("XMinutesAgo_Other") 
              };

			PluralSecondStrings = new string[4] { 
                  R.GetString("XSecondsAgo_2To4"), 
                  R.GetString("XSecondsAgo_EndsIn1Not11"), 
                  R.GetString("XSecondsAgo_EndsIn2To4Not12To14"), 
                  R.GetString("XSecondsAgo_Other") 
              };
		}

		/// <summary>
		/// Returns a localized text string to express months in plural.
		/// </summary>
		/// <param name="month">Number of months.</param>
		/// <returns>Localized text string.</returns>
		private string GetPluralMonth(int month)
		{
			if (month >= 2 && month <= 4)
			{
				return string.Format(R.GetString("XMonthsAgo_2To4"), month.ToString(CultureInfo.CurrentUICulture));
			}
			else if (month >= 5 && month <= 12)
			{
				return string.Format(R.GetString("XMonthsAgo_5To12"), month.ToString(CultureInfo.CurrentUICulture));
			}
			else
			{
				throw new ArgumentException("Properties.Resources.InvalidNumberOfMonths");
			}
		}

		/// <summary>
		/// Returns a localized text string to express time units in plural.
		/// </summary>
		/// <param name="units">
		/// Number of time units, e.g. 5 for five months.
		/// </param>
		/// <param name="resources">
		/// Resources related to the specified time unit.
		/// </param>
		/// <returns>Localized text string.</returns>
		private string GetPluralTimeUnits(int units, string[] resources)
		{
			int modTen = units % 10;
			int modHundred = units % 100;

			if (units <= 1)
			{
				throw new ArgumentException("Properties.Resources.InvalidNumberOfTimeUnits");
			}
			else if (units >= 2 && units <= 4)
			{
				return string.Format(resources[0], units.ToString(CultureInfo.CurrentUICulture));
			}
			else if (modTen == 1 && modHundred != 11)
			{
				return string.Format(resources[1], units.ToString(CultureInfo.CurrentUICulture));
			}
			else if ((modTen >= 2 && modTen <= 4) && !(modHundred >= 12 && modHundred <= 14))
			{
				return string.Format(resources[2], units.ToString(CultureInfo.CurrentUICulture));
			}
			else
			{
				return string.Format(resources[3], units.ToString(CultureInfo.CurrentUICulture));
			}
		}

		/// <summary>
		/// Returns a localized text string for the "ast" + "day of week as {0}".
		/// </summary>
		/// <param name="dow">Last Day of week.</param>
		/// <returns>Localized text string.</returns>
		private string GetLastDayOfWeek(DayOfWeek dow)
		{
			string result;

			switch (dow)
			{
				case DayOfWeek.Monday:
					result = R.GetString("lastMonday");
					break;
				case DayOfWeek.Tuesday:
					result = R.GetString("lastTuesday");
					break;
				case DayOfWeek.Wednesday:
					result = R.GetString("lastWednesday");
					break;
				case DayOfWeek.Thursday:
					result = R.GetString("lastThursday");
					break;
				case DayOfWeek.Friday:
					result = R.GetString("lastFriday");
					break;
				case DayOfWeek.Saturday:
					result = R.GetString("lastSaturday");
					break;
				case DayOfWeek.Sunday:
					result = R.GetString("lastSunday");
					break;
				default:
					result = R.GetString("lastSunday");
					break;
			}

			return result;
		}


		/// <summary>
		/// Returns a localized text string to express "on {0}"
		/// where {0} is a day of the week, e.g. Sunday.
		/// </summary>
		/// <param name="dow">Day of week.</param>
		/// <returns>Localized text string.</returns>
		private string GetOnDayOfWeek(DayOfWeek dow)
		{
			string result;

			switch (dow)
			{
				case DayOfWeek.Monday:
					result = R.GetString("onMonday");
					break;
				case DayOfWeek.Tuesday:
					result = R.GetString("onTuesday");
					break;
				case DayOfWeek.Wednesday:
					result = R.GetString("onWednesday");
					break;
				case DayOfWeek.Thursday:
					result = R.GetString("onThursday");
					break;
				case DayOfWeek.Friday:
					result = R.GetString("onFriday");
					break;
				case DayOfWeek.Saturday:
					result = R.GetString("onSaturday");
					break;
				case DayOfWeek.Sunday:
					result = R.GetString("onSunday");
					break;
				default:
					result = R.GetString("onSunday");
					break;
			}

			return result;
		}

		/// <summary>
		/// Converts a 
		/// <see cref="T:System.DateTime"/>
		/// object into a string the represents the elapsed time 
		/// relatively to the present.
		/// </summary>
		/// <param name="value">The given date and time.</param>
		/// <param name="targetType">
		/// The type corresponding to the binding property, which must be of
		/// <see cref="T:System.String"/>.
		/// </param>
		/// <param name="parameter">(Not used).</param>
		/// <param name="language">
		/// The language to use in the converter.
		/// When not specified, the converter uses the current language
		/// as specified by the system locale.
		/// </param>
		/// <returns>The given date and time as a string.</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			// Target value must be a System.DateTime object.
			if (!(value is DateTime))
			{
				throw new ArgumentException("Properties.Resources.InvalidDateTimeArgument");
			}

			string result;

			DateTime given = ((DateTime)value).ToLocalTime();

			DateTime current = DateTime.Now;

			TimeSpan difference = current - given;

			SetLocalizationCulture(new CultureInfo(language));

			if (DateTimeFormatHelper.IsFutureDateTime(current, given))
			{
				// Future dates and times are not supported, but to prevent crashing an app
				// if the time they receive from a server is slightly ahead of the phone's clock
				// we'll just default to the minimum, which is "2 seconds ago".
				result = GetPluralTimeUnits(2, PluralSecondStrings);
			}

			if (difference.TotalSeconds > Year)
			{
				// "over a year ago"
				result = R.GetString("OverAYearAgo");
			}
			else if (difference.TotalSeconds > (1.5 * Month))
			{
				// "x months ago"
				int nMonths = (int)((difference.TotalSeconds + Month / 2) / Month);
				result = GetPluralMonth(nMonths);
			}
			else if (difference.TotalSeconds >= (3.5 * Week))
			{
				// "about a month ago"
				result = R.GetString("AboutAMonthAgo");
			}
			else if (difference.TotalSeconds >= Week)
			{
				int nWeeks = (int)(difference.TotalSeconds / Week);
				if (nWeeks > 1)
				{
					// "x weeks ago"
					result = string.Format(R.GetString("XWeeksAgo_2To4"), nWeeks.ToString(CultureInfo.CurrentUICulture));
				}
				else
				{
					// "about a week ago"
					result = R.GetString("AboutAWeekAgo");
				}
			}
			else if (difference.TotalSeconds >= (5 * Day))
			{
				// "last <dayofweek>"    
				result = GetLastDayOfWeek(given.DayOfWeek);
			}
			else if (difference.TotalSeconds >= Day)
			{
				// "on <dayofweek>"
				result = GetOnDayOfWeek(given.DayOfWeek);
			}
			else if (difference.TotalSeconds >= (2 * Hour))
			{
				// "x hours ago"
				int nHours = (int)(difference.TotalSeconds / Hour);
				result = GetPluralTimeUnits(nHours, PluralHourStrings);
			}
			else if (difference.TotalSeconds >= Hour)
			{
				// "about an hour ago"
				result = R.GetString("AboutAnHourAgo");
			}
			else if (difference.TotalSeconds >= (2 * Minute))
			{
				// "x minutes ago"
				int nMinutes = (int)(difference.TotalSeconds / Minute);
				result = GetPluralTimeUnits(nMinutes, PluralMinuteStrings);
			}
			else if (difference.TotalSeconds >= Minute)
			{
				// "about a minute ago"
				result = R.GetString("AboutAMinuteAgo");
			}
			else
			{
				// "x seconds ago" or default to "2 seconds ago" if less than two seconds.
				int nSeconds = ((int)difference.TotalSeconds > 1.0) ? (int)difference.TotalSeconds : 2;
				result = GetPluralTimeUnits(nSeconds, PluralSecondStrings);
			}

			return result;
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		/// <param name="value">(Not used).</param>
		/// <param name="targetType">(Not used).</param>
		/// <param name="parameter">(Not used).</param>
		/// <param name="culture">(Not used).</param>
		/// <returns>null</returns>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
