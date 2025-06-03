using FluentValidation;
using Restaurants.Application.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
    {
        private int[] allowSize = [5, 10, 15, 30];
        private string[] allowSortByColumnNames = [nameof(RestaurantDto.Name), nameof(RestaurantDto.Description), nameof(RestaurantDto.Category) ];

        public GetAllRestaurantsQueryValidator()
        {
            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(r => r.PageSize)
                .Must(c => allowSize.Contains(c))
                .WithMessage($"page size must be in [{string.Join(",", allowSize)}]");

            RuleFor(r => r.SortBy)
             .Must(c => allowSortByColumnNames.Contains(c))
             .When(q => q.SortBy != null)
             .WithMessage($"sort by is optional, or must be in [{string.Join(",", allowSortByColumnNames)}]");
        }
    }
}
