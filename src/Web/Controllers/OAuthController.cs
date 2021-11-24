using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Views;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Google.Apis.Auth;

namespace Web.Controllers
{
	public class OAuthController : BaseController
	{
		private readonly IUsersService _usersService;
		private readonly IAuthService _authService;

		public OAuthController(IUsersService usersService, IAuthService authService)
		{
			_usersService = usersService;
			_authService = authService;
		}


		[HttpPost("google")]
		public async Task<ActionResult> Google([FromBody] OAuthLoginRequest model)
		{
			var payload = await GoogleJsonWebSignature.ValidateAsync(model.Token, new GoogleJsonWebSignature.ValidationSettings());

			var user = await _usersService.FindUserByEmailAsync(payload.Email);

			if (user == null)
			{
				bool emailConfirmed = true;
				user = await _usersService.CreateUserAsync(payload.Email, emailConfirmed);
			}

			var oAuth = new OAuth
			{
				OAuthId = payload.Subject,
				Provider = OAuthProvider.Google,
				GivenName = payload.GivenName,
				FamilyName = payload.FamilyName,
				Name = payload.Name,
				UserId = user.Id,
				PictureUrl = payload.Picture
			};

			await _authService.CreateUpdateUserOAuthAsync(user.Id, oAuth);

			var roles = await _usersService.GetRolesAsync(user);

			var responseView = await _authService.CreateTokenAsync(RemoteIpAddress, user, oAuth, roles);

			return Ok(responseView);
		}


	}

}