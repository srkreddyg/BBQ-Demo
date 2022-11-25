using BBQN.DBFactory;
using BBQN.UserManagement.API.DataAccess;
using BBQN.UserManagement.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBQN.UserManagement.API.Test.DataAccess
{
    [TestClass]
    public class AdminDBAdaptorTests
    {
        private Mock<IDBFactory> _dbFactory = null;
        private Mock<IConfiguration> _configuration = null;
        private Mock<IHttpContextAccessor> _httpContextAccessor = null;
        private Mock<IDbConnection> _dbConnection = null;
        private Mock<IDbDataAdapter> _dbDataAdapter = null;
        private Mock<IDbCommand> _dbCommand = null;

        private AdminDBAdaptor target = null;

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
            target = new AdminDBAdaptor(_dbFactory.Object, _configuration.Object, _httpContextAccessor.Object);
        }

        [TestMethod]
        public async Task Update_Admin_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.UpdateAdmin(It.IsAny<Admin>());
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task Should_Return__Admin_list_DBAdaptorAsync()
        {
            List<Users> users = new List<Users>() { new Users { UserID = 5, FirstName = "xyz" } };
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _dbFactory.Setup(x => x.Executeprocedure<Users>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>())).ReturnsAsync(users);

            var res = await target.GetAllAdmins();
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task Should_Return_User_DBAdaptorAsync()
        {
            List<Users> users = new List<Users>() { new Users { UserID = 5, FirstName = "xyz" } };
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _dbFactory.Setup(x => x.Executeprocedure<Users>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>())).ReturnsAsync(users);

            var res = await target.GetUser(It.IsAny<int>());
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task Should_Add_or_Remove_PrivilegeRights_to_Admin_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);

            int res = await target.PrivilegeRightsToAdmin(It.IsAny<string>(),It.IsAny<int>());
            Assert.IsNotNull(res);
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public async Task Blok_Unblok_User_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.BlockUnblockUser(It.IsAny<Admin>());
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }
    }
}
