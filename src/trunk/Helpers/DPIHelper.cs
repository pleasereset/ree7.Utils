/**************************************************************************
Module Name:  DPIHelper
Author : Pierre BELIN <pierre@ree7.fr>
 
This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.
 
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
***************************************************************************/

using Windows.Graphics.Display;

namespace ree7.Utils.Helpers
{
	public static class DPIHelper
	{
		static double? currentRatio = null;

		static DPIHelper()
		{
			var di = DisplayInformation.GetForCurrentView();
			di.DpiChanged += OnDpiChanged;
		}

		// Lazy initialization of the ratio
		private static void LoadCurrentRatio()
		{
			var di = DisplayInformation.GetForCurrentView();
			switch (di.ResolutionScale)
			{
				case ResolutionScale.Scale120Percent:
					currentRatio = 1.2; break;
				case ResolutionScale.Scale140Percent:
					currentRatio = 1.4; break;
				case ResolutionScale.Scale150Percent:
					currentRatio = 1.5; break;
				case ResolutionScale.Scale160Percent:
					currentRatio = 1.6; break;
				case ResolutionScale.Scale180Percent:
					currentRatio = 1.8; break;
				case ResolutionScale.Scale225Percent:
					currentRatio = 2.25; break;
				default:
					currentRatio = 1.0; break;
			}
		}

		private static void OnDpiChanged(DisplayInformation sender, object args)
		{
			LoadCurrentRatio();
		}

		public static double ConvertToPhysicalPixels(double pixel)
		{
			if (currentRatio.HasValue == false) LoadCurrentRatio();			

			return pixel * currentRatio.Value;
		}
	}
}
