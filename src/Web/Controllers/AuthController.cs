using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Views;
using ApplicationCore.Models;
using ApplicationCore.Auth;
using ApplicationCore.Services;
using Microsoft.Extensions.Options;
using ApplicationCore.Settings;

namespace Web.Controllers
{
	public class AuthController : BaseController
	{
		private readonly AdminSettings _adminSettings;
		private readonly IUsersService _usersService;
		private readonly IAuthService _authService;
		public AuthController(IOptions<AdminSettings> adminSettings, IUsersService usersService, IAuthService authService)
		{
			_adminSettings = adminSettings.Value;
			_usersService = usersService;
			_authService = authService;
		}

		[HttpPost("")]
		public async Task<ActionResult> Login([FromBody] OAuthLoginRequest model)
		{
			var user = _usersService.FindUserByPhone(model.Token);

			if (user == null)
			{
				ModelState.AddModelError("auth", "登入失敗.");
				return BadRequest(ModelState);
			}

			if (user.Email == _adminSettings.Email)
			{
				ModelState.AddModelError("auth", "登入失敗.");
				return BadRequest(ModelState);
			}


			var roles = await _usersService.GetRolesAsync(user);

			var responseView = await _authService.CreateTokenAsync(RemoteIpAddress, user, roles);

			return Ok(responseView);
		}

		//POST api/auth/refreshtoken
		[HttpPost("refreshtoken")]
		public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
		{
			var cp = _authService.ResolveClaimsFromToken(model.AccessToken);
			string userId = cp.GetUserId();
			OAuthProvider oauthProvider = cp.GetOAuthProvider();

			ValidateRequest(model, userId);
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var user = await _usersService.FindUserByIdAsync(userId);
			var oauth = _authService.FindOAuthByProvider(userId, oauthProvider);
			var roles = await _usersService.GetRolesAsync(user);

			var responseView = await _authService.CreateTokenAsync(RemoteIpAddress, user, oauth, roles);

			return Ok(responseView);

		}

		void ValidateRequest(RefreshTokenRequest model, string userId)
		{
			bool isValid = _authService.IsValidRefreshToken(model.RefreshToken, userId);
			if(!isValid) ModelState.AddModelError("token", "身分驗證失敗. 請重新登入");

		}



	}

}