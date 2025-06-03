using Xunit;
using Restaurants.API.MiddleWares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Restaurants.Domain.Exeptions;
using Restaurants.Domain.Entities;
using FluentAssertions;
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.MiddleWares.Tests
{
    public class ErrorHandlingMiddleTests
    {
        [Fact()]
        public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
        {
            //arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(loggerMock.Object);
            var context = new DefaultHttpContext();
            var nextDelegateMock = new Mock<RequestDelegate>();


            //act 
            await middleware   .InvokeAsync(context,nextDelegateMock.Object);

            //assert 
            nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
        }

        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldReturn404NotFount()
        {
            //arrange
          
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(loggerMock.Object);
            var context = new DefaultHttpContext();
            var notFoundException = new NotFoundException(nameof(Restaurant), "1");

            //act 
            await middleware.InvokeAsync(context, _ => throw notFoundException);

            //assert 
            context.Response.StatusCode.Should().Be(404);
        }
        [Fact()]
        public async Task InvokeAsync_WhenForbiddenExceptionThrown_ShouldReturn403Forbidden()
        {
            //arrange

            var loggerMock = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(loggerMock.Object);
            var context = new DefaultHttpContext();
            var forbiddenException = new ForbiddenException();

            //act 
            await middleware.InvokeAsync(context, _ => throw forbiddenException);

            //assert 
            context.Response.StatusCode.Should().Be(403);
        }

        [Fact()]
        public async Task InvokeAsync_WhenExceptionThrown_ShouldReturn500ServerError()
        {
            //arrange

            var loggerMock = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(loggerMock.Object);
            var context = new DefaultHttpContext();
            var exception = new Exception();

            //act 
            await middleware.InvokeAsync(context, _ => throw exception);

            //assert 
            context.Response.StatusCode.Should().Be(500);
        }


    }
}