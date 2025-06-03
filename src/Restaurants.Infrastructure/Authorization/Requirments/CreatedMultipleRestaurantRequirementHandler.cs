using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Authorization.Requirments
{
    internal class CreatedMultipleRestaurantRequirementHandler(IRestaurantRepository restaurantRepository,
        IUserContext userContext) : AuthorizationHandler<CreatedMultipleRestaurantRequirement>
    {
        protected override async Task HandleRequirementAsync
            (AuthorizationHandlerContext context, CreatedMultipleRestaurantRequirement requirement)
        {
            var user = userContext.GetCurrentUser();
            var restaurants =await restaurantRepository.GetAllAsync();
            var userRestaurantsCreated = restaurants.Count(r => r.OwnerId == user.Id);
            if(userRestaurantsCreated >= requirement.MinimumRestaurantsCreated)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
