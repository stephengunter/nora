using ApplicationCore.Helpers;
using ApplicationCore.Models;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationCore.Specifications
{

	public class OAuthFilterSpecification : BaseSpecification<OAuth>
	{
		public OAuthFilterSpecification(string userId, OAuthProvider provider) : base(x => x.UserId == userId && x.Provider == provider)
		{

		}
	}
}
