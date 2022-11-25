using BBQN.UserManagement.API.Controllers;
using BBQN.UserManagement.API.Models;
using BBQN.UserManagement.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace BBQN.UserManagement.API.Test.Controllers
{
    [TestClass]
    public class AdminControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<AdminController>> _mockLogger;
        private readonly Mock<IAdminService> _mockAdminService;
        private readonly AdminController _controller;
        
        public AdminControllerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<AdminController>>();
            _mockAdminService = new Mock<IAdminService>();
            _controller = new AdminController(_mockConfiguration.Object, _mockLogger.Object, _mockAdminService.Object);
        }

        [TestMethod]
        public void Should_Return_Admin_List()
        {
            List<Users> users = new List<Users>() { new Users { UserID = 1, GradeID = 1, FirstName = "test" } };
            _mockAdminService.Setup(m => m.GetAllAdmins()).ReturnsAsync(users);
            var obj = _controller.GetAllAdmins().Result;
            var res = (obj as OkObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void Should_GetAllAdmin_Throws_InternalServerException()
        {
            _mockAdminService.Setup(m => m.GetAllAdmins()).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.GetAllAdmins().Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Update_Admin_Data()
        {
            _mockAdminService.Setup(m => m.UpdateAdmin(It.IsAny<Admin>())).ReturnsAsync(true);
            var obj = _controller.SetAdmin(It.IsAny<Admin>()).Result;
            var res = (obj as OkObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }


        [TestMethod]
        public void Should_UpdateAdmin_Throws_InternalServerException()
        {
            _mockAdminService.Setup(m => m.UpdateAdmin(It.IsAny<Admin>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.SetAdmin(It.IsAny<Admin>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Should_Return_User_By_UserID()
        {
            List<Users> users = new List<Users>() { new Users { UserID = 1, GradeID = 1, FirstName = "test" } };
            _mockAdminService.Setup(m => m.GetUser(It.IsAny<int>())).ReturnsAsync(users);
            var obj = _controller.GetUser(It.IsAny<int>()).Result;
            var res = (obj as OkObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void Should_User_By_UserID_Throws_InternalServerException()
        {
            _mockAdminService.Setup(m => m.GetUser(It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.GetUser(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Should_Add_Or_Remove_Privilege_Rights_To_Admin()
        {
            List<int> privilegeID = new List<int> { 1, 2, 3 };
            _mockAdminService.Setup(m => m.PrivilegeRightsToAdmin(privilegeID.ToString(),It.IsAny<int>())).ReturnsAsync(1);
            var obj = _controller.PrivilegeRightsToAdmin(privilegeID, It.IsAny<int>()).Result;
            var res = (obj as OkObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }



        [TestMethod]
        public void Should_Add_Or_Remove_Privilege_Rights_To_Admin_Throws_InternalServerException()
        {
            _mockAdminService.Setup(m => m.PrivilegeRightsToAdmin(It.IsAny<string>(),It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.PrivilegeRightsToAdmin(It.IsAny<List<int>>(), It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }
        [TestMethod]
        public void Blok_Unblok_User()
        {
            _mockAdminService.Setup(m => m.BlockUnblockUser(It.IsAny<Admin>())).ReturnsAsync(true);
            var obj = _controller.BlockUnblockUser(It.IsAny<Admin>()).Result;
            var res = (obj as OkObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }
        [TestMethod]
        public void Should_Blok_Unblok_Use_Throws_InternalServerException()
        {
            _mockAdminService.Setup(m => m.BlockUnblockUser(It.IsAny<Admin>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.BlockUnblockUser(It.IsAny<Admin>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }
    }
}
