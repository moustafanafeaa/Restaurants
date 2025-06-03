using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands;
using Restaurants.Application.Users.Commands.AssignRoleToUser;
using Restaurants.Application.Users.Commands.UnAssignRoleToUser;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        [HttpPatch("user")]
        [Authorize]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserCommand command) 
        {
            await mediator.Send(command);
            return NoContent();
        }
        [HttpPost("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UnAssignRoleToUser([FromBody] UnAssignRoleToUserCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }
    }
}
