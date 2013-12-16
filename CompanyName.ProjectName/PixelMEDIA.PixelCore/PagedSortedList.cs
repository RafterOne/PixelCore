using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FilterSet = System.Collections.Generic.Dictionary<string, string>;
using System.ComponentModel;

namespace PixelMEDIA.PixelCore
{

    /// <summary>
    /// Describes the current sort state of a list.
    /// </summary>
	public class SortProperties
	{
        /// <summary>
        /// The name of the field being sorted on.
        /// </summary>
		public string SortField { get; set; }

        /// <summary>
        /// The sort direction.
        /// </summary>
		public ListSortDirection Direction { get; set; }
	}

    /// <summary>
    /// A single page of a larger list. Used for views.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class PagedSortedList<T> : List<T>
	{
        /// <summary>
        /// There is a page before this one.
        /// </summary>
		public bool HasPrevious { get; protected set; }

        /// <summary>
        /// There is a page after this one.
        /// </summary>
		public bool HasNext { get; protected set; }

        /// <summary>
        /// This page number.
        /// </summary>
		public int CurrentPage { get; protected set; }

        /// <summary>
        /// The total number of pages in the list.
        /// </summary>
		public int TotalPages { get; protected set; }

        /// <summary>
        /// The total number of items in the list.
        /// </summary>
		public int TotalItems { get; protected set; }


        /// <summary>
        /// The number of items per page.
        /// </summary>
		public int ItemsPerPage { get; protected set; }

        /// <summary>
        /// The current filters applied to the list.
        /// </summary>
		public FilterSet Filters { get; protected set; }

        /// <summary>
        /// The current sort state of the list.
        /// </summary>
		public SortProperties SortProperties { get; protected set; }

        /// <summary>
        /// Creates a new list page from the collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="totalItems"></param>
        /// <param name="sortProperties"></param>
        /// <param name="filters"></param>
		public PagedSortedList(IEnumerable<T> collection, int currentPage, int itemsPerPage, int totalItems, SortProperties sortProperties, FilterSet filters)
			: base(collection)
		{
			SetProperties(currentPage, itemsPerPage, totalItems, sortProperties, filters);
		}

		private void SetProperties(int currentPage, int itemsPerPage, int totalItems, SortProperties sortProperties, FilterSet filters)
		{
			this.HasPrevious = (currentPage > 1);
			this.HasNext = (totalItems > (currentPage * itemsPerPage));
			this.CurrentPage = currentPage;
			this.TotalPages = Convert.ToInt32(Math.Ceiling((double)totalItems / (double)itemsPerPage));
			this.TotalItems = totalItems;
			this.ItemsPerPage = itemsPerPage;
			this.SortProperties = sortProperties;
			this.Filters = filters;
		}


	}
}
