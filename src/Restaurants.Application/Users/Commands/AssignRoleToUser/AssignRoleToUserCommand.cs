using MediatR;

namespace Restaurants.Application.Users.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommand : IRequest
    {
        public string UserEmail { get; set; } = default!;
        public string RoleName { get; set; } = default!;

    }
}
