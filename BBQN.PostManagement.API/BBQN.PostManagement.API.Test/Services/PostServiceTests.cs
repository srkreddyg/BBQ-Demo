using BBQN.PostManagement.API.DataAccess;
using BBQN.PostManagement.API.Models;
using BBQN.PostManagement.API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace BBQN.PostManagement.API.Test.Services
{
    [TestClass]
    public class PostServiceTests
    {
        private readonly Mock<IPostDBAdaptor> __mockPostDBAdaptor;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            __mockPostDBAdaptor = new Mock<IPostDBAdaptor>();
            _postService = new PostService(__mockPostDBAdaptor.Object);
        }

        [TestMethod]
        public void Create_Post_Test()
        {
            __mockPostDBAdaptor.Setup(m => m.CreatePost(It.IsAny<SocialPost>())).ReturnsAsync(1);
            var res = _postService.CreatePost(It.IsAny<SocialPost>()).Result;
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void Update_Post_Test()
        {
            __mockPostDBAdaptor.Setup(m => m.UpdatePost(It.IsAny<SocialPost>())).ReturnsAsync(true);
            var res = _postService.UpdatePost(It.IsAny<SocialPost>()).Result;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Delete_Post_By_PostID()
        {
            __mockPostDBAdaptor.Setup(m => m.DeletePost(It.IsAny<int>())).ReturnsAsync(true);
            var res = _postService.DeletePost(It.IsAny<int>()).Result;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Return_All_Post_List()
        {
            List<SocialPost> list = new List<SocialPost>() { new SocialPost() { GroupID = 1, PostTitle = "BBQ", PostDetails = "Xys" } };
            __mockPostDBAdaptor.Setup(m => m.GetAllPosts(It.IsAny<int>())).ReturnsAsync(list);
            var res = _postService.GetAllPosts(It.IsAny<int>()).Result;
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Return_All_Reported_Post_List()
        {
            List<SocialPost> list = new List<SocialPost>() { new SocialPost() { GroupID = 1, PostTitle = "BBQ", PostDetails = "Xys" } };
            __mockPostDBAdaptor.Setup(m => m.GetAllReportedPosts()).ReturnsAsync(list);
            var res = _postService.GetAllReportedPosts().Result;
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Comment_Post_By_PostID()
        {
            __mockPostDBAdaptor.Setup(m => m.CommentPost(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            var res = _postService.CommentPost(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()).Result;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Like_Post_By_PostID()
        {
            __mockPostDBAdaptor.Setup(m => m.LikePost(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            var res = _postService.LikePost(It.IsAny<int>(), It.IsAny<int>()).Result;
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Report_Post_By_PostID()
        {
            __mockPostDBAdaptor.Setup(m => m.ReportPost(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            var res = _postService.ReportPost(It.IsAny<int>(), It.IsAny<int>()).Result;
            Assert.IsTrue(res);
        }
    }
}
