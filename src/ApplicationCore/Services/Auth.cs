using ApplicationCore.Auth;
using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using ApplicationCore.Settings;
using ApplicationCore.Specifications;
using ApplicationCore.Views;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
	public interface IAuthService
	{
		Task<AuthResponse> CreateTokenAsync(string ipAddress, User user, IList<string> roles = null);

		Task<AuthResponse> CreateTokenAsync(string ipAddress, User user, OAuth oAuth, IList<string> roles = null);

		Task CreateUpdateUserOAuthAsync(string userId, OAuth oAuth);

		ClaimsPrincipal ResolveClaimsFromToken(string accessToken);

		bool IsValidRefreshToken(string token, string userId);

		OAuth FindOAuthByProvider(string userId, OAuthProvider provider);
	}

	public class AuthService : IAuthService
	{
		private readonly AuthSettings _authSettings;

		private readonly IJwtFactory _jwtFactory;
		private readonly ITokenFactory _tokenFactory;
		private readonly IJwtTokenValidator _jwtTokenValidator;

		private readonly IDefaultRepository<RefreshToken> _refreshTokenRepository;
		private readonly IDefaultRepository<OAuth> _oAuthRepository;

		public AuthService(IOptions<AuthSettings> authSettings, IJwtFactory jwtFactory, ITokenFactory tokenFactory, IJwtTokenValidator jwtTokenValidator,
			IDefaultRepository<RefreshToken> refreshTokenRepository, IDefaultRepository<OAuth> oAuthRepository)
		{
			_authSettings = authSettings.Value;

			_jwtFactory = jwtFactory;
			_tokenFactory = tokenFactory;
			_jwtTokenValidator = jwtTokenValidator;

			_refreshTokenRepository = refreshTokenRepository;
			_oAuthRepository = oAuthRepository;
		}

		int RefreshTokenDaysToExpire => _authSettings.RefreshTokenDaysToExpire < 1 ? 5 : _authSettings.RefreshTokenDaysToExpire;

		string SecretKey => _authSettings.SecurityKey;

		public async Task<AuthResponse> CreateTokenAsync(string ipAddress, User user, IList<string> roles = null)
		{
			var accessToken = await _jwtFactory.GenerateEncodedToken(user, roles);
			var refreshToken = _tokenFactory.GenerateToken();

			await SetRefreshTokenAsync(ipAddress, user, refreshToken);

			return new AuthResponse
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken
			};
		}

		public async Task<AuthResponse> CreateTokenAsync(string ipAddress, User user, OAuth oAuth, IList<string> roles = null)
		{
			var accessToken = await _jwtFactory.GenerateEncodedToken(user, oAuth, roles);
			var refreshToken = _tokenFactory.GenerateToken();

			await SetRefreshTokenAsync(ipAddress, user, refreshToken);

			return new AuthResponse
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken
			};
		}

		public OAuth FindOAuthByProvider(string userId, OAuthProvider provider)
			=> _oAuthRepository.GetSingleBySpec(new OAuthFilterSpecification(userId, provider));
		

		public async Task CreateUpdateUserOAuthAsync(string userId, OAuth oAuth)
		{
			var exist = FindOAuthByProvider(userId, oAuth.Provider);

			if (exist != null)
			{
				oAuth.Id = exist.Id;
				await _oAuthRepository.UpdateAsync(exist, oAuth);
			}
			else
			{
				await _oAuthRepository.AddAsync(oAuth);
			}

		}

		public ClaimsPrincipal ResolveClaimsFromToken(string accessToken)
			=> _jwtTokenValidator.GetPrincipalFromToken(accessToken, SecretKey);


		public string GetUserIdFromToken(string accessToken)
		{
			var cp = _jwtTokenValidator.GetPrincipalFromToken(accessToken, SecretKey);
			if (cp == null) return "";

			return cp.Claims.First(c => c.Type == "id").Value;
		}

		public string GetOAuthProviderFromToken(string accessToken)
		{
			var cp = _jwtTokenValidator.GetPrincipalFromToken(accessToken, SecretKey);
			if (cp == null) return "";

			return cp.Claims.First(c => c.Type == "provider").Value;
		}

		public bool IsValidRefreshToken(string token, string userId)
		{
			var entity = GetRefreshToken(userId);
			if (entity == null) return false;

			return entity.Token == token && entity.Active;

		}

		async Task SetRefreshTokenAsync(string ipAddress, User user, string token)
		{
			var expires = DateTime.UtcNow.AddDays(RefreshTokenDaysToExpire);

			var exist = GetRefreshToken(user.Id);
			if (exist != null)
			{
				exist.Token = token;
				exist.Expires = expires;
				exist.RemoteIpAddress = ipAddress;

				await _refreshTokenRepository.UpdateAsync(exist);
			}
			else
			{
				var refreshToken = new RefreshToken
				{
					Token = token,
					Expires = expires,
					UserId = user.Id,
					RemoteIpAddress = ipAddress
				};

				await _refreshTokenRepository.AddAsync(refreshToken);

			}

		}

		RefreshToken GetRefreshToken(string userId)
		{
			var spec = new RefreshTokenFilterSpecification(userId);
			return _refreshTokenRepository.GetSingleBySpec(spec);
		}

	}
}
