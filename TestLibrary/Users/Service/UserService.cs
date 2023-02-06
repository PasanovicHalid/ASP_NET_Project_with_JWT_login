using Microsoft.AspNetCore.Identity;
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
using TestLibrary.Users.Exceptions;
using TestLibrary.Users.Interfaces;
using TestLibrary.Users.Model;

namespace TestLibrary.Users.Service
{
    public class UserService : IUserService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IClaimsService _claimsService;
        private readonly IJWTService _jwtService;
        public UserService(UserManager<User> userManager, RoleManager<UserRole> roleManager, SignInManager<User> signInManager, IClaimsService claimsService, IJWTService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _claimsService = claimsService;
            _jwtService = jwtService;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            User user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new UserDoesntExistException("User with username: " + username + " doesn't exist");
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
            {
                throw new FailedLoginException("Failed Login! Invalid Username or Password");
            }

            return _jwtService.GetJwtToken(await _claimsService.GetUserClaimsAsync(user));
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> RegisterAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AssignRoles(User user, List<string> roles)
        {
            await SeedRoles();
            return await _userManager.AddToRolesAsync(user, roles);
        }

        private async Task SeedRoles()
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new UserRole(UserRoles.Admin));

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new UserRole(UserRoles.User));
        }

    }
}
