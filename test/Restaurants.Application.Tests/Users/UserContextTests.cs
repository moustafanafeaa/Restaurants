using Xunit;
using Restaurants.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using FluentAssertions;

namespace Restaurants.Application.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {
            //arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var dateOfBirth = new DateOnly(2004, 1, 13);

            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, "1"),
                new(ClaimTypes.Name, "test"),
                new(ClaimTypes.Email, "test@gmail.com"),
                new("Nationality", "Egypt"),
                new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd")),
                new(ClaimTypes.Role, UserRoles.Admin),
                new(ClaimTypes.Role, UserRoles.User)
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims,"test"));

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user
            });
            var userContext  = new UserContext(httpContextAccessorMock.Object);



            //act
            var currentUser =  userContext.GetCurrentUser();
            //asset 

            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be("1");
            currentUser.Email.Should().Be("test@gmail.com");
            currentUser.Name.Should().Be("test");
            currentUser.Nationality.Should().Be("Egypt");
            currentUser.DateOfBirth.Should().Be(dateOfBirth);
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin,UserRoles.User);
        }

        [Fact()]
        public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
        {
            //arrange
            var httpAccessorMock = new Mock<IHttpContextAccessor>();

            httpAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

            var userContext = new UserContext(httpAccessorMock.Object);

            //act
            Action action = () => userContext.GetCurrentUser();
            //asset 

            action.Should().Throw<InvalidOperationException>().WithMessage("User context is not present") ;
        }
    }
}