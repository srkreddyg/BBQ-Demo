using BBQN.ChatManagement.API.Common;
using BBQN.ChatManagement.API.DataAccess;
using BBQN.ChatManagement.API.Models;
using BBQN.DBFactory;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Group = BBQN.ChatManagement.API.Models.Group;

namespace BBQN.ChatManagement.API.Test.DataAccess
{
    [TestClass()]
    public class ChatDBAdaptorTests
    {
        private Mock<IDBFactory> _dbFactory = null;
        private Mock<IConfiguration> _configuration = null;
        private Mock<IHttpContextAccessor> _httpContextAccessor = null;
        private Mock<IDbDataParameter> _dbDataParams = null;
        private Mock<IDbConnection> _dbConnection = null;
        private Mock<IDbDataAdapter> _dbDataAdapter = null;
        private Mock<IDbCommand> _dbCommand = null;


        private ChatDBAdaptor target = null;
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
            target = new ChatDBAdaptor(_dbFactory.Object, _configuration.Object, _httpContextAccessor.Object);
        }

        [TestMethod]
        public async Task Create_Chat_Group_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            int b = await target.CreateGroup(GetGroupUsers());
            Assert.IsNotNull(b);
            Assert.IsTrue(b > 0);
        }

        [TestMethod]
        public async Task Update_Chat_Group_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.UpdateGroup(GetGroupUsers());
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task Delete_Chat_Group_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.DeleteGroup(It.IsAny<int>());
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task Remove_User_From_Group_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>());
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task Block_User_DBAdaptorAsync()
        {
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            var res = await target.BlockUser(It.IsAny<int>(), It.IsAny<int>());
            Assert.IsNotNull(res);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task Should_Return_User_List_DBAdaptorAsync()
        {
            List<User> users = new List<User>() { new User { UserID=5, FirstName = "xyz"} };
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _dbFactory.Setup(x => x.Executeprocedure<User>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>())).ReturnsAsync(users);

            var res = await target.GetAllUser(It.IsAny<int>());
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task Should_Return_Group_List_FilteredBy_GroupID_DBAdaptorAsync()
        {
            List<Group> groups = new List<Group>() { new Group { GroupId = 1, GroupName = "xyz" } };
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _dbFactory.Setup(x => x.Executeprocedure<Group>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>())).ReturnsAsync(groups);
            var res = await target.GetAllGroups(It.IsAny<int>());
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task Should_Return_All_Group_List_DBAdaptorAsync()
        {
            List<Group> groups = new List<Group>() { new Group { GroupId = 1, GroupName = "xyz" } };
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _dbFactory.Setup(x => x.Executeprocedure<Group>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>())).ReturnsAsync(groups);
            var res = await target.GetAllGroups();
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task Should_Return_User_List_FilteredByConditions_DBAdaptorAsync()
        {
            GetUserFilter filter = new GetUserFilter();
            filter.filterCondition = new FilterConditions() { StartDate = System.DateTime.Now, EndDate = System.DateTime.Now };

            List<User> users = new List<User>() { new User { UserID = 5, FirstName = "xyz" } };
            Mock<IDbDataParameter> groupReferenceParam = new Mock<IDbDataParameter>();
            groupReferenceParam.Setup(x => x.Value).Returns(1);
            _dbFactory.Setup(x => x.CreateParameter(It.IsAny<DbType>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ParameterDirection>(), It.IsAny<object>())).Returns(groupReferenceParam.Object);
            _dbCommand.Setup(x => x.Parameters.Add(It.IsAny<object>)).Returns(1);
            _dbFactory.Setup(x => x.Executeprocedure<User>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDbDataParameter[]>())).ReturnsAsync(users);

            var res = await target.GetAllUsers(filter);
            Assert.IsNotNull(res);
        }

        private GroupUsers GetGroupUsers()
        {

            Group grp = new Group()
            {
                GroupName = "TestGRP",
                GradeName = "TestGrade"

            };
            UserOperations usOp = new UserOperations()
            {
                action = ActionType.Added,
                user = new User()
                {
                    UserID = 1,
                    FirstName = "TestUser"
                }

            };
            List<UserOperations> usops = new List<UserOperations>()
            {
                usOp
            };

            GroupUsers grpUsers = new GroupUsers()
            {
                Group = grp,
                userOperations = usops
            };
            return grpUsers;
        }
    }
}
