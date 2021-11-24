using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.Views
{
	public class RefreshTokenRequest
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }

	}

	public class LoginRequest
	{
		[Required(ErrorMessage = "必須填寫使用者名稱")]
		public string Username { get; set; }

		[Required(ErrorMessage = "必須填寫密碼")]
		public string Password { get; set; }
	}

	public class OAuthLoginRequest
	{
		public string Token { get; set; }

	}
}
