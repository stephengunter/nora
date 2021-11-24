using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Paging
{
	public class PagingParams
	{
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
	}
}
