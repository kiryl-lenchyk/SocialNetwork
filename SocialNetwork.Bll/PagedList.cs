using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using SocialNetwork.Bll.Interface;

namespace SocialNetwork.Bll
{
    public class PagedList<T> : IMappedPagedList<T>
    {
        #region Fields

        private IList<T> pageItems;

        #endregion

        #region Constractors

        /// <summary>
        /// Create new paged list from full list
        /// </summary>
        /// <param name="allElements">all elements</param>
        /// <param name="pageSize">page size</param>
        /// <param name="pageNumber">current page nu,ber</param>
        public PagedList(IOrderedQueryable<T> allElements, int pageSize, int pageNumber)
        {
            if (pageItems == null) throw new ArgumentNullException("allElements");

            TotalItemCount = allElements.Count();
            PageSize = pageSize;
            PageNumber = pageNumber;
            PageCount = TotalItemCount / pageSize + (TotalItemCount % pageSize == 0 ? 0 : 1);

            pageItems = allElements.Skip(pageSize*(pageNumber-1)).Take(pageSize).ToList();
        }

        /// <summary>
        /// Create new PagedList from elements for one page.
        /// </summary>
        /// <param name="pageElements">elements for current page</param>
        /// <param name="pageSize">page size</param>
        /// <param name="pageNumber">current page number</param>
        /// <param name="totalCount">total elements count</param>
        /// <param name="pagesCount">count of pages</param>
        public PagedList(IList<T> pageElements, int pageSize, int pageNumber, int totalCount, int pagesCount)
        {
            if (pageItems == null) throw new ArgumentNullException("pageElements");

            TotalItemCount = totalCount;
            PageSize = pageSize;
            PageNumber = pageNumber;
            PageCount = pagesCount;

            pageItems = pageElements;
        }

        private PagedList()
        {
        }

        /// <summary>
        /// Create new paged list from full list with result elements convertion
        /// </summary>
        /// <param name="allElements">all elements</param>
        /// <param name="pageSize">page size</param>
        /// <param name="pageNumber">current page number</param>
        /// <param name="convert">lambda to convert</param>
        public static PagedList<T> GetPagedListWithConvert<TSours>(IOrderedQueryable<TSours> allElements, int pageSize, int pageNumber, Func<TSours, T> convert)
        {
            if (allElements == null) throw new ArgumentNullException("allElements");

            int totalItemCount = allElements.Count();
            PagedList<T> pagedList = new PagedList<T>();
            pagedList.TotalItemCount = totalItemCount;
            pagedList.PageSize = pageSize;
            pagedList.PageNumber = pageNumber;
            pagedList.PageCount = totalItemCount/pageSize + (totalItemCount%pageSize == 0 ? 0 : 1);
            pagedList.pageItems = allElements.Skip(pageSize*(pageNumber - 1))
                .Take(pageSize)
                .ToList()
                .Select(convert)
                .ToList();


            return pagedList;
        }

        #endregion

        #region Properties and Indexer
        /// <summary>
        /// Total number of subsets within the superset.
        /// </summary>
        /// <value>
        /// Total number of subsets within the superset.
        /// </value>
        public int PageCount { get; private set; }

        /// <summary>
        /// Total number of objects contained within the superset.
        /// </summary>
        /// <value>
        /// Total number of objects contained within the superset.
        /// </value>
        public int TotalItemCount { get; private set; }

        /// <summary>
        /// One-based index of this subset within the superset.
        /// </summary>
        /// <value>
        /// One-based index of this subset within the superset.
        /// </value>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Maximum size any individual subset.
        /// </summary>
        /// <value>
        /// Maximum size any individual subset.
        /// </value>
        public int PageSize { get; private set; }

        /// <summary>
        /// Returns true if this is NOT the first subset within the superset.
        /// </summary>
        /// <value>
        /// Returns true if this is NOT the first subset within the superset.
        /// </value>
        public bool HasPreviousPage
        {
            get { return PageNumber > 1; }
        }

        /// <summary>
        /// Returns true if this is NOT the last subset within the superset.
        /// </summary>
        /// <value>
        /// Returns true if this is NOT the last subset within the superset.
        /// </value>
        public bool HasNextPage
        {
            get { return PageNumber != PageCount; }
        }

        /// <summary>
        /// Returns true if this is the first subset within the superset.
        /// </summary>
        /// <value>
        /// Returns true if this is the first subset within the superset.
        /// </value>
        public bool IsFirstPage
        {
            get { return PageNumber == 1; }
        }

        /// <summary>
        /// Returns true if this is the last subset within the superset.
        /// </summary>
        /// <value>
        /// Returns true if this is the last subset within the superset.
        /// </value>
        public bool IsLastPage
        {
            get { return PageNumber == PageCount; }
        }

        /// <summary>
        /// One-based index of the first item in the paged subset.
        /// </summary>
        /// <value>
        /// One-based index of the first item in the paged subset.
        /// </value>
        public int FirstItemOnPage
        {
            get { return 0; }
        }

        /// <summary>
        /// One-based index of the last item in the paged subset.
        /// </summary>
        /// <value>
        /// One-based index of the last item in the paged subset.
        /// </value>
        public int LastItemOnPage
        {
            get { return pageItems.Count - 1; }
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        public T this[int index]
        {
            get { return pageItems[index]; }
        }

        /// <summary>
        /// Gets the number of elements contained on this page.
        /// </summary>
        public int Count { get { return pageItems.Count; } }

        #endregion

        #region Public Methods
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            return pageItems.GetEnumerator();
        }

        /// <summary>
        /// Project an element on page into new form.
        /// </summary>
        /// <typeparam name="TRes">type resulted PageList items</typeparam>
        /// <param name="convert">function to convert</param>
        /// <returns></returns>
        public IMappedPagedList<TRes> Map<TRes>(Func<T, TRes> convert)
        {
            return new PagedList<TRes>()
            {
                PageCount = PageCount,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalItemCount = TotalItemCount,
                pageItems = pageItems.Select(convert).ToList()
            };
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets a non-enumerable copy of this paged list.
        /// </summary>
        /// <returns>
        /// A non-enumerable copy of this paged list.
        /// </returns>
        public IPagedList GetMetaData()
        {
            return new PagedListMetaData(this);
        }

        #endregion
    }
}
