using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Tests;
using Restaurants.Application.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Net.Http.Json;
using Xunit;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock = new();
    public RestaurantControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantRepository),
                                                                _ => _restaurantRepositoryMock.Object));
            });
        });
    }


    [Fact()]
    public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
    {
        //arrange
        var id = 1;
       _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);
        var client = _factory.CreateClient();

        //act
        var result = await client.GetAsync($"/api/restaurant/{id}");

        //assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
    [Fact()]
    public async Task GetById_ForExistingId_ShouldReturn200OK()
    {
        //arrange
        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "test",
            Description = "testt"
        };
        _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restaurant.Id)).ReturnsAsync(restaurant);
        var client = _factory.CreateClient();

        //act
        var result = await client.GetAsync($"/api/restaurant/{restaurant.Id}");
        var restaurantDto = await result.Content.ReadFromJsonAsync<RestaurantDto>();

        //assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be("test");
        restaurantDto.Description.Should().Be("testt");
    }



    [Fact()]
    public async Task GetAll_ForValidRequest_Return200Ok()
    {
        //arrange
        var client = _factory.CreateClient();

        //act
        var result =  await client.GetAsync("/api/restaurant?PageSize=5&PageNumber=1");

        //assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

   
}