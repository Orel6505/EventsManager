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
                // Assign roles based on userId (customize this logic as needed)
                Claim roles = GetRoleForUserId(userId);

                ClaimsIdentity identity = new ClaimsIdentity(claimsPrincipal.Identity.AuthenticationType);
                identity.AddClaims(claimsPrincipal.Claims);
                identity.AddClaim(new Claim(ClaimTypes.Name, userId));
                identity.AddClaim(roles);
                AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
                return ticket;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Custom logic to get roles based on userId (replace with your actual logic)
        private static Claim GetRoleForUserId(string userId)
        {
            // Example: Query database or use any other logic to get roles for the userId
            WebClient<UserType> client = new WebClient<UserType>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "UserTypeByUserId"
            };
            client.AddKeyValue("UserId", userId);
            UserType userType = client.Get();
            if (userType.UserTypeId != 0)
            {
                return new Claim(ClaimTypes.Role, userType.UserTypeName);
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