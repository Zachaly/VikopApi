using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Models.Auth.Commands;
using VikopApi.Application.Models.User.Commands;
using VikopApi.Application.Models.User.Requests;
using VikopApi.Application.Models.User.Validators;
using VikopApi.Application.User.Abstractions;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;

        public UserController(IUserService userService, IAuthService authService,
            IMediator mediator)
        {
            _userService = userService;
            _authService = authService;
            _mediator = mediator;
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
            [FromServices] AddUserValidator validator)
        {
            var validation = validator.Validate(request);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
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
        public async Task<IActionResult> Login(LoginCommand request)
        {
            var res = await _mediator.Send(request);

            if (res.Errors?.Any() ?? false)
                return BadRequest(res);

            return Ok(res);
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
            [FromForm] UpdateUserCommand updateUserCommand,
            [FromServices] UpdateUserValidator validator)
        {
            var validation = validator.Validate(updateUserCommand);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            return Ok(await _mediator.Send(updateUserCommand));
        }
    }
}
