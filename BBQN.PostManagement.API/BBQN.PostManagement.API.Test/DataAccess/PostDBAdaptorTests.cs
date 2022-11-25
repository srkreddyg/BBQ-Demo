using BBQN.DBFactory;
using BBQN.PostManagement.API.DataAccess;
using BBQN.PostManagement.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BBQN.PostManagement.API.Test.DataAccess
{
    [TestClass]
    public class PostDBAdaptorTests
    {
        private Mock<IDBFactory> _dbFactory = null;
        private Mock<IConfiguration> _configuration = null;
        private Mock<IHttpContextAccessor> _httpContextAccessor = null;
        private Mock<IDbDataParameter> _dbDataParams = null;
        private Mock<IDbConnection> _dbConnection = null;
        private Mock<IDbDataAdapter> _dbDataAdapter = null;
        private Mock<IDbCommand> _dbCommand = null;
        private PostDBAdaptor target = null;
        SocialPost post = new SocialPost() { GroupID = 1, PostDetails = "test", PostTitle = "test" };
        List<SocialPost> list = new List<SocialPost>() { new SocialPost() { GroupID = 1, PostTitle = "BBQ", PostDetails = "Xys" } };

        [TestInitialize]
        public void Initialize()
        {
            _configuration = new Mock<IConfiguration>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _dbCommand = new Mock<IDbCommand>();
            _dbCommand.Setup(c => c.ExecuteNonQuery()).Returns(1);
            _dbDataAdapter = new Mock<IDbDataAdapter>();
            _dbConnection = new Mock<IDbConnection>();
            _dbConnection.Setup(x => x.CreateCommand()).Returns(_dbCommand.Object);
            _dbFactory = new Mock<IDBFactory>();
            _dbFactory.Setup(x => x.GetDbConnection(It.IsAny<string>())).Returns(_dbConnection.Object);

            _dbFactory.Setup(x => x.GetDbDataAdapter()).Returns(_dbDataAdapter.Object);


            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            target = new PostDBAdaptor(_dbFactory.Object, _configuration.Object, _httpContextAccessor.Object);
        }

        [TestMethod]
        public async Task Create_Post_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            int b = await target.CreatePost(post);
            Assert.IsNotNull(b);
            Assert.IsTrue(b > 0);
        }

        [TestMethod]
        public async Task Update_Post_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.UpdatePost(post);
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task Delete_Post_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.DeletePost(It.IsAny<int>());
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task Should_Return_Post_List_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _dbFactory.Setup(x => x.Executeprocedure<SocialPost>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>())).ReturnsAsync(list);

            var res = await target.GetAllPosts(It.IsAny<int>());
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task Should_Return_Reported_Post_List_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _dbFactory.Setup(x => x.Executeprocedure<SocialPost>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>())).ReturnsAsync(list);

            var res = await target.GetAllReportedPosts();
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task Comment_Post_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.CommentPost(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>());
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task Like_Post_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.LikePost(It.IsAny<int>(), It.IsAny<int>());
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task Report_Post_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.ReportPost(It.IsAny<int>(), It.IsAny<int>());
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

    }
}
