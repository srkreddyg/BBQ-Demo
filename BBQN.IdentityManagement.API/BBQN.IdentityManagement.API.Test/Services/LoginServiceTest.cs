using BBQN.IdentityManagement.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using BBQN.DBFactory;
using Moq;
using BBQN.IdentityManagement.API.DataAccess;
using BBQN.IdentityManagement.API.Common;

namespace BBQN.IdentityManagement.API.Test.Services
{
    [TestClass]
    public class LoginServiceTest
    {
        private readonly Mock<ILoginDBAdaptor> _mockLoginadaptor;
        private readonly LoginService _loginService;
        public LoginServiceTest()
        {
            _mockLoginadaptor = new Mock<ILoginDBAdaptor>();
            _loginService = new LoginService(_mockLoginadaptor.Object);
        }

        [TestMethod]
        public void GenerateOTP_Return_OTP()
        {
            _mockLoginadaptor.Setup(m => m.GenerateOTP(It.IsAny<string>(), It.IsAny<LoginType>())).ReturnsAsync(1);
            var res = _loginService.GenerateOTP(It.IsAny<string>(), It.IsAny<LoginType>()).Result;
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void Check_Whether_User_IS_Authenticated_Or_Not()
        {
            _mockLoginadaptor.Setup(m => m.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<LoginType>())).ReturnsAsync(true);
            bool isAuthenticated = _loginService.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<LoginType>()).Result;
            Assert.IsTrue(isAuthenticated);
        }
    }
}
