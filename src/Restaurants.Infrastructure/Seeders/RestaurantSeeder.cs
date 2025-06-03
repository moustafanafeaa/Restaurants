using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistance;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }


    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles = [
                new(UserRoles.User){
                    NormalizedName = UserRoles.User.ToUpper()
                },
                new(UserRoles.Owner){
                      NormalizedName = UserRoles.Owner.ToUpper()
                },
                new(UserRoles.Admin){
                      NormalizedName = UserRoles.Admin.ToUpper()
                }
            ];
        return roles;
    }
    private IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant> restaurants = [
            new()
            {
                Name = "El Mohamady",
                Category = "Egyptian Cuisine",
                Description = "El Mohamady offers traditional Egyptian dishes with a modern touch. A long-standing favorite for Mansoura locals, known for hearty meals and excellent service.",
                ContactEmail = "contact@elmohamady.com",
                HasDelivery = true,
                Dishes =
                [
                    new()
                    {
                        Name = "Koshary Special",
                        Description = "A traditional Egyptian dish with lentils, rice, macaroni, fried onions, and spicy tomato sauce.",
                        Price = 45.00M
                    },
                    new()
                    {
                        Name = "Mixed Grill Platter",
                        Description = "A platter featuring grilled kofta, kebabs, and chicken, served with tahini and rice.",
                        Price = 150.00M
                    },
                    new()
                    {
                        Name = "Molokhia with Chicken",
                        Description = "A classic Egyptian green soup served with tender chicken and white rice.",
                        Price = 85.00M
                    }
                ],
                Address = new()
                {
                    Street = "Talaat Harb Street",
                    City = "Mansoura",
                    PostalCode = "35512"
                }
            },
            new()
            {
                Name = "Bremer",
                Category = "International Cuisine",
                Description = "Bremer offers a variety of international dishes including Italian, Indian, and Chinese cuisines along with local favorites. Known for its innovative meals and cozy ambiance.",
                ContactEmail = "contact@bremer.com",
                HasDelivery = true,
                Dishes =
                [
                    new()
                    {
                        Name = "American Vitamin Salad",
                        Description = "A mix of greens topped with tuna, pastrami, cheese, and ranch sauce, served with fries and eggs.",
                        Price = 75.00M
                    },
                    new()
                    {
                        Name = "Butter Chicken",
                        Description = "Indian-style creamy chicken in a rich butter sauce served with basmati rice.",
                        Price = 120.00M
                    },
                    new()
                    {
                        Name = "Kung Pao Chicken",
                        Description = "Stir-fried chicken with peanuts, vegetables, and a spicy-sweet sauce, served with steamed rice.",
                        Price = 110.00M
                    }
                ],
                Address = new()
                {
                    Street = "Downtown Mansoura",
                    City = "Mansoura",
                    PostalCode = "35511"
                }
            },
            new()
            {
                Name = "Chicken Kickers",
                Category = "Fast Food",
                Description = "Chicken Kickers is the ultimate destination for crispy, juicy chicken meals and fiery wings. Known for its vibrant ambiance and bold flavors, it's a favorite for chicken lovers.",
                ContactEmail = "contact@chickenkickers.com",
                HasDelivery = true,
                Dishes =
                [
                    new()
                    {
                        Name = "Kickers' Spicy Wings",
                        Description = "A dozen spicy chicken wings, coated in our signature hot sauce, served with ranch dip.",
                        Price = 80.00M
                    },
                    new()
                    {
                        Name = "Crunchy Chicken Burger",
                        Description = "Crispy fried chicken breast with lettuce, pickles, and spicy mayo, served in a toasted bun.",
                        Price = 65.00M
                    },
                    new()
                    {
                        Name = "Loaded Fries",
                        Description = "Golden fries topped with melted cheese, crispy chicken bites, and Kickers' special sauce.",
                        Price = 45.00M
                    },
                    new()
                    {
                        Name = "Classic Chicken Bucket",
                        Description = "A bucket filled with 8 pieces of perfectly fried chicken, crispy on the outside and tender inside.",
                        Price = 150.00M
                    }
                ],
                Address = new()
                {
                    Street = "Shinnawi District",
                    City = "Mansoura",
                    PostalCode = "35514"
                }
            }
            ];
        return restaurants;
    }
}
