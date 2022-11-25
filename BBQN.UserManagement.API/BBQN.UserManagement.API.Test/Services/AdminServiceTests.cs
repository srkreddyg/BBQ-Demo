using BBQN.UserManagement.API.Controllers;
using BBQN.UserManagement.API.DataAccess;
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

namespace BBQN.UserManagement.API.Test.Services
{
    [TestClass]
    public class AdminServiceTests
    {
        private readonly Mock<IAdminDBAdaptor> _mockAdminDBAdaptor;
        private readonly AdminService _adminService;
        public AdminServiceTests()
        {
            _mockAdminDBAdaptor = new Mock<IAdminDBAdaptor>();
            _adminService = new AdminService(_mockAdminDBAdaptor.Object);
        }

        [TestMethod]
        public void Update_Admin_Data()
        {
            _mockAdminDBAdaptor.Setup(m => m.UpdateAdmin(It.IsAny<Admin>())).ReturnsAsync(true);
            var res = _adminService.UpdateAdmin(It.IsAny<Admin>()).Result;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Should_Return_Admin_List()
        {
            List<Users> users = new List<Users>() { new Users { UserID = 1, GradeID = 1, FirstName = "test" } };
            _mockAdminDBAdaptor.Setup(m => m.GetAllAdmins()).ReturnsAsync(users);
            var res = _adminService.GetAllAdmins().Result;
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Should_Return_User()
        {
            List<Users> users = new List<Users>() { new Users { UserID = 1, GradeID = 1, FirstName = "test" } };
            _mockAdminDBAdaptor.Setup(m => m.GetUser(It.IsAny<int>())).ReturnsAsync(users);
            var res = _adminService.GetUser(It.IsAny<int>()).Result;
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Should_Add_or_Remove_Privilege_Rights_to_Admin()
        {
            _mockAdminDBAdaptor.Setup(m => m.PrivilegeRightsToAdmin(It.IsAny<string>(),It.IsAny<int>())).ReturnsAsync(1);
            var res = _adminService.PrivilegeRightsToAdmin(It.IsAny<string>(), It.IsAny<int>()).Result;
            Assert.IsNotNull(res);
        }
        [TestMethod]
        public void Blok_Unblok_User()
        {
            _mockAdminDBAdaptor.Setup(m => m.BlockUnblockUser(It.IsAny<Admin>())).ReturnsAsync(true);
            var res = _adminService.BlockUnblockUser(It.IsAny<Admin>()).Result;
            Assert.IsTrue(res);
        }

    }
}
