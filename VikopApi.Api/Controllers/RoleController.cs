using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VikopApi.Application.Models.Role.Commands;
using VikopApi.Application.Models.Role.Validators;
using VikopApi.Application.Role.Abstractions;
using VikopApi.Application.Role.Commands;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Policy = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMediator _mediator;

        public RoleController(IRoleService roleService, IMediator mediator)
        {
            _roleService = roleService;
            _mediator = mediator;
        }

        [HttpGet("{roleName}")]
        public async Task<IActionResult> Users(string roleName)
            => Ok(await _roleService.GetUsersWithRole(roleName));

        [HttpPut]
        public async Task<IActionResult> Add(
            AddRoleCommand command,
            [FromServices] AddRoleValidator validator)
        {
            var validation = validator.Validate(command);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> Remove(RemoveRoleCommand command)
            => Ok(await _mediator.Send(command));
    }
}
