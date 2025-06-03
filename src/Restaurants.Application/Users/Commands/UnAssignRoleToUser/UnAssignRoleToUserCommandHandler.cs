using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users.Commands.AssignRoleToUser;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Users.Commands.UnAssignRoleToUser
{
    public class UnAssignRoleToUserCommandHandler(ILogger<AssignRoleToUserCommandHandler> logger,
        RoleManager<IdentityRole> roleManager,
        UserManager<User> userManager) : IRequestHandler<UnAssignRoleToUserCommand>
    {
        public async Task Handle(UnAssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Unassign user role: {@request}", request);

            var user = await userManager.FindByEmailAsync(request.UserEmail)
                 ?? throw new NotFoundException(nameof(User), request.UserEmail);

            var role = await roleManager.FindByNameAsync(request.RoleName)
                ?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);

            await userManager.RemoveFromRoleAsync(user, request.RoleName);
        }
    }
}
