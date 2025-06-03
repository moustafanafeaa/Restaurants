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

namespace Restaurants.Application.Dishes.Queries.GetAllDishes
{
    public class GetDishesForRestaurantQueryHandler(
        ILogger<GetDishesForRestaurantQueryHandler> logger
        ,IRestaurantRepository restaurantRepository,
        IMapper mapper) 
        : IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDto>>
    {
        async Task<IEnumerable<DishDto>> IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDto>>.Handle(GetDishesForRestaurantQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving dishes for restaurant with id: {restaurantId}", request.RestaurantId);
            var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (restaurant is null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());


            var results = mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);

            return results;
        }
    }
}
