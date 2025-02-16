using EventsManagerModels;
using EventsManagerWebService;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventsManagerWebApp
{
    public class JWTManager(IConfiguration config)
    {
        private readonly IConfiguration config = config;
        private readonly string JwtSecret = config.GetSection("Jwt:Secret").ToString();

        public string GetToken(User user)
        {
            var issuer = config["Jwt:Issuer"] ?? "https://localhost/";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = UserClaim(user.UserId);

            //Create Security Token object by giving required parameters    
            JwtSecurityToken token = new JwtSecurityToken(issuer, //Issuer
                issuer,  //Audience
                permClaims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            string jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }

        public static async Task<Claim> GetRolesFromSource(int userId) // Pass configuration
        {
            WebClient<UserType> client = new WebClient<UserType>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Visitor",
                Method = "UserTypeByUserId"
            };
            client.AddKeyValue("UserId", userId.ToString());
            try
            {
                UserType userType = await client.GetAsync();
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

        public List<Claim> UserClaim(int userId)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", userId.ToString()),
            };
        }
    }
}
