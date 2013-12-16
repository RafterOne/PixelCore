using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace PixelMEDIA.PixelCore.Helpers
{
	/// <summary>
	/// Helper library for manipulating collections.
	/// </summary>
	public static class CollectionHelper
	{
		/// <summary>
		/// Returns true if the value is included in the secondary parameters.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static bool CollectionContains<T>(T value, params T[] collection)
		{
			return (collection.Contains(value));
		}

		/// <summary>
		/// Returns true if the value is included in the collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static bool CollectionContains<T>(T value, IEnumerable<T> collection)
		{
			return (collection.Contains(value));
		}

		/// <summary>
		/// Take a page of a given size from the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public static IEnumerable<T> TakePage<T>(this IEnumerable<T> collection, int pageNumber, int pageSize)
		{
			return collection.Skip((pageNumber - 1) * pageSize).Take(pageSize);
		}

        /// <summary>
        /// Order by ascending or descending by passing a boolean value.
        /// </summary>
        /// <typeparam name="TSource">The type of the IQueryable.</typeparam>
        /// <typeparam name="TKey">The type of the expression.</typeparam>
        /// <param name="source">The IQueryable to be sorted.</param>
        /// <param name="keySelector">The sort expression.</param>
        /// <param name="reverse">Whether to reverse the sort (aka OrderByDescending).</param>
        /// <returns>The IOrderedQueryable sorted by the expression.</returns>
		public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool reverse)
		{
			if (reverse)
			{
				return source.OrderByDescending(keySelector);
			}
			else
			{
				return source.OrderBy(keySelector);
			}
		}
        
		/// <summary>
		/// Gets the item at the wrapped position. Useful if you want to round robin a list of unknown size.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static T GetNthRoundRobinItem<T>(IList<T> list, int position)
		{
			var listCount = list.Count();

			if (position < 0)
			{
				position = listCount + (position % listCount);
			}

			return list[position % listCount];
		}

		/// <summary>
		/// Returns a list of items between the first and last item, inclusive.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="last"></param>
		/// <returns></returns>
		public static List<int> InclusiveRangeList(int first, int last)
		{
			var size = last - first;
			var list = new List<int>();
			for (var i = first; i <= last; i++)
			{
				list.Add(i);
			}
			return list;
		}

		/// <summary>
		/// Returns a list of items between 0 and the item, inclusive.
		/// </summary>
		/// <param name="last"></param>
		/// <returns></returns>
		public static List<int> InclusiveRangeList(int last)
		{
			return InclusiveRangeList(0, last);
		}

		/// <summary>
		/// Returns an array of items between the first and last item, inclusive.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="last"></param>
		/// <returns></returns>
		public static int[] InclusiveRangeArray(int first, int last)
		{
			return InclusiveRangeList(first, last).ToArray();
		}

		/// <summary>
		/// Returns an array of items between 0 and the item, inclusive.
		/// </summary>
		/// <param name="last"></param>
		/// <returns></returns>
		public static int[] InclusiveRangeArray(int last)
		{
			return InclusiveRangeList(0, last).ToArray();
		}

		/// <summary>
		/// Makes an array from the arguments.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <returns></returns>
		public static T[] MakeArray<T>(params T[] args)
		{
			return args;
		}

		/// <summary>
		/// Makes a generic list from the arguments.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <returns></returns>
		public static List<T> MakeList<T>(params T[] args)
		{
			return args.ToList();
		}

	}
}
