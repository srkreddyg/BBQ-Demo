using BBQN.AutoEnrolled.API.DataAccess;
using BBQN.AutoEnrolled.API.Models;
using BBQN.DBFactory;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BBQN.AutoEntrolled.API.Test.DataAccess
{
    [TestClass]
    public class AutoEntrollDBAdaptorTests
    {
        private Mock<IDBFactory> _dbFactory = null;
        private Mock<IConfiguration> _configuration = null;
        private Mock<IHttpContextAccessor> _httpContextAccessor = null;
        private Mock<IDbDataParameter> _dbDataParams = null;
        private Mock<IDbConnection> _dbConnection = null;
        private Mock<IDbDataAdapter> _dbDataAdapter = null;
        private Mock<IDbCommand> _dbCommand = null;
        private AutoEnrollDBAdaptor target = null;

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
            target = new AutoEnrollDBAdaptor(_dbFactory.Object, _configuration.Object, _httpContextAccessor.Object);
        }

        [TestMethod]
        public async Task Should_Return_Group_List_DBAdaptorAsync()
        {
            List<Group> list = new List<Group>() { new Group() { GroupId = 1, GradeName = "BBQ", GroupDescription = "Xys" } };
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _dbFactory.Setup(x => x.Executeprocedure<Group>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>())).ReturnsAsync(list);
            GetUserFilter getUserFilter = new GetUserFilter() { GradeID = 1, OutletID = 1 };

            var res = await target.GetMatchedGroupIds(getUserFilter);
            Assert.IsNotNull(res);
        }
        [TestMethod]
        public async Task Remove_User_AutoEntroll_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.RemoveUserAutoEnroll(It.IsAny<int>());
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }
        [TestMethod]
        public async Task Create_User_AutoEntroll_DBAdaptorAsync()
        {
            User user = new User() { UserID = 1, UserLoginID = "1" };
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.AddAutoEnrollUser(It.IsAny<int>(), user);
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }


    }
}
