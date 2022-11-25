using BBQN.DBFactory;
using BBQN.Master.API.DataAccess;
using BBQN.Master.API.Models;
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

namespace BBQN.Master.API.Test.DataAccess
{
    [TestClass]
    public class MasterDBAdaptorTests
    {
        private Mock<IDBFactory> _dbFactory = null;
        private Mock<IConfiguration> _configuration = null;
        private Mock<IHttpContextAccessor> _httpContextAccessor = null;
        private Mock<IDbDataParameter> _dbDataParams = null;
        private Mock<IDbConnection> _dbConnection = null;
        private Mock<IDbDataAdapter> _dbDataAdapter = null;
        private Mock<IDbCommand> _dbCommand = null;

        private MasterDBAdaptor target = null;

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
            target = new MasterDBAdaptor(_dbFactory.Object, _configuration.Object);
        }

        [TestMethod]
        public async Task Should_Return_SubDepartment_List_DBAdaptorAsync()
        {
            List<SubDepartments> subDepartments = new List<SubDepartments>() { new SubDepartments() { SubDepartmentID = 1, SubDepartmentName = "test" } };
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _dbFactory.Setup(x => x.Executeprocedure<SubDepartments>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>())).ReturnsAsync(subDepartments);

            var res =  target.GetDepartments(It.IsAny<int>()).Result;
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task Should_Return__OutletsList_DBAdaptorAsync()
        {
            List<Outlets> subDepartments = new List<Outlets>() { new Outlets() { OutletID = 1, OutletName = "test" } };
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _dbFactory.Setup(x => x.Executeprocedure<Outlets>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>())).ReturnsAsync(subDepartments);

            var res =  target.GetOutlets(It.IsAny<int>()).Result;
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task Should_Return__Privileges_List_DBAdaptorAsync()
        {
            List<Privileges> users = new List<Privileges>() { new Privileges { PrivilegeID = 5, Description = "xyz" } };
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _dbFactory.Setup(x => x.Executeprocedure<Privileges>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>())).ReturnsAsync(users);

            var res = await target.GetAllPrivileges();
            Assert.IsNotNull(res);
        }
    }
}
