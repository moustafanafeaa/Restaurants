using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance;
using System.Formats.Asn1;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Repositories;
//why it is not public

internal class RestaurantRepository(RestaurantDbContext context) : IRestaurantRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await context.Restaurants.AsNoTracking().ToListAsync();
    }
    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(
        string? searchPhrase,
        int pageSize,
        int pageNumber,
        string? sortBy,
        SortDirection sortDirection)
    {
        var searchPhraseLower = searchPhrase?.ToLower();

        var baseQuery = context.Restaurants
            .Where(r => searchPhraseLower == null || (r.Name.ToLower().Contains(searchPhraseLower)
                       || r.Description.ToLower().Contains(searchPhraseLower)));


        var count = await baseQuery.CountAsync();

        if(sortBy != null)
        {
            var columnSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                {nameof(Restaurant.Name), r => r.Name },
                {nameof(Restaurant.Description), r => r.Description },
                {nameof(Restaurant.Category), r => r.Category }
            };

            var selectedColumn = columnSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        var result =  await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (result, count);
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        return await context.Restaurants
            .AsNoTracking()
            .Include(x=>x.Dishes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<int> CreateAsync(Restaurant restaurant)
    {
         await context.AddAsync(restaurant);
         await context.SaveChangesAsync();
         return restaurant.Id;
    }

    public async Task DeleteAsync(Restaurant restaurant)
    {
        context.Remove(restaurant);
        await context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Restaurant restaurant)
    {
        context.Restaurants.Update(restaurant);
        await context.SaveChangesAsync();
    }
    public  Task SaveChanges()
        => context.SaveChangesAsync();

    
}

