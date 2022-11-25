using Castle.Core.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BBQN.DBFactory.Test
{
    [TestClass]
    public class DBFactoryTests
    {
        private Mock<IDBFactory> _dbFactory = null;
        private Mock<IConfiguration> _configuration = null;
        private Mock<IDbDataParameter> _dbDataParams = null;
        private Mock<IDbConnection> _dbConnection = null;
        private Mock<IDbDataAdapter> _dbDataAdapter = null;
        private Mock<IDbCommand> _dbCommand = null;

        private DBFactory target = null;

        [TestInitialize]
        public void Initialize()
        {
            _configuration = new Mock<IConfiguration>();
            _dbCommand = new Mock<IDbCommand>();
            _dbCommand.Setup(c => c.ExecuteNonQuery()).Returns(1);
            _dbDataAdapter = new Mock<IDbDataAdapter>();
            _dbConnection = new Mock<IDbConnection>();
            _dbConnection.Setup(x => x.CreateCommand()).Returns(_dbCommand.Object);
            _dbFactory = new Mock<IDBFactory>();
            _dbFactory.Setup(x => x.GetDbConnection(It.IsAny<string>())).Returns(_dbConnection.Object);

            _dbFactory.Setup(x => x.GetDbDataAdapter()).Returns(_dbDataAdapter.Object);


            target = new DBFactory();
        }

        [TestMethod]
        public void Create_Parameter()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res= target.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>());
            Assert.IsNotNull(res);
        }

        //[TestMethod]
        //public void Execute_Procedure_DBAdaptorAsync()
        //{
        //    Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
        //    groupReferenceParam.Setup(x => x.Value).Returns(1);
        //    _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
        //    _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
        //   var res =  target.Executeprocedure(It.IsAny<string>(),It.IsAny<string>(), It.IsAny<IDbDataParameter[]>());
        //   Assert.IsNotNull(res);
        //}

        [TestMethod]
        public async Task Execute_Procedure_DBAdaptorAsyncList()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = target.Executeprocedure<It.IsAnyType>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>());
            Assert.IsNotNull(res);
        }
    }
}
