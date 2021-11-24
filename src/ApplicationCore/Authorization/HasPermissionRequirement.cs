using Microsoft.AspNetCore.Authorization;

namespace ApplicationCore.Authorization
{
	public class HasPermissionRequirement : IAuthorizationRequirement
	{
		private readonly Permissions _permission;

		public HasPermissionRequirement(Permissions permission)
		{
			this._permission = permission;
		}

		public Permissions Permission => _permission;
	}

}
