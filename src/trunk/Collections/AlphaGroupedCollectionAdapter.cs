
namespace ree7.Utils.Collections
{
	public class AlphaGroupedCollectionAdapter : GroupedCollectionAdapter
	{
		public AlphaGroupedCollectionAdapter()
		{
			grouperGenericType = typeof(AlphaCollectionGrouper<>);
		}
	}

	internal class AlphaCollectionGrouper<T> : CollectionGrouper<T>
	{
		const string KeyForDigit = "#";
		const string KeyForOther = "?";

		protected override string GetKeyForItem(T obj, string propertyName)
		{
			string value = base.GetKeyForItem(obj, propertyName);
			if (value.Length > 0)
			{
				char c = char.ToUpperInvariant(value[0]);

				if (c >= 'A' && c <= 'Z') return c.ToString();
				if (c >= '0' && c <= '9') return KeyForDigit;
			}
			return KeyForOther;
		}
	}
}
