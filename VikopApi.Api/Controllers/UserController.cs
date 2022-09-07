using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.DTO;
using VikopApi.Application.User;
using VikopApi.Domain.Models;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
                ProfilePicture = "placeholder.jpg"
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
    }
}
