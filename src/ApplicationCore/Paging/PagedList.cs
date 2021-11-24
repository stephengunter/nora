using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationCore.Helpers;

namespace ApplicationCore.Paging
{
	public interface IPagedList<T,V>
	{
		List<T> List { get; set; }
		List<V> ViewList { get; set; }

		int TotalItems { get; set; }
		int PageNumber { get; }
		int PageSize { get; }
		


		int TotalPages { get; }
		
		bool HasPreviousPage { get; }
		bool HasNextPage { get; }
		int NextPageNumber { get; }
		int PreviousPageNumber { get; }

		string SortBy { get; }
		bool Desc { get; }

		IPagingHeader GetHeader();

	}
	public class PagedList<T,V> : IPagedList<T,V>
	{
		
		public PagedList(IEnumerable<T> list, int pageNumber = 1, int pageSize = -1, string sortBy = "", bool desc = true)
		{
			TotalItems = list.Count();
			PageNumber = pageNumber < 1 ? 1 : pageNumber;
			PageSize = pageSize == 0 ? -1 : pageSize;

			List = list.GetPaged(PageNumber, PageSize).ToList();
			ViewList = new List<V>();

			TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);

			HasPreviousPage= PageNumber > 1;
			HasNextPage = PageNumber < TotalPages;
			NextPageNumber= HasNextPage ? PageNumber + 1 : TotalPages;
			PreviousPageNumber = HasPreviousPage ? PageNumber - 1 : 1;

			SortBy = sortBy;
			Desc = desc;

		}

		

		public List<T> List { get; set; }
		public List<V> ViewList { get; set; }

		public int TotalItems { get; set; }
		public int PageNumber { get; }
		public int PageSize { get; }
		public int TotalPages { get; }
		public bool HasPreviousPage { get; }
		public bool HasNextPage { get; }

		public int NextPageNumber { get; }
		public int PreviousPageNumber { get; }

		public string SortBy { get; }
		public bool Desc { get; }

		public IPagingHeader GetHeader()
		{
			return new PagingHeader(TotalItems, PageNumber, PageSize, TotalPages);
		}
	}


	public class PagedList<T>
	{
		public PagedList(IEnumerable<T> list, int pageNumber = 1, int pageSize = -1, string sortBy = "", bool desc = true)
		{
			TotalItems = list.Count();
			PageNumber = pageNumber < 1 ? 1 : pageNumber;
			PageSize = pageSize == 0 ? -1 : pageSize;

			List = list.GetPaged(PageNumber, PageSize).ToList();

			TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);

			HasPreviousPage = PageNumber > 1;
			HasNextPage = PageNumber < TotalPages;
			NextPageNumber = HasNextPage ? PageNumber + 1 : TotalPages;
			PreviousPageNumber = HasPreviousPage ? PageNumber - 1 : 1;

			SortBy = sortBy;
			Desc = desc;

		}



		public List<T> List { get; set; }

		public int TotalItems { get; set; }
		public int PageNumber { get; }
		public int PageSize { get; }
		public int TotalPages { get; }
		public bool HasPreviousPage { get; }
		public bool HasNextPage { get; }

		public int NextPageNumber { get; }
		public int PreviousPageNumber { get; }

		public string SortBy { get; }
		public bool Desc { get; }

		public IPagingHeader GetHeader()
		{
			return new PagingHeader(TotalItems, PageNumber, PageSize, TotalPages);
		}
	}
}
