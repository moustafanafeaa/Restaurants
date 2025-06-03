using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Domain.Repositories
{
    public interface IDishRepository
    {
        Task<int> CreateAsync(Dish dish);
        Task DeleteDishesForRestaurant(IEnumerable<Dish> dishes);

    }
}
