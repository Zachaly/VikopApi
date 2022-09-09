using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VikopApi.Api.DTO;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Application.User;
using VikopApi.Domain.Models;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthManager _authManager;

        public UserController(UserManager<ApplicationUser> userManager, IAuthManager authManager)
        {
            _userManager = userManager;
            _authManager = authManager;
        }

        /// <summary>
        /// Creates new user with given data
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                ProfilePicture = "placeholder.jpg",
                Rank = 0
            };

            await _userManager.CreateAsync(user, model.Password);

            return Ok(user.Id);
        }

        /// <summary>
        /// Returns list of all users
        /// </summary>
        [HttpGet]
        public IActionResult Users([FromServices] GetUsers getUsers)
            => Ok(getUsers.Execute());

        /// <summary>
        /// Returns user with given id
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetUser(string id, [FromServices] GetUser getUser)
            => Ok(getUser.Execute(id));

        /// <summary>
        /// User login
        /// </summary>
        /// <param name="loginModel">
        /// Consists of: 
        /// * email
        /// * password
        /// </param>
        /// <returns>JWT Token</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if(user is null)
            {
                return BadRequest("Email or password is incorrect!");
            }

            if(!await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                return BadRequest("Email or password is incorrect!");
            }

            var token = await _authManager.GetToken(user);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(tokenJson);
        }

        /// <summary>
        /// Gets id of current user
        /// </summary>
        [HttpGet]
        [Authorize]
        public IActionResult Id() => Ok(_authManager.GetCurrentUserId());
        
    }
}
