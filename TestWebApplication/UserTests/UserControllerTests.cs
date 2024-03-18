using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TestDataAccess.Contracts;
using TestServiceModel;
using TestWebApplication.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace UserRandomTests
{
    public class UserControllerTests
    {
        [Test]
        public async Task GetUser_ValidAuthentication_ReturnsOk()
        {
            // Arrange
            var mockRandomUserDataService = new Mock<IRandomUserDataService>();
            mockRandomUserDataService.Setup(service => service.GetRandomUserData())
                                     .ReturnsAsync(new RandomUserApiResponse());

            var controller = new UserController(mockRandomUserDataService.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Basic YWRtaW46cGFzc3dvcmQ="; // "admin:password" base64 encoded

            // Act
            var result = await controller.GetUser() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public async Task GetUser_InvalidAuthentication_ReturnsUnauthorized()
        {
            // Arrange
            var mockRandomUserDataService = new Mock<IRandomUserDataService>();

            var controller = new UserController(mockRandomUserDataService.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Basic invalid_credentials";

            // Act
            var result = await controller.GetUser() as UnauthorizedResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);
        }

        [Test]
        public async Task GetUser_ServiceError_ReturnsStatusCode500()
        {
            // Arrange
            var mockRandomUserDataService = new Mock<IRandomUserDataService>();
            mockRandomUserDataService.Setup(service => service.GetRandomUserData())
                                     .ThrowsAsync(new Exception("Test exception"));

            var controller = new UserController(mockRandomUserDataService.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Basic YWRtaW46cGFzc3dvcmQ="; // "admin:password" base64 encoded

            // Act
            var result = await controller.GetUser() as StatusCodeResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);
        }
    }
}
