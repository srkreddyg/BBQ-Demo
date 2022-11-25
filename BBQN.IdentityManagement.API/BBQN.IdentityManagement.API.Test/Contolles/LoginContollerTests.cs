using BBQN.IdentityManagement.API.Common;
using BBQN.IdentityManagement.API.Controllers;
using BBQN.IdentityManagement.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace BBQN.IdentityManagement.API.Test
{
    [TestClass]
    public class LoginContollerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<LoginController>> _mockLogger;
        private readonly Mock<ILoginService> _mockLoginService;
        private readonly LoginController _loginController;

        public LoginContollerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<LoginController>>();
            _mockLoginService = new Mock<ILoginService>();
            _loginController = new LoginController(_mockConfiguration.Object,_mockLogger.Object,_mockLoginService.Object);

        }
        [TestMethod]
        public void GenerateOTP_Success()
        {
            _mockLoginService.Setup(x => x.GenerateOTP(It.IsAny<string>(), It.IsAny<LoginType>())).ReturnsAsync(1);
            var obj = _loginController.GenerateOTP(It.IsAny<string>(), It.IsAny<LoginType>());
            var res = (obj.Result as OkObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK,res);
        }

        [TestMethod]
        public void GenerateOTP_BadRequest()
        {
            _mockLoginService.Setup(x => x.GenerateOTP(It.IsAny<string>(),It.IsAny<LoginType>())).ReturnsAsync(0);
            var obj = _loginController.GenerateOTP(It.IsAny<string>(), It.IsAny<LoginType>());
            var res = (obj.Result as BadRequestObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status400BadRequest, res);
        }

        [TestMethod]
        public void Authenticate_Success()
        {
            LoginType loginType = LoginType.Admin;
            _mockConfiguration.SetupGet(x => x[It.IsAny<string>()]).Returns("SecretKeyBBQN141022");
            _mockLoginService.Setup(x => x.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<LoginType>())).ReturnsAsync(true);
            var obj = _loginController.Authenticate("test", It.IsAny<int>(), loginType, true);
            var res = (obj.Result as OkObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void Authenticate_BadRequest()
        {
            _mockLoginService.Setup(x => x.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<LoginType>())).ReturnsAsync(false);
            var obj = _loginController.Authenticate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<LoginType>());
            var res = (obj.Result as BadRequestObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status400BadRequest, res);
        }

        [TestMethod]
        public void Authenticate_Token_From_ZingHR()
        {
            var obj = _loginController.AuthenticateToken(It.IsAny<int>());
            var res = (obj as OkObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }
    }
}