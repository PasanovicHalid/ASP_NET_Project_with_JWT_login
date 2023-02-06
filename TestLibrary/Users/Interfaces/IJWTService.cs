using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary.Users.Interfaces
{
    public interface IJWTService
    {
        string GetJwtToken(List<Claim> userClaims);
    }
}
