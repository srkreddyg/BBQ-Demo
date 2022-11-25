using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BBQN.DBFactory;
using Moq;
using BBQN.ChatManagement.API.DataAccess;
using BBQN.ChatManagement.API.Services;
using BBQN.ChatManagement.API.Models;
using System.Collections.Generic;

namespace BBQN.ChatManagement.API.Test.Services
{
    [TestClass]
    public class ChatGroupServiceTest
    {
        private readonly Mock<IChatDBAdaptor> _mockChatDBAdaptor;
        private readonly ChatGroupService _chatGroupService;
        public ChatGroupServiceTest()
        {
            _mockChatDBAdaptor = new Mock<IChatDBAdaptor>();
            _chatGroupService = new ChatGroupService(_mockChatDBAdaptor.Object);
        }

        [TestMethod]
        public void Create_Chat_Group()
        {
            _mockChatDBAdaptor.Setup(m => m.CreateGroup(It.IsAny<GroupUsers>())).ReturnsAsync(1);
            var res = _chatGroupService.CreateGroup(It.IsAny<GroupUsers>()).Result;
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void UpdateGroup_By_GroupData()
        {
            _mockChatDBAdaptor.Setup(m => m.UpdateGroup(It.IsAny<GroupUsers>())).ReturnsAsync(true);
            var res = _chatGroupService.UpdateGroup(It.IsAny<GroupUsers>()).Result;
           Assert.IsTrue(res);
        }

        [TestMethod]
        public void Delete_Group_By_GroupID()
        {
            _mockChatDBAdaptor.Setup(m => m.DeleteGroup(It.IsAny<int>())).ReturnsAsync(true);
            var res = _chatGroupService.DeleteGroup(It.IsAny<int>()).Result;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Remove_User_From_Group_By_GroupID_And_UserID()
        {
            _mockChatDBAdaptor.Setup(m => m.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            var res = _chatGroupService.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>()).Result;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Return_User_List_By_FilterCondition()
        {
            List<User> users = new List<User>() { new User { UserID=1, FirstName = "test",IsAdmin=false} };
            _mockChatDBAdaptor.Setup(m => m.GetAllUsers(It.IsAny<GetUserFilter>())).ReturnsAsync(users);
            var res = _chatGroupService.GetAllUsers(It.IsAny<GetUserFilter>()).Result;
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Return_All_Groups_List()
        {
            List<Group> group = new List<Group> { new Group { GradeID=1,GroupDescription="test",GroupName="test"} };
            _mockChatDBAdaptor.Setup(m => m.GetAllGroups()).ReturnsAsync(group);
            var res = _chatGroupService.GetAllGroups().Result;
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Return_All_Groups_List_Filtered_By_UserID()
        {
            List<Group> group = new List<Group> { new Group { GradeID = 1, GroupDescription = "test", GroupName = "test" } };
            _mockChatDBAdaptor.Setup(m => m.GetAllGroups(It.IsAny<int>())).ReturnsAsync(group);
            var res = _chatGroupService.GetAllGroups(It.IsAny<int>()).Result;
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Return_All_Users_List_Filtered_By_GroupID()
        {
            List<User> user = new List<User> { new User { GradeID = 1, FirstName = "test", RoleID = 1 } };
            _mockChatDBAdaptor.Setup(m => m.GetAllUser(It.IsAny<int>())).ReturnsAsync(user);
            var res = _chatGroupService.GetAllUser(It.IsAny<int>()).Result;
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Block_User_By_GroupID_And_UserID()
        {
            _mockChatDBAdaptor.Setup(m => m.BlockUser(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            var res = _chatGroupService.BlockUser(It.IsAny<int>(), It.IsAny<int>()).Result;
            Assert.IsTrue(res);
        }
    }
}
