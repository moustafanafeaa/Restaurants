using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Users.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandHandler(ILogger<AssignRoleToUserCommandHandler> logger,
        RoleManager<IdentityRole> roleManager,
        UserManager<User> userManager) : IRequestHandler<AssignRoleToUserCommand>
    {
        public async Task Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Assign user role: {@request}", request);

            var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

            var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);

            await userManager.AddToRoleAsync(user, role.Name!);
        }

    }
}
