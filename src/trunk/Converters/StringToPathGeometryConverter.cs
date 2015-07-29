using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ree7.Utils.Converters
{
    /// <summary>
    /// == EXPERIMENTAL ==
    /// Ported from Silverlight 2 project :http://stringtopathgeometry.codeplex.com/
    /// as Windows Runtime does not expose Geometry.Parse (seriously Microsoft, what the fuck ?)
    /// </summary>
    public class StringToPathGeometryConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string path = value as string;
			if (null != path)
				return new PathGeometryConverter().Convert(path);
			else
				return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			PathGeometry geometry = value as PathGeometry;

			if (null != geometry)
				return new PathGeometryConverter().ConvertBack(geometry);
			else
				return default(string);
		}

		#endregion

	}
}