using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Authorization.Requirments
{
    public class MinimumAgeRequirmentHandler(ILogger<MinimumAgeRequirmentHandler> logger,
        IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MinimumAgeRequirment requirement)
        {
            var user = userContext.GetCurrentUser();

            var dob = context.User.Claims.FirstOrDefault(x => x.Type == "");
            logger.LogInformation("User: {email}, date of birth {DoB} - handling mimimum age requirment"
                , user.Email, user.DateOfBirth);


            if(user.DateOfBirth == null)
            {
                logger.LogInformation("user date of birth is null");
                context.Fail();
                return Task.CompletedTask;
            }
            if (user.DateOfBirth.Value.AddYears(requirement.MinAge) <= DateOnly.FromDateTime(DateTime.Today))
            {
                logger.LogInformation("Authorization succeeded");
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
