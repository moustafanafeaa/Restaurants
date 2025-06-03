using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Restaurants.Application.Dtos;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantController(IMediator mediator) : ControllerBase
{
    [HttpGet]

    public async Task<IActionResult> GetAll([FromQuery] GetAllRestaurantsQuery query)
    {
        var restaurant = await mediator.Send(query);
        return Ok(restaurant);
    }

    [HttpGet("{Id}")]
    [Authorize(Policy = PolicyNames.HasNationality)]
    public async Task<IActionResult> GetById(int Id)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(Id));
      
        return Ok(restaurant);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateRestaurantCommand restaurantCommand)
    {
        var id = await mediator.Send(restaurantCommand);

        return CreatedAtAction(nameof(GetById), new { id }, null);

    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteRestaurantCommand(id));
       
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id,UpdateRestaurantCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
       
        return NoContent();
    }
}

