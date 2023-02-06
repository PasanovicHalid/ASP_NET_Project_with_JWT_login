using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestLibrary.Users.Model;

namespace TestLibrary.Users.Interfaces
{
    public interface IClaimsService
    {
        Task<List<Claim>> GetUserClaimsAsync(User user);
    }
}
