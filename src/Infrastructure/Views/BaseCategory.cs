using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Views
{
	public class BaseCategoryView : BaseRecordView
	{
		public string Title { get; set; }
		
		public int ParentId { get; set; }
		public string ParentTitle { get; set; }

		public bool IsRoot => ParentId == 0;
	}
}
