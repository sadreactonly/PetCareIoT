using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PetCareIoTMiddleware.Authentication
{
    /// <summary>
    /// Represents class for token validation.
    /// </summary>
    public class TokenValidator
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Create new TokenValidator object.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public TokenValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Validate client's token.
        /// </summary>
        /// <param name="token">Received client's token.</param>
        /// <returns>True if it's valid, false if it's invalid.</returns>
        public bool ValidateCurrentToken(string token)
        {
            var mySecret = _configuration["JWT:Secret"];
            var mySecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mySecret));

            var myIssuer = _configuration["JWT:ValidIssuer"];
            var myAudience = _configuration["JWT:ValidAudience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}