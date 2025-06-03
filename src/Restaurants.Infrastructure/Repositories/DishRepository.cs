using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance;
using System;
using System.Collections.Generic;

namespace Restaurants.Infrastructure.Repositories;

//why it is not public
internal class DishRepository(RestaurantDbContext context) : IDishRepository
{
    public async  Task<int> CreateAsync(Dish dish)
    {
       context.Dishes.Add(dish);
       await context.SaveChangesAsync();
       return dish.Id;
    }

    public async Task DeleteDishesForRestaurant(IEnumerable<Dish> dishes)
    {
        context.Dishes.RemoveRange(dishes);
        await context.SaveChangesAsync();
    }
}
