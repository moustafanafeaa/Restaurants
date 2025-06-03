
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using System.Globalization;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetByIdAsync(int id);
        Task<int> CreateAsync(Restaurant restaurant);
        Task DeleteAsync(Restaurant restaurant);
        Task UpdateAsync(Restaurant restaurant);
        Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPharse, int pageSize, int pageNumber, string? sortBy,
                SortDirection sortDirection);
        Task SaveChanges();
    }
}
