using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VikopApi.Api.DTO;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.User.Abstractions;
using VikopApi.Domain.Models;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(UserManager<ApplicationUser> userManager, IUserService userService, IAuthService authService)
        {
            _userManager = userManager;
            _userService = userService;
            _authService = authService;
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
            AddUserRequest request,
            [FromServices] IValidator<AddUserRequest> validator)
        {
            var validation = validator.Validate(request);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.Select(error => error.ErrorMessage));
            }

            return Ok(await _authService.AddUser(request));
        }

        /// <summary>
        /// Returns list of all users
        /// </summary>
        [HttpGet]
        public IActionResult Users()
            => Ok(_userService.GetUsers());

        /// <summary>
        /// Returns user with given id
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult Profile(string id)
            => Ok(_userService.GetUserById(id));

        [HttpGet("{id}")]
        public IActionResult Posts(string id)
            => Ok(_userService.GetUserPosts(id));

        [HttpGet("{id}")]
        public IActionResult Findings(string id)
            => Ok(_userService.GetUserFindings(id));

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

            var token = await _authService.GetToken(user);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(tokenJson);
        }

        /// <summary>
        /// Gets id of current user
        /// </summary>
        [HttpGet]
        [Authorize]
        public IActionResult Id() => Ok(_authService.GetCurrentUserId());

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(
            [FromForm] UpdateUserModel model,
            [FromServices] IFileService fileManager)
        {
            var request = new UpdateUserRequest
            {
                UserName = model.Username,
                Id = _authService.GetCurrentUserId(),
                Picture = ""
            };

            if(model.ProfilePicture != null)
            {
                fileManager.RemoveProfilePicture(request.Id);
                request.Picture = await fileManager.SaveProfilePicture(model.ProfilePicture);
            }

            await _userService.UpdateUser(request);
            return Ok();
        }
    }
}
