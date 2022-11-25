using BBQN.AutoEnrolled.API.Common;
using BBQN.AutoEnrolled.API.Controllers;
using BBQN.AutoEnrolled.API.Integrations;
using BBQN.AutoEnrolled.API.Models;
using BBQN.AutoEnrolled.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BBQN.AutoEntrolled.API.Test.Controllers
{
    [TestClass]
    public class AutoEntrollControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<AutoEnrollController>> _mockLogger;
        private readonly Mock<IAutoEnrollService> _mockAutoEntrolllService;
        private readonly Mock<IIntegration> _mockIIntegration;
        private readonly AutoEnrollController _controller;
        public AutoEntrollControllerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<AutoEnrollController>>();
            _mockAutoEntrolllService = new Mock<IAutoEnrollService>();
            _mockIIntegration = new Mock<IIntegration>();
            _controller = new AutoEnrollController(_mockConfiguration.Object, _mockLogger.Object, _mockAutoEntrolllService.Object,_mockIIntegration.Object);
        }


        [TestMethod]
        public void AddAutoEnrollUser_Throws_InternalException()
        {
            var obj = _controller.AddAutoEnrollUser().Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void RemoveUserControll_Throws_InternalException()
        {
            var obj = _controller.RemoveUserAutoEnroll().Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

    }
}
