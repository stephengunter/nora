using ApplicationCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.Helpers
{
	
	public static class PagingHelper
    {
		public static IList<T> GetPaged<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
		{
			if (pageSize <= 0) pageSize = 999;
			return source.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();

		}
		

	}
}
