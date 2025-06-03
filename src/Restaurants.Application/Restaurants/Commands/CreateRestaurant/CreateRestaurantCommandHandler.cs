using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler(IMapper mapper,
        ILogger<CreateRestaurantCommandHandler> logger,
        IRestaurantRepository restaurantRepository,
        IUserContext userContext) : IRequestHandler<CreateRestaurantCommand, int>
    {
        public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            logger.LogInformation("{username} [{userId}] Creating a new restaurant {@Restaurant}"
                ,user.Email,user.Id, request);
            
            var restaurant = mapper.Map<Restaurant>(request);
            restaurant.OwnerId = user.Id;   
            var RestId = await restaurantRepository.CreateAsync(restaurant);

            return RestId;
        }
    }
}
