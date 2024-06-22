using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thinktecture.IdentityModel.Http.Hawk.Core;
using EventsManagerModels;
using EventsManagerWebService;
using System.Web.Helpers;
using System.Reflection;
using System.Web.Mvc;
using System.Runtime.Caching;

namespace EventsManagerWeb
{
    public class JwtCookieAuthenticationMiddleware : AuthenticationMiddleware<JwtCookieAuthenticationOptions>
    {
        public JwtCookieAuthenticationMiddleware(OwinMiddleware next, JwtCookieAuthenticationOptions options)
            : base(next, options)
        {
        }

        protected override AuthenticationHandler<JwtCookieAuthenticationOptions> CreateHandler()
        {
            return new JwtCookieAuthenticationHandler();
        }
    }

    public class JwtCookieAuthenticationHandler : AuthenticationHandler<JwtCookieAuthenticationOptions>
    {
        const string CacheKeyBase = "user_roles_";

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            string token = Request.Cookies["Token"]; // Get the token from the cookie

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var validationParameters = Options.TokenValidationParameters;

            try
            {
                ClaimsPrincipal claimsPrincipal = handler.ValidateToken(token, validationParameters, out _);
                string userId = claimsPrincipal.FindFirst("UserId")?.Value;

                if (userId == null)
                {
                    return null;
                }

                // Assign roles based on userId
                Claim roles = await GetRoleForUserId(userId);

                ClaimsIdentity identity = new ClaimsIdentity(claimsPrincipal.Identity.AuthenticationType);
                identity.AddClaims(claimsPrincipal.Claims);
                identity.AddClaim(new Claim(ClaimTypes.Name, userId??"None"));
                identity.AddClaim(roles);
                AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
                return ticket;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Claim> GetRoleForUserId(string userId)
        {
            var cacheKey = CacheKeyBase + userId; // Combine base key and user ID
            var cache = MemoryCache.Default;

            var roles = cache.Get(cacheKey); // Try to get from cache
            if (roles == null)
            {
                // Fetch roles from database or COM component
                roles = await GetRolesFromSource(userId);

                // Add to cache with expiration
                var cacheItemPolicy = new CacheItemPolicy(); // Set expiration time
                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
                cache.Set(cacheKey, roles, cacheItemPolicy);
            }

            return (Claim) roles;
        }

        private async static Task<Claim> GetRolesFromSource(string userId)
        {
            WebClient<UserType> client = new WebClient<UserType>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "UserTypeByUserId"
            };
            client.AddKeyValue("UserId", userId);
            try
            {
                UserType userType = await client.GetAsync() ?? null;
                if (userType != null && userType.UserTypeId != 0)
                {
                    return new Claim(ClaimTypes.Role, userType.UserTypeName);
                }
            }
            catch (Exception)
            {
                return new Claim(ClaimTypes.Role, "Anonymous");
            }
            return new Claim(ClaimTypes.Role, "Anonymous");
        }
    }

    public class JwtCookieAuthenticationOptions : AuthenticationOptions
    {
        public TokenValidationParameters TokenValidationParameters { get; set; }

        public JwtCookieAuthenticationOptions() : base("JwtCookie")
        {
            AuthenticationMode = AuthenticationMode.Active;
        }
    }
}