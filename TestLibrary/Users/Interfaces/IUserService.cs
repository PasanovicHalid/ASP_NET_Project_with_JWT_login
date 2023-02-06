using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLibrary.Users.Model;

namespace TestLibrary.Users.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(User user, string password);

        Task<string> LoginAsync(string username, string password);

        Task<IdentityResult> AssignRoles(User user, List<string> roles);

        void Logout();
    }
}
