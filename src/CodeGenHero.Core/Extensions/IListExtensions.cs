using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
///
/// </summary>
/// <remarks>
/// For original source see: https://raw.githubusercontent.com/Emilien-M/IListExtension/master/src/IListExtension/IListExtension.cs
///
/// </remarks>
namespace System.Collections.Generic
{
	public static class IListExtension
	{
		public static void AddRange<T>(this IList<T> source, IEnumerable<T> newList)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (newList == null)
			{
				throw new ArgumentNullException(nameof(newList));
			}

			if (source is List<T> concreteList)
			{
				concreteList.AddRange(newList);
				return;
			}

			foreach (var element in newList)
			{
				source.Add(element);
			}
		}

		public static IReadOnlyCollection<T> AsReadOnly<T>(this IList<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source is List<T> concreteList)
			{
				return concreteList.AsReadOnly();
			}

			return new ReadOnlyCollection<T>(source);
		}

		public static int BinarySearch<T>(this IList<T> source, int index, int count, T item, IComparer<T> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (index < 0)
			{
				throw new ArgumentNullException(nameof(index));
			}

			if (count < 0)
			{
				throw new ArgumentNullException(nameof(count));
			}

			if (source.Count - index < count)
			{
				throw new ArgumentNullException("invalid length");
			}

			if (source is List<T> concreteList)
			{
				return concreteList.BinarySearch(index, count, item, comparer);
			}

			return Array.BinarySearch(source.ToArray(), index, count, item, comparer);
		}

		public static int BinarySearch<T>(this IList<T> source, T item)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source is List<T> concreteList)
			{
				return concreteList.BinarySearch(item);
			}

			return source.BinarySearch(0, source.Count, item, null);
		}

		public static int BinarySearch<T>(this IList<T> source, T item, IComparer<T> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source is List<T> concreteList)
			{
				return concreteList.BinarySearch(item);
			}

			return source.BinarySearch(0, source.Count, item, comparer);
		}

		public static IList<TOutput> ConvertAll<T, TOutput>(IList<T> source, Converter<T, TOutput> converter)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (converter == null)
			{
				throw new ArgumentNullException(nameof(converter));
			}

			if (source is List<T> concreteList)
			{
				return concreteList.ConvertAll(converter);
			}

			IList<TOutput> list = new List<TOutput>(source.Count);

			for (int i = 0; i < source.Count; i++)
			{
				list[i] = converter(source[i]);
			}

			return list;
		}

		public static bool Exists<T>(this IList<T> source, Predicate<T> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			if (source is List<T> concreteList)
			{
				return concreteList.Exists(predicate);
			}

			foreach (T element in source)
			{
				if (predicate(element))
				{
					return true;
				}
			}

			return false;
		}

		public static T Find<T>(this IList<T> source, Predicate<T> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			if (source is List<T> concreteList)
			{
				return concreteList.Find(predicate);
			}

			foreach (T element in source)
			{
				if (predicate(element))
				{
					return element;
				}
			}

			return default;
		}

		public static IList<T> FindAll<T>(this IList<T> source, Predicate<T> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			if (source is List<T> concreteList)
			{
				return concreteList.FindAll(predicate);
			}

			IList<T> results = new List<T>();

			foreach (T element in source)
			{
				if (predicate(element))
				{
					results.Add(element);
				}
			}

			return results;
		}

		public static int FindIndex<T>(this IList<T> source, Predicate<T> predicate)
		{
			if (source == null || predicate == null)
			{
				return -1;
			}

			if (source is List<T> concreteList)
			{
				return concreteList.RemoveAll(predicate);
			}

			for (int i = 0; i < source.Count; i++)
			{
				if (predicate(source[i]))
				{
					return i;
				}
			}

			return -1;
		}

		public static T FindLast<T>(this IList<T> source, Predicate<T> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			if (source is List<T> concreteList)
			{
				return concreteList.FindLast(predicate);
			}

			T result = default;

			foreach (T element in source)
			{
				if (predicate(element))
				{
					result = element;
				}
			}

			return result;
		}

		public static int FindLastIndex<T>(this IList<T> source, Predicate<T> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			if (source is List<T> concreteList)
			{
				return concreteList.FindLastIndex(predicate);
			}

			int result = -1;

			foreach (T element in source)
			{
				if (predicate(element))
				{
					result = source.IndexOf(element);
				}
			}

			return result;
		}

		public static void ForEach<T>(this IList<T> source, Action<T> action)
		{
			if (source == null || action == null)
			{
				return;
			}

			if (source is List<T> concreteList)
			{
				concreteList.ForEach(action);
				return;
			}

			foreach (var element in source)
			{
				action.Invoke(element);
			}
		}

		public static async Task ForEach<T>(this IList<T> source, Func<T, Task> func)
		{
			if (source == null || func == null)
			{
				return;
			}

			foreach (var element in source)
			{
				await func.Invoke(element);
			}
		}

		public static IList<T> GetRange<T>(this IList<T> source, int index, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (index < 0)
			{
				throw new ArgumentNullException(nameof(index));
			}

			if (count < 0)
			{
				throw new ArgumentNullException(nameof(count));
			}

			if (source.Count - index < count)
			{
				throw new ArgumentNullException("invalid length");
			}

			if (source is List<T> concreteList)
			{
				return concreteList.GetRange(index, count);
			}

			IList<T> results = new List<T>(count);

			Array.Copy(source.ToArray(), index, results.ToArray(), 0, count);

			return results;
		}

		public static int RemoveAll<T>(this IList<T> source, Predicate<T> predicate)
		{
			if (source == null || predicate == null)
			{
				return -1;
			}

			if (source is List<T> concreteList)
			{
				return concreteList.RemoveAll(predicate);
			}

			int result = 0;

			for (int i = source.Count - 1; i >= 0; i--)
			{
				if (!predicate(source[i]))
				{
					continue;
				}

				++result;
				source.RemoveAt(i);
			}

			return result;
		}

		public static void Sort<T>(this IList<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source is List<T> concreteList)
			{
				concreteList.Sort();
				return;
			}

			source.Sort(0, source.Count, null);
		}

		public static void Sort<T>(this IList<T> source, IComparer<T> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source is List<T> concreteList)
			{
				concreteList.Sort(comparer);
				return;
			}

			source.Sort(0, source.Count, comparer);
		}

		public static void Sort<T>(this IList<T> source, int index, int count, IComparer<T> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(index), index, "Can't be less than 0");
			}

			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count), count, "Can't be less than 0");
			}

			if (source.Count - index < count)
			{
				throw new ArgumentOutOfRangeException("Invalid range on the list");
			}

			if (source is List<T> concreteList)
			{
				concreteList.Sort(index, count, comparer);
				return;
			}

			T[] array = source.ToArray();

			Array.Sort(array, index, count, comparer);

#pragma warning disable IDE0059 // Unnecessary assignment of a value
			source = array.ToList();
#pragma warning restore IDE0059 // Unnecessary assignment of a value
		}

		public static void Sort<T>(this IList<T> source, Comparison<T> comparison)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (comparison == null)
			{
				throw new ArgumentNullException(nameof(comparison));
			}

			if (source is List<T> concreteList)
			{
				concreteList.Sort(comparison);
				return;
			}

			IComparer<T> comparer = new FunctorComparer<T>(comparison);
			source.Sort(0, source.Count, comparer);
		}

		public static bool TrueForAll<T>(this IList<T> source, Predicate<T> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			if (source is List<T> concreteList)
			{
				return concreteList.TrueForAll(predicate);
			}

			foreach (T element in source)
			{
				if (!predicate(element))
				{
					return false;
				}
			}

			return true;
		}

		//InsertRange
		//RemoveRange
	}
}