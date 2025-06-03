using Xunit;
using Restaurants.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Constants;
using FluentAssertions;

namespace Restaurants.Application.Users.Tests
{
    public class CurrentUserTests
    {
        [Theory()]
        [InlineData(UserRoles.Admin)]
        [InlineData(UserRoles.User)]
        public void IsInRoleTest_WithMatchingRole_ShouldReturnTrue(string roleName)
        {
            //arrange
            var user = new CurrentUser("1", "test", "test@test.com", null, null, [UserRoles.User,UserRoles.Admin]);

            //act
            var inRole = user.IsInRole(roleName);

            //assert
            inRole.Should().BeTrue();
        }

        [Fact()]
        public void IsInRoleTest_WithNoMatchingRole_ShouldReturnFalse()
        {
            //arrange
            var user = new CurrentUser("1", "test", "test@test.com", null, null, [UserRoles.User, UserRoles.Admin]);

            //act
            var inRole = user.IsInRole(UserRoles.Owner);

            //assert
            inRole.Should().BeFalse();
        }
    }
}