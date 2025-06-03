using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
    {
        public UpdateRestaurantCommandValidator()
        {

            RuleFor(dto => dto.Name)
                    .Length(5, 100);
            RuleFor(dto => dto.Description)
                    .NotEmpty().WithMessage("Description is required");
        }

    }
}
