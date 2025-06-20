﻿using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Authorization.Services
{
    public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
        IUserContext userContext) : IRestaurantAuthorizationService
    {
        public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
        {
            var user = userContext.GetCurrentUser();

            logger.LogInformation("Authorizing user {email}, to {operation} for restaurant {restaurantName}",
                user.Email, resourceOperation.ToString(), restaurant.Name);

            if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
            {
                logger.LogInformation("Create/Read operation - successful authorization");
                return true;
            }
            if (resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
            {
                logger.LogInformation("Admin user, delete operation - successful authorization");
                return true;
            }
            if (resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update
                && user.Id == restaurant.OwnerId)
            {
                logger.LogInformation("Restaurant owner, Update/Delete operation - successful authorization");
                return true;
            }
            return false;
        }
    }
}
