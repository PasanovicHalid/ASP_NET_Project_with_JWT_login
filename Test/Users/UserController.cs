using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using TestAPI.Users.DTOs;
using TestLibrary.Users.Interfaces;
using TestLibrary.Users.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestAPI.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO loginInfo)
        {
            return Ok(await _userService.LoginAsync(loginInfo.Username, loginInfo.Password));
        }

        // GET api/<LoginController>/5
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO registrationInfo)
        {
            User user = SetupUser(registrationInfo);

            IdentityResult result =  await _userService.RegisterAsync(user, registrationInfo.Password);

            if(!result.Succeeded) 
            {
                return Conflict(new UserRegisterResultDTO
                {
                    Succeeded = result.Succeeded,
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            result = await _userService.AssignRoles(user, new List<string> { UserRoles.User });

            if (!result.Succeeded)
            {
                return Conflict(new UserRegisterResultDTO
                {
                    Succeeded = result.Succeeded,
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            return CreatedAtAction(nameof(RegisterUser), new UserRegisterResultDTO { Succeeded = true });
        }

        // POST api/<LoginController>
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return Ok("value");
        }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return Ok("value");
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            return Ok("value");
        }

        private static User SetupUser(UserRegistrationDTO registrationInfo)
        {
            User user = new User();
            user.Email = registrationInfo.Email;
            user.UserName = registrationInfo.Username;
            user.PhoneNumber = registrationInfo.PhoneNumber;
            return user;
        }
    }
}
