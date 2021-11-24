using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Views
{
	public class AccessTokenResponse
	{
		public string Token { get; }
		public int ExpiresIn { get; }

		public AccessTokenResponse(string token, int expiresIn)
		{
			this.Token = token;
			this.ExpiresIn = expiresIn;
		}
	}

	public class AuthResponse
	{
		public AccessTokenResponse AccessToken { get; set; }
		public string RefreshToken { get; set; }

	}

}
