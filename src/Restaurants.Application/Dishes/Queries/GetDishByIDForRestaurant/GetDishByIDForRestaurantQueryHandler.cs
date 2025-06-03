using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exeptions;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Queries.GetDishByIDForRestaurant
{
    public class GetDishByIDForRestaurantQueryHandler(
        ILogger<GetDishByIDForRestaurantQueryHandler> logger,
        IRestaurantRepository restaurantRepository,
        IMapper mapper
        ) : IRequestHandler<GetDishByIDForRestaurantQuery, DishDto>
    {
        public async Task<DishDto> Handle(GetDishByIDForRestaurantQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving dish: {DishId} for restaurant with id: {restaurantId}",
                request.DishId,
                request.RestaurantId);

            var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (restaurant is null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId);
            if(dish is null) throw new NotFoundException(nameof(Dish), request.DishId.ToString());

            return mapper.Map<DishDto>(dish);
        }
    }
}
