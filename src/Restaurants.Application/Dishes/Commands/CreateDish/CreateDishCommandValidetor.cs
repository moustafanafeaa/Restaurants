using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Commands.CreateDish
{
    public class CreateDishCommandValidetor : AbstractValidator<CreateDishCommand>
    {
        public CreateDishCommandValidetor()
        {
            RuleFor(n => n.Price)
                .GreaterThanOrEqualTo(0).
                WithMessage("Price can't be a negative number");

            RuleFor(n => n.KiloCalories)
               .GreaterThanOrEqualTo(0).
               WithMessage("Calories can't be a negative number");
        }
    }
}
