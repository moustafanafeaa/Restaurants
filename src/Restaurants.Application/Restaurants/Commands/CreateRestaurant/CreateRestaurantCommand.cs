using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommand : IRequest<int>
    {
        [Required, StringLength(100, MinimumLength = 5)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string? Category { get; set; }
        public bool HasDelivery { get; set; }
        public string? ContactEmail { get; set; }
        [Phone(ErrorMessage = "please provide a valid number")]
        public string? ContactNumber { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "please provide a valid postal code (XX-XXX).")]
        public string? PostalCode { get; set; }
    }
}
