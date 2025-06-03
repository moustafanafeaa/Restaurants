using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Exeptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Commands.DeleteDish
{
    public class DeleteDishesForRestaurantCommandHandler(
        IRestaurantRepository restaurantRepository,
        ILogger<DeleteDishesForRestaurantCommandHandler> logger,
        IDishRepository dishRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService
        )
        : IRequestHandler<DeleteDishesForRestaurantCommand>
    {
        public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleteing all dishes for restaurant with id: {restaurantId}",
                            request.RestaurantId);

            var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (restaurant is null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());


            if (restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
                throw new ForbiddenException();


            await dishRepository.DeleteDishesForRestaurant(restaurant.Dishes);
        }
    }
}
