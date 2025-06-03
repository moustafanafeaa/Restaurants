using Xunit;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Entities;
using AutoMapper;
using Restaurants.Domain.Interfaces;
using FluentAssertions;
using Restaurants.Domain.Exeptions;
using System.Security.AccessControl;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests
{
    public class UpdateRestaurantCommandHandlerTests
    {
        private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
        private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

        private readonly UpdateRestaurantCommandHandler _handler;
        public UpdateRestaurantCommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
            _mapperMock = new Mock<IMapper>();
            _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

            _handler = new UpdateRestaurantCommandHandler(
                _loggerMock.Object,
                _restaurantRepositoryMock.Object,
                _mapperMock.Object,
                _restaurantAuthorizationServiceMock.Object);
        }
        [Fact()]
        public async Task Handle_ForValidRequest_ShouldUpdateRestaurants()
        {
            //arrange
            var restautantId = 1;
            var command = new UpdateRestaurantCommand()
            {
                Id = restautantId,
                Name = "New test",
                Description = "New Desc",
                HasDelivery = true
            };

            var restaurant = new Restaurant()
            {
                Id = restautantId,
                Name = "test",
                Description = "desc"
            };


            _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restautantId))
                .ReturnsAsync(restaurant);

            _restaurantAuthorizationServiceMock.Setup(m => m.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
                .Returns(true);

            //act
            await _handler.Handle(command, CancellationToken.None);

            //assert
            _mapperMock.Verify(r => r.Map(command,restaurant), Times.Once);
            _restaurantRepositoryMock.Verify(r => r.UpdateAsync(restaurant), Times.Once);

        }
        [Fact]
        public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
        {

            //arrange
            var restautantId = 1;
            var command = new UpdateRestaurantCommand
            {
                Id = restautantId
            };
             
            _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restautantId))
               .ReturnsAsync((Restaurant?)null);

            //act 

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            //assert

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Restaurant with id: {restautantId} dosen't exist");
        }

        [Fact]
        public async Task Handle_WithUnauthorizedUser_ShouldThrowForbidException()
        {

            //arrange
            var restautantId = 1;
            var command = new UpdateRestaurantCommand
            {
                Id = restautantId
            };

            var existingRestaurant = new Restaurant
            {
                Id = restautantId
            };

            _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restautantId))
               .ReturnsAsync(existingRestaurant);

            _restaurantAuthorizationServiceMock.Setup(a => a.Authorize(existingRestaurant, Domain.Constants.ResourceOperation.Update))
                .Returns(false);
            //act 

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            //assert

            await act.Should().ThrowAsync<ForbiddenException>();
        }
    }
}