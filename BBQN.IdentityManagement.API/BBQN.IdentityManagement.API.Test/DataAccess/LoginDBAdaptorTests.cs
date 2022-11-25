using BBQN.IdentityManagement.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using BBQN.DBFactory;
using Moq;
using BBQN.IdentityManagement.API.DataAccess;
using BBQN.IdentityManagement.API.Common;
using System.Data.Common;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BBQN.IdentityManagement.API.Test.DataAccess
{
    [TestClass]
    public class LoginDBAdaptorTests
    {
        private Mock<IDBFactory> _dbFactory = null;
        private Mock<IConfiguration> _configuration = null;
        private Mock<IHttpContextAccessor> _httpContextAccessor = null;
        private Mock<IDbDataParameter> _dbDataParams = null;
        private Mock<IDbConnection> _dbConnection = null;
        private Mock<IDbDataAdapter> _dbDataAdapter = null;
        private Mock<IDbCommand> _dbCommand = null;
        private Mock<ISMSService> _mockSMSService = null;

        private LoginDbAdaptor target = null;
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
            _mockSMSService = new Mock<ISMSService>();
            target = new LoginDbAdaptor(_dbFactory.Object, _configuration.Object, _mockSMSService.Object);
        }

        [TestMethod]
        public async Task GenerateOTP_Return_Int_Value()
        {

            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _mockSMSService.Setup(m => m.SendSMS(It.IsAny<int>(), It.IsAny<long>())).Returns(true);
            int b = await target.GenerateOTP(It.IsAny<string>(),It.IsAny<LoginType>());
            Assert.IsNotNull(b);
            Assert.IsTrue(b > 0);

        }

        [TestMethod]
        public async Task Check_User_Is_Authenticated_Or_Not()
        {

            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.IsUserAuthenticated(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<LoginType>());
            Assert.IsNotNull(res);
            Assert.IsTrue(res);

        }

    }
}
