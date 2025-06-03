using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteDish;
using Restaurants.Application.Dishes.Queries.GetAllDishes;
using Restaurants.Application.Dishes.Queries.GetDishByIDForRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Domain.Entities;

namespace Restaurants.API.Controllers
{
    [Route("api/restaurant/{restaurantId}/Dishes")]
    [ApiController]
    public class DishesController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllForRestaurant([FromRoute] int restaurantId)
        {
           var dishes =  await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
            return Ok(dishes);

        }

        [HttpGet("{dishId}")]
        public async Task<IActionResult> GetByIdForRestaurant([FromRoute] int restaurantId,[FromRoute] int dishId)
        {
            var dish = await mediator.Send(new GetDishByIDForRestaurantQuery(restaurantId,dishId));
            return Ok(dish);

        }

        [HttpPost]
        public async Task<IActionResult> CreateDishe([FromRoute]int restaurantId,[FromBody] CreateDishCommand command)
        {
            command.RestaurantId = restaurantId;
           var id = await mediator.Send(command);

            return CreatedAtAction(nameof(GetByIdForRestaurant),new { restaurantId , id},null);
        }

        [HttpDelete]
        public async Task<IActionResult> CreateDishe([FromRoute] int restaurantId)
        {
            await mediator.Send(new DeleteDishesForRestaurantCommand(restaurantId));

            return NoContent();
        }


    }
}
