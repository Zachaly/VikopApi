using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.User.Abstractions;
using VikopApi.Mediator.Requests;

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
        public async Task<IActionResult> Login(LoginQuery request)
        {
            var res = await _mediator.Send(request);

            if (res.Error)
                return BadRequest(res.Errors);

            return Ok(res.Token);
        }

        /// <summary>
        /// Gets id of current user
        /// </summary>
        [HttpGet]
        [Authorize]
        public IActionResult Id() => Ok(_authService.GetCurrentUserId());

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UpdateUserCommand updateUserCommand)
        {
            await _mediator.Send(updateUserCommand);
            return Ok();
        }
    }
}
