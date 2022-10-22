using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VikopApi.Api.DTO;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Api.Infrastructure.FileManager;
using VikopApi.Application.Files;
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
        private readonly IAuthManager _authManager;
        private readonly string _placeholderImage;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        public UserController(UserManager<ApplicationUser> userManager, IAuthManager authManager,
            IConfiguration config, IUserService userService, IFileService fileService)
        {
            _userManager = userManager;
            _authManager = authManager;
            _placeholderImage = config["Image:Placeholder"];
            _userService = userService;
            _fileService = fileService;
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
            [FromServices] IFileManager fileManager)
        {
            var request = new UpdateUserRequest
            {
                UserName = model.Username,
                Id = _authManager.GetCurrentUserId(),
                Picture = ""
            };

            if(model.ProfilePicture != null)
            {
                fileManager.RemoveProfilePicture(_fileService.GetProfilePicture(request.Id));
                request.Picture = await fileManager.SaveProfilePicture(model.ProfilePicture);
            }
            await _userService.UpdateUser(request);
            return Ok();
        }
    }
}
