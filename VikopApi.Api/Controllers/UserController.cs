using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VikopApi.Api.DTO;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Api.Infrastructure.FileManager;
using VikopApi.Application.Files;
using VikopApi.Application.User;
using VikopApi.Domain.Models;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthManager _authManager;
        private readonly string _placeholderImage;

        public UserController(UserManager<ApplicationUser> userManager, IAuthManager authManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _authManager = authManager;
            _placeholderImage = config["Image:Placeholder"];
        }

        /// <summary>
        /// Creates new user with given data
        /// </summary>
        /// <response code="200">
        /// Id of new user
        /// </response>
        /// <response code="200">
        /// Lists of validation errors:
        /// * Email
        /// * Password
        /// * Username
        /// </response>
        [HttpPost]
        public async Task<IActionResult> Register(
            RegisterModel model,
            [FromServices] IValidator<RegisterModel> validator)
        {
            var validation = validator.Validate(model);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.Select(error => error.ErrorMessage));
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                ProfilePicture = _placeholderImage,
                Rank = 0,
                Created = DateTime.Now
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
        public IActionResult Profile(string id, [FromServices] GetUser getUser)
            => Ok(getUser.Execute(id));

        [HttpGet("{id}")]
        public IActionResult Posts(string id, [FromServices] GetUserPosts getUserPosts)
            => Ok(getUserPosts.Execute(id));

        [HttpGet("{id}")]
        public IActionResult Findings(string id, [FromServices] GetUserFindings getUserFindings)
            => Ok(getUserFindings.Execute(id));

        /// <summary>
        /// User login
        /// </summary>
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

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(
            [FromForm] UpdateUserModel model,
            [FromServices] UpdateUser updateUser,
            [FromServices] IFileManager fileManager,
            [FromServices] GetProfilePicture getProfilePicture)
        {
            var request = new UpdateUser.Request
            {
                UserName = model.Username,
                Id = _authManager.GetCurrentUserId(),
                Picture = ""
            };

            if(model.ProfilePicture != null)
            {
                fileManager.RemoveProfilePicture(getProfilePicture.Execute(request.Id));
                request.Picture = await fileManager.SaveProfilePicture(model.ProfilePicture);
            }

            return Ok(await updateUser.Execute(request));
        }
    }
}
