using BBQN.Master.API.DataAccess;
using BBQN.Master.API.Models;
using BBQN.Master.API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace BBQN.Master.API.Test.Services
{
    [TestClass]
    public class MasterServiceTests
    {
        private readonly Mock<IMasterDBAdaptor> _mockMasterDBAdaptor;
        private readonly MasterService _masterService;
        public MasterServiceTests()
        {
            _mockMasterDBAdaptor= new Mock<IMasterDBAdaptor>();
            _masterService = new MasterService(_mockMasterDBAdaptor.Object);
        }

        [TestMethod]
        public void Should_Return_Master_Data()
        {
            MasterData data = new MasterData();
            data.Company = new List<Company> { new Company { CompanyID = 1, CompanyName = "Xyz" } };
            _mockMasterDBAdaptor.Setup(m => m.FetchMasterData()).ReturnsAsync(data);
            var res = _masterService.FetchMasterData().Result;
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Should_Return_Sub_Department_By_DepartmentID()
        {
            List<SubDepartments> subDepartments = new List<SubDepartments>() { new SubDepartments { DepartmentID = 1, SubDepartmentID = 1, SubDepartmentName = "test" } };
            _mockMasterDBAdaptor.Setup(m => m.GetDepartments(It.IsAny<int>())).ReturnsAsync(subDepartments);
            var res = _masterService.GetSubDepartments(It.IsAny<int>()).Result;
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Should_Return_Outlets_By_RegionID()
        {
            List<Outlets> outlets = new List<Outlets>() { new Outlets { OutletID = 1, OutletName = "test" } };
            _mockMasterDBAdaptor.Setup(m => m.GetOutlets(It.IsAny<int>())).ReturnsAsync(outlets);
            var res = _masterService.GetOutlets(It.IsAny<int>()).Result;
            Assert.IsNotNull(res);
        }
        [TestMethod]
        public void Should_Return_Privilege_List()
        {
            List<Privileges> privileges = new List<Privileges>() { new Privileges { PrivilegeID = 1, PrivilegeUniqueKey = "test", Description = "test" } };
            _mockMasterDBAdaptor.Setup(m => m.GetAllPrivileges()).ReturnsAsync(privileges);
            var res = _masterService.GetAllPrivileges().Result;
            Assert.IsNotNull(res);
        }
    }
}
