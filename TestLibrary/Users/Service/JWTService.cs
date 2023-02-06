using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestLibrary.Settings;
using TestLibrary.Users.Interfaces;
using TestLibrary.Users.Model;

namespace TestLibrary.Users.Service
{
    public class JWTService : IJWTService
    {
        private readonly JWTSettings _JWTSettings;

        public JWTService(IOptions<JWTSettings> jWTSettings)
        {
            _JWTSettings = jWTSettings.Value;
        }

        public string GetJwtToken(List<Claim> userClaims)
        {
            SecurityTokenDescriptor tokenDescriptor = SetupTokenDescriptor(userClaims);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor SetupTokenDescriptor(List<Claim> userClaims)
        {
            byte[] secretKey = Encoding.UTF8.GetBytes(_JWTSettings.SecretKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddHours(_JWTSettings.ExpirationHours),
                Issuer = _JWTSettings.ValidIssuer,
                Audience = _JWTSettings.ValidAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha512Signature)
            };
            return tokenDescriptor;
        }
    }
}
