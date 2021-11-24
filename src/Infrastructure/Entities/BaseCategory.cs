using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Entities
{
	public abstract class BaseCategory : BaseRecord
	{
		public string Title { get; set; }
		public int ParentId { get; set; }

		public bool IsRootItem => ParentId == 0;
	}
}
