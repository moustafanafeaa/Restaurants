using Xunit;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandValidatorTests
    {
        [Fact()]
        public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
        {
            //arrange 
            var command = new CreateRestaurantCommand()
            {
                Name = "Testt",
                Category = "Italian",
                Description = "test ",
            };

            var validator = new CreateRestaurantCommandValidator();
            //act
           var result =  validator.TestValidate(command);
            //assert 
            result.ShouldNotHaveAnyValidationErrors();

        }
        [Fact()]
        public void Validator_ForNotValidCommand_ShouldHaveValidationErrors()
        {
            //arrange 
            var command = new CreateRestaurantCommand()
            {
                Name = "Tt",
                Category = "test",
            };

            var validator = new CreateRestaurantCommandValidator();
            //act
            var result = validator.TestValidate(command);
            //assert 
            result.ShouldHaveValidationErrorFor(c => c.Name);
            result.ShouldHaveValidationErrorFor(c => c.Category);
            result.ShouldHaveValidationErrorFor(c=>c.Description);

        }

        [Theory()]
        [InlineData("Italian")]
        [InlineData("Egyptian Cuisine")]
        [InlineData("Fast Food")]

        public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
        {
            //arrange 
            var command = new CreateRestaurantCommand()
            {
                Category = category
            };

            var validator = new CreateRestaurantCommandValidator();
            //act
            var result = validator.TestValidate(command);
            //assert 
            result.ShouldNotHaveValidationErrorFor(c => c.Category);

        }
    }
}