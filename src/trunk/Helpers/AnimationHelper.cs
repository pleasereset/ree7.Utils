using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace ree7.Utils.Helpers
{
	public static class AnimationHelper
	{
		public static Task BeginAsync(Storyboard sb)
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			EventHandler<object> completionCallback = null;
			completionCallback = (s, e) =>
			{
				((Storyboard)s).Completed -= completionCallback;
				tcs.SetResult(null);
			};
			sb.Completed += completionCallback;
			sb.Begin();

			return tcs.Task;
		}
	}
}
