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
    public class HealthCheckTests
    {
        [Test]
        public void HealthCheck_ReturnsOK()
        {
            // Arrange
            var controller = new HealthController();
            // Act
            var result = controller.Get() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("OK", result.Value);
        }
    }
}
