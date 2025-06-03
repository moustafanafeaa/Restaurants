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

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,
        IRestaurantRepository restaurantRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteRestaurantCommand>
    {
        public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting restaurant with id : {RestaurantId}", request.Id);
            var restaurant = await restaurantRepository.GetByIdAsync(request.Id);

            if(restaurant is null)
               throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
            
            if(!restaurantAuthorizationService.Authorize(restaurant,ResourceOperation.Delete))
            {
                throw new ForbiddenException();
            }

            await restaurantRepository.DeleteAsync(restaurant);
          
        }

        
    }
}
