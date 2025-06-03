using FluentValidation;
using Restaurants.Application.Dtos;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
    {
        private readonly List<string> validCategories = ["Italian", "Egyptian Cuisine", "Fast Food"];
        public CreateRestaurantCommandValidator()
        {
            RuleFor(dto => dto.Category)
                .Must(validCategories.Contains)
                .WithMessage("invalid category, please choose from the valid categories.");

            RuleFor(dto => dto.Name)
                .Length(5, 100);
            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description is required");
        }
    }
}
