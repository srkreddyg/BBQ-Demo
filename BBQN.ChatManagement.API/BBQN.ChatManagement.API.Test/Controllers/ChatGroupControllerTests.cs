using BBQN.ChatManagement.API.Controllers;
using BBQN.ChatManagement.API.Models;
using BBQN.ChatManagement.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace BBQN.ChatManagement.API.Test.Controllers
{
    [TestClass]
    public class ChatGroupControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<ChatGroupController>> _mockLogger;
        private readonly Mock<IChatGroupService> _mockChatService;
        private readonly ChatGroupController _controller;
        public ChatGroupControllerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<ChatGroupController>>();
            _mockChatService = new Mock<IChatGroupService>();
            _controller = new ChatGroupController(_mockConfiguration.Object, _mockLogger.Object, _mockChatService.Object);
        }
        
        [TestMethod]
        public void Create_Chat_Group()
        {
            _mockChatService.Setup(m => m.CreateGroup(It.IsAny<GroupUsers>())).ReturnsAsync(1);
            var obj = _controller.CreateGroup(It.IsAny<GroupUsers>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void ChatGroup_Throws_InternalException()
        {
            _mockChatService.Setup(m => m.CreateGroup(It.IsAny<GroupUsers>())).ThrowsAsync(new Exception("Internal server error"));
         var obj = _controller.CreateGroup(It.IsAny<GroupUsers>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError,res);
        }

        [TestMethod]
        public void Update_Group_Chat()
        {
            _mockChatService.Setup(m => m.UpdateGroup(It.IsAny<GroupUsers>())).ReturnsAsync(true);
            var obj = _controller.UpdateGroup(It.IsAny<GroupUsers>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void UpdateGroup_Returns_InternalServerException()
        {
            _mockChatService.Setup(m => m.UpdateGroup(It.IsAny<GroupUsers>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.UpdateGroup(It.IsAny<GroupUsers>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Delete_Group_Chat_By_GroupID()
        {
            _mockChatService.Setup(m => m.DeleteGroup(It.IsAny<int>())).ReturnsAsync(true);
            var obj = _controller.DeleteGroup(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void DeleteGroup_Returns_InternalServerException()
        {
            _mockChatService.Setup(m => m.DeleteGroup(It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.DeleteGroup(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Remove_User_FromGroup_By_GroupID_And_UserID()
        {
            _mockChatService.Setup(m => m.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            var obj = _controller.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void RemoveUserFromGroup_Returns_InternalServerException()
        {
            _mockChatService.Setup(m => m.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Return_GetAllGroups_List()
        {
            List<Group> groups = new List<Group>() { new Group() { GradeID =1,GroupName ="BBQ",GroupDescription ="Xys"} };
            _mockChatService.Setup(m => m.GetAllGroups()).ReturnsAsync(groups);
            var obj = _controller.GetAllGroups().Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void GetAllGroups_Returns_InternalServerException()
        {
            _mockChatService.Setup(m => m.GetAllGroups()).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.GetAllGroups().Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Return_GetAllGroups_Filtered_By_UserID()
        {
            List<Group> groups = new List<Group>() { new Group() { GradeID = 1, GroupName = "BBQ", GroupDescription = "Xys" } };
            _mockChatService.Setup(m => m.GetAllGroups(It.IsAny<int>())).ReturnsAsync(groups);
            var obj = _controller.GetAllGroups(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void GetAllGroups_Returns_InternalServerException_while_filtered_by_userID()
        {
            _mockChatService.Setup(m => m.GetAllGroups(It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.GetAllGroups(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Return_GetAllUsers_Filtered_By_GroupID()
        {
            List<User> groups = new List<User>() { new User() { GradeID = 1, UserID = 1, FirstName = "Xys" } };
            _mockChatService.Setup(m => m.GetAllUser(It.IsAny<int>())).ReturnsAsync(groups);
            var obj = _controller.GetAllUser(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void GetAllUsers_Returns_InternalServerException_while_filtered_by_groupID()
        {
            _mockChatService.Setup(m => m.GetAllUser(It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.GetAllUser(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void BlockUser_Returns_BoolValue_As_True()
        {
            _mockChatService.Setup(m => m.BlockUser(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            var obj = _controller.BlockUser(It.IsAny<int>(), It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void BlockUser_Returns_InternalServerException()
        {
            _mockChatService.Setup(m => m.BlockUser(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.BlockUser(It.IsAny<int>(), It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Return_GetAllUsers_Filtered_By_Conditions()
        {
            List<User> groups = new List<User>() { new User() { GradeID = 1, UserID = 1, FirstName = "Xys" } };
            _mockChatService.Setup(m => m.GetAllUsers(It.IsAny<GetUserFilter>())).ReturnsAsync(groups);
            var obj = _controller.GetAllUsers(It.IsAny<GetUserFilter>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void GetAllUsers_Returns_InternalServerException_while_filtered_by_condition()
        {
            _mockChatService.Setup(m => m.GetAllUsers(It.IsAny<GetUserFilter>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.GetAllUsers(It.IsAny<GetUserFilter>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

    }
}
