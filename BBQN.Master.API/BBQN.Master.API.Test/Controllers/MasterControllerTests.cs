using BBQN.Master.API.Controllers;
using BBQN.Master.API.Models;
using BBQN.Master.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace BBQN.Master.API.Test.Controllers
{
    [TestClass]
    public class MasterControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<MasterController>> _mockLogger;
        private readonly Mock<IMasterService> _mockMasterService;
        private readonly MasterController _controller;
        public MasterControllerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<MasterController>>();
            _mockMasterService = new Mock<IMasterService>();
            _controller = new MasterController(_mockConfiguration.Object, _mockLogger.Object, _mockMasterService.Object);
        }

        [TestMethod]
        public void Should_Return_Master_Data()
        {
            MasterData data = new MasterData();
            data.Company = new List<Company> { new Company { CompanyID = 1, CompanyName = "Xyz" } };
            _mockMasterService.Setup(m => m.FetchMasterData()).ReturnsAsync(data);
            var obj = _controller.GetMasterData().Result;
            var res = (obj as OkObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void Should_Master_Data_Throws_Exception()
        {
            _mockMasterService.Setup(m => m.FetchMasterData()).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.GetMasterData().Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Should_Return_Sub_Department_By_DepartmentID()
        {
            List<SubDepartments> subDepartments = new List<SubDepartments>() { new SubDepartments { DepartmentID = 1, SubDepartmentID = 1, SubDepartmentName = "test" } };
            _mockMasterService.Setup(m => m.GetSubDepartments(It.IsAny<int>())).ReturnsAsync(subDepartments);
            var obj = _controller.GetSubDepartments(It.IsAny<int>()).Result;
            var res = (obj as OkObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void Should_Sub_Departments_Throws_Exception()
        {
            _mockMasterService.Setup(m => m.GetSubDepartments(It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.GetSubDepartments(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Should_Return_Outlets_By_RegionID()
        {
            List<Outlets> outlets = new List<Outlets>() { new Outlets { OutletID = 1, OutletName = "test" } };
            _mockMasterService.Setup(m => m.GetOutlets(It.IsAny<int>())).ReturnsAsync(outlets);
            var obj = _controller.GetOutlets(It.IsAny<int>()).Result;
            var res = (obj as OkObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void Should_Outlets_Throws_Exception()
        {
            _mockMasterService.Setup(m => m.GetOutlets(It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.GetOutlets(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Should_Return_Privileges_List()
        {
            List<Privileges> privileges = new List<Privileges>() { new Privileges { PrivilegeID = 1, PrivilegeUniqueKey = "test", Description = "test" } };
            _mockMasterService.Setup(m => m.GetAllPrivileges()).ReturnsAsync(privileges);
            var obj = _controller.GetAllPrivileges().Result;
            var res = (obj as OkObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void Should_GetAllPrivileges_Throws_InternalServerException()
        {
            _mockMasterService.Setup(m => m.GetAllPrivileges()).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.GetAllPrivileges().Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }
    }
}
