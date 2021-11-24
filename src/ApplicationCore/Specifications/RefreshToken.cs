using ApplicationCore.Helpers;
using ApplicationCore.Models;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationCore.Specifications
{

	public class RefreshTokenFilterSpecification : BaseSpecification<RefreshToken>
	{
		public RefreshTokenFilterSpecification(string userId) : base(x => x.UserId == userId)
		{

		}
	}
}
