using Xunit;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Restaurants.Application.Users;
using FluentAssertions;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
        {

            //arrange 
            var mapperMock = new Mock<IMapper>();
            var command = new CreateRestaurantCommand();
            var restaurant = new Restaurant();

            mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);

            var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();

            var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            restaurantRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Restaurant>()))
                .ReturnsAsync(1);

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("owner-id", "test", "test@example.com", null, null, []);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var commandHandler = new CreateRestaurantCommandHandler(mapperMock.Object,
                loggerMock.Object,
                restaurantRepositoryMock.Object,
                userContextMock.Object);

            //act
            var result = await commandHandler.Handle(command,CancellationToken.None);

            //assert 
            result.Should().Be(1);
            restaurant.OwnerId.Should().Be("owner-id");
            restaurantRepositoryMock.Verify(r => r.CreateAsync(restaurant), Times.Once);
        }
    }
}