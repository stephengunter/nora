using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using ApplicationCore.Services;

namespace ApplicationCore.Authorization
{
	public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
	{

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
		{
			Permissions permissione = requirement.Permission;

			if(permissione == Permissions.Admin)
			{
				if(context.User.Claims.IsBoss() || context.User.Claims.IsDev())
				{
					context.Succeed(requirement);
					return Task.CompletedTask;
				}
				
			}

			context.Fail();
			return Task.CompletedTask;
		}
	}
}
