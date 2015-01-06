using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;

namespace ree7.Utils.Collections
{
	public interface IIncrementalLoadingCollection
	{
		/// <summary>
		/// Is the collection currently loading more items.
		/// </summary>
		bool IsLoading { get; }

		/// <summary>
		/// Triggered when the value of IsLoading changes
		/// </summary>
		event EventHandler IsLoadingChanged;

		/// <summary>
		/// 
		/// </summary>
		Task<int> TryLoadMoreItems();
	}

	public class IncrementalLoadingCollection<T> : ObservableCollection<T>, IIncrementalLoadingCollection, ISupportIncrementalLoading
	{
		private const int MaxItemsNoLimit = -1;
		private const int PageSizeDefaultValue = 50;

		private int currentOffset;
		private bool isLoading = false;

		public IncrementalLoadingCollection()
		{
			PageSize = PageSizeDefaultValue;
			MaxItems = MaxItemsNoLimit;
			Dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
		}

		/// <summary>
		/// Is the collection currently loading more items.
		/// </summary>
		public bool IsLoading
		{
			get
			{
				return isLoading;
			}
			private set
			{
				if (isLoading != value)
				{
					isLoading = value;

					var notAwaited = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
					{
						if (IsLoadingChanged != null) IsLoadingChanged(this, EventArgs.Empty);
					});
				}
			}
		}
		public event EventHandler IsLoadingChanged;

		/// <summary>
		/// The maximum item count that the collection can have. 
		/// Optional, and should be manually populated when the remote web service exposes it
		/// </summary>
		public volatile int _maxItems;
		public int MaxItems
		{
			get { return _maxItems; }
			set { _maxItems = value; }
		}

		/// <summary>
		/// Holds the function that provides the Collections with more items when needed.
		/// Function signature is :
		///    async Task<IEnumerable<T>> GetMoreItemsAsync(int pageSize, int offset)
		/// </summary>
		public Func<int, int, Task<IEnumerable<T>>> ItemsProviderAsyncFunction { get; set; }

		/// <summary>
		/// Size of the paging feature
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		/// Dispatcher thread used for all manipulations on the ObservableCollection
		/// </summary>
		public CoreDispatcher Dispatcher { get; set; }


		/// <summary>
		/// Called by the view when more items are needed
		/// </summary>
		public async Task<int> TryLoadMoreItems()
		{
			try
			{
				int addedItemsCount = 0;

				if (ItemsProviderAsyncFunction != null
					&& !IsLoading && HasMoreItems)
				{
					IsLoading = true;

					var newItems = await ItemsProviderAsyncFunction(PageSize, currentOffset);
					if (newItems != null)
					{
						int newCount = newItems.Count();

						if (newCount == 0 && MaxItems == MaxItemsNoLimit)
						{
							// We've just hit the max item count
							MaxItems = this.Count;
						}
						else
						{
							currentOffset += newCount;

							await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
								{
									foreach (var item in newItems)
									{
										addedItemsCount += 1;
										this.Add(item);
									}
								});
						}
					}

					IsLoading = false;
				}

				return addedItemsCount;
			}
			finally
			{
				_busy = false;
			}
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnCollectionChanged(e);
		}

		#region ISupportIncrementalLoading
		public bool HasMoreItems
		{
			get { return (MaxItems == MaxItemsNoLimit || MaxItems > this.Count); }
		}

		private bool _busy = false;

		public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
		{
			if (_busy)
				throw new InvalidOperationException("Only one operation in flight at a time");

			_busy = true;

			var currentTask = Task.Run<LoadMoreItemsResult>(async () =>
			{
				int realCount = await TryLoadMoreItems();
				return new LoadMoreItemsResult() { Count = (uint)realCount };
			});

			return currentTask.AsAsyncOperation<LoadMoreItemsResult>();
		}
		#endregion
	}
}
