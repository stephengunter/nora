using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ApplicationCore.Auth
{
    public static class ClaimsHelper
    {
        public static string GetUserId(this ClaimsPrincipal cp) => cp == null ? "" : cp.Claims.First(c => c.Type == "id").Value;

        public static OAuthProvider GetOAuthProvider(this ClaimsPrincipal cp)
        {
            if (cp == null) return OAuthProvider.Unknown;
            string providerName = cp.Claims.First(c => c.Type == "provider").Value;

            OAuthProvider provider = OAuthProvider.Unknown;
            if (Enum.TryParse(providerName, true, out provider))
            {
                if (Enum.IsDefined(typeof(OAuthProvider), provider)) return provider;
                else return OAuthProvider.Unknown;
            }
            else return OAuthProvider.Unknown;
        }
    }
}
