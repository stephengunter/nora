using ApplicationCore.Helpers;
using ApplicationCore.Models;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Specifications
{

	public class ArticleFilterSpecification : BaseSpecification<Article>
	{
		public ArticleFilterSpecification() : base(item => !item.Removed) 
		{
			
		}

	}
}
