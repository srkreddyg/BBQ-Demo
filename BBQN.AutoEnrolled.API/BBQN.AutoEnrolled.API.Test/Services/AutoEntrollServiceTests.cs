using BBQN.AutoEnrolled.API.DataAccess;
using BBQN.AutoEnrolled.API.Models;
using BBQN.AutoEnrolled.API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBQN.AutoEntrolled.API.Test.Services
{
    [TestClass]
    public class AutoEntrollServiceTests
    {
        private readonly Mock<IAutoEnrollDbAdaptor> __mockAutoEntrollDBAdaptor;
        private readonly AutoEnrollService _autoEntrollService;

        public AutoEntrollServiceTests()
        {
            __mockAutoEntrollDBAdaptor = new Mock<IAutoEnrollDbAdaptor>();
            _autoEntrollService = new AutoEnrollService(__mockAutoEntrollDBAdaptor.Object);
        }

        [TestMethod]
        public void Create_AutoEntrollUser_Test()
        {
            __mockAutoEntrollDBAdaptor.Setup(m => m.AddAutoEnrollUser(It.IsAny<int>(),It.IsAny<User>())).ReturnsAsync(true);
            var res = _autoEntrollService.AddAutoEnrollUser(It.IsAny<int>(), It.IsAny<User>()).Result;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Remove_AutoEntrollUser_Test()
        {
            __mockAutoEntrollDBAdaptor.Setup(m => m.RemoveUserAutoEnroll(It.IsAny<int>())).ReturnsAsync(true);
            var res = _autoEntrollService.RemoveUserAutoEnroll(It.IsAny<int>()).Result;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Return_Group_List_By_ID()
        {
            List<Group> list = new List<Group>() { new Group() { GroupId = 1, GradeName = "BBQ", GroupDescription = "Xys" } };
            __mockAutoEntrollDBAdaptor.Setup(m => m.GetMatchedGroupIds(It.IsAny<GetUserFilter>())).ReturnsAsync(list);
            var res = _autoEntrollService.GetMatchedGroupIds(It.IsAny<GetUserFilter>()).Result;
            Assert.IsNotNull(res);
        }

    }
}
