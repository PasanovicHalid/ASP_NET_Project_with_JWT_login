using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestLibrary.Users.Interfaces;
using TestLibrary.Users.Model;

namespace TestLibrary.Users.Service
{
    public class ClaimsService : IClaimsService
    {
        private readonly UserManager<User> _userManager;

        public ClaimsService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<Claim>> GetUserClaimsAsync(User user)
        {
            List<Claim> userClaims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            foreach(string role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            return userClaims;
        }
    }
}
