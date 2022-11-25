using BBQN.PostManagement.API.Controllers;
using BBQN.PostManagement.API.Models;
using BBQN.PostManagement.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace BBQN.PostManagement.API.Test.Controllers
{
    [TestClass]
    public class PostControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<PostController>> _mockLogger;
        private readonly Mock<IPostService> _mockPostService;
        private readonly PostController _controller;

        public PostControllerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<PostController>>();
            _mockPostService = new Mock<IPostService>();
            _controller = new PostController(_mockConfiguration.Object, _mockLogger.Object, _mockPostService.Object);
        }

        [TestMethod]
        public void Create_Post_Test()
        {
            _mockPostService.Setup(m => m.CreatePost(It.IsAny<SocialPost>())).ReturnsAsync(1);
            var obj = _controller.CreatePost(It.IsAny<SocialPost>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void ChatPost_Throws_InternalException()
        {
            _mockPostService.Setup(m => m.CreatePost(It.IsAny<SocialPost>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.CreatePost(It.IsAny<SocialPost>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Update_Post_Test()
        {
            _mockPostService.Setup(m => m.UpdatePost(It.IsAny<SocialPost>())).ReturnsAsync(true);
            var obj = _controller.UpdatePost(It.IsAny<SocialPost>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void UpatePost_Throws_InternalException()
        {
            _mockPostService.Setup(m => m.UpdatePost(It.IsAny<SocialPost>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.UpdatePost(It.IsAny<SocialPost>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Delete_Post_Test()
        {
            _mockPostService.Setup(m => m.DeletePost(It.IsAny<int>())).ReturnsAsync(true);
            var obj = _controller.DeletePost(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void DeletePost_Throws_InternalException()
        {
            _mockPostService.Setup(m => m.DeletePost(It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.DeletePost(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Return_GetAllPost_List()
        {
            List<SocialPost> list = new List<SocialPost>() { new SocialPost() { GroupID = 1, PostTitle = "BBQ", PostDetails = "Xys" } };
            _mockPostService.Setup(m => m.GetAllPosts(It.IsAny<int>())).ReturnsAsync(list);
            var obj = _controller.GetAllPosts(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void GetAllPost_Returns_InternalServerException()
        {
            _mockPostService.Setup(m => m.GetAllPosts(It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.GetAllPosts(It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Like_Post_Test()
        {
            _mockPostService.Setup(m => m.LikePost(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            var obj = _controller.LikePost(It.IsAny<int>(), It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void Like_InternalException()
        {
            _mockPostService.Setup(m => m.LikePost(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.LikePost(It.IsAny<int>(), It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Return_GetAllReportedPost_List()
        {
            List<SocialPost> list = new List<SocialPost>() { new SocialPost() { GroupID = 1, PostTitle = "BBQ", PostDetails = "Xys" } };
            _mockPostService.Setup(m => m.GetAllReportedPosts()).ReturnsAsync(list);
            var obj = _controller.GetAllReportedPosts().Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void GetAllReportedPost_Returns_InternalServerException()
        {
            _mockPostService.Setup(m => m.GetAllReportedPosts()).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.GetAllReportedPosts().Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

        [TestMethod]
        public void Report_Post_Test()
        {
            _mockPostService.Setup(m => m.ReportPost(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            var obj = _controller.ReportPost(It.IsAny<int>(), It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status200OK, res);
        }

        [TestMethod]
        public void ReportPost_Throws_InternalException()
        {
            _mockPostService.Setup(m => m.ReportPost(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));
            var obj = _controller.ReportPost(It.IsAny<int>(), It.IsAny<int>()).Result;
            var res = (obj as ObjectResult).StatusCode;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, res);
        }

    }
}
