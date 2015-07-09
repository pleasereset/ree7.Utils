using System;

namespace ree7.Utils
{
	public static class StringExtensions
	{
		public static Uri ToAbsoluteUri(this string s)
		{
			if (string.IsNullOrWhiteSpace(s)) return null;
			return new Uri(s, UriKind.Absolute);
		}
	}
}
