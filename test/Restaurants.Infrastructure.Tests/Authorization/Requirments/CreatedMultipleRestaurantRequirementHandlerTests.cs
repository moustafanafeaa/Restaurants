using Xunit;
using Restaurants.Infrastructure.Authorization.Requirments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using FluentAssertions;

namespace Restaurants.Infrastructure.Authorization.Requirments.Tests
{
    public class CreatedMultipleRestaurantRequirementHandlerTests
    {
        [Fact()]
        public async void HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
        {
            //arrange 
            var currentUser = new CurrentUser("1", "test", "test@", null, null, []) ;
            
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = "2",
                }
            };

            var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            restaurantRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

            var requirement = new CreatedMultipleRestaurantRequirement(2);
            var handler = new CreatedMultipleRestaurantRequirementHandler(restaurantRepositoryMock.Object
                , userContextMock.Object);

            var context = new AuthorizationHandlerContext([requirement], null, null);

            //act 
            await handler.HandleAsync(context);

            //assert 
            context.HasSucceeded.Should().BeTrue();
        }
    }
}