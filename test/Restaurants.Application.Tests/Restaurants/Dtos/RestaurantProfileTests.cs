using AutoMapper;
using Castle.Core.Configuration;
using FluentAssertions;
using Restaurants.Application.Dtos;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Xunit;
 
namespace Restaurants.Application.Restaurants.Dtos.Tests;

public class RestaurantProfileTests
{
    private IMapper _mapper;

    public RestaurantProfileTests()
    {
        var configurantion = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantProfile>();
        });

        _mapper = configurantion.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        //arrange
        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "Test",
            Category = "Test Category",
            HasDelivery = true,
            Description = "Test Desc",
            ContactEmail = "test@example.com",
            ContactNumber = "123456",
            Address = new Address()
            {
                City = "Test City",
                PostalCode = "123",
                Street = "Test Street"
            }
        };

        //action
        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        //assert
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.ContactEmail.Should().Be(restaurant.ContactEmail);
        restaurantDto.ContactNumber.Should().Be(restaurant.ContactNumber);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);

    }

    [Fact()]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        //arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test",
            Category = "Test Category",
            HasDelivery = true,
            Description = "Test Desc",
            ContactEmail = "test@example.com",
            ContactNumber = "123456",
            City = "Test City",
            PostalCode = "123",
            Street = "Test Street"
        };

        //action
        var restaurant = _mapper.Map<Restaurant>(command);

        //assert
        restaurant.Should().NotBeNull();
        restaurant.Category.Should().Be(command.Category);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.ContactNumber.Should().Be(command.ContactNumber);
        restaurant.ContactEmail.Should().Be(command.ContactEmail);
        restaurant.Address.Should().NotBeNull();
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.PostalCode.Should().Be(command.PostalCode);

    }

    [Fact()]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        //arrange
        var command = new UpdateRestaurantCommand
        {
            Id = 1,
            Name = "Updated Name",
            HasDelivery = false,
            Description = "Updated Desc",
        };

        //action
        var restaurant = _mapper.Map<Restaurant>(command);

        //assert
        restaurant.Should().NotBeNull(); 
   //     restaurant.Id.Should().Be(command.Id);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
    }

}