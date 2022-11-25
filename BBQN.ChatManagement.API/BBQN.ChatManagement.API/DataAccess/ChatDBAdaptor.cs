using BBQN.ChatManagement.API.Common;
using BBQN.ChatManagement.API.Models;
using BBQN.DBFactory;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Group = BBQN.ChatManagement.API.Models.Group;

namespace BBQN.ChatManagement.API.DataAccess
{
    /// <summary>
    /// DAL for Chat Group Management
    /// </summary>
    public class ChatDBAdaptor : IChatDBAdaptor
    {
        private readonly IDBFactory _dbFactory;
        private IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatDBAdaptor(IDBFactory dbFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbFactory = dbFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        #region Public Methods
        /// <summary>
        /// Create Group with List of User,GroupDescription
        /// </summary>
        /// <param name="groupUsers"></param>
        /// <returns></returns>
        public async Task<int> CreateGroup(GroupUsers groupUsers)
        {
            var  groupUserJson = JsonSerializer.Serialize(groupUsers);
            int  groupID = 0;
            IDbDataParameter[] param = new[]
            {
                 _dbFactory.CreateParameter(DbType.String, 500, "@GroupUsers", ParameterDirection.Input,groupUserJson),
                  _dbFactory.CreateParameter(DbType.Int32,50, "@GroupIDVal", ParameterDirection.Output, 0)
            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.CreateGroup;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[1].Value != DBNull.Value && param[1].Value != null)
                {
                    groupID = Convert.ToInt32(param[1].Value);
                }
                con.Close();

            }
            // TO Audit Log 
            if (groupID > 0 && groupUsers.userOperations != null)
            {
                CreateAuditLog(groupUsers, groupID, ChatActionType.GroupAdded.ToString());
            }
           return await Task.FromResult(groupID);
       
        }
     

        /// <summary>
        /// To Update Group
        /// </summary>
        /// <param name="groupUsers"></param>
        /// <returns></returns>
        public async Task<bool> UpdateGroup(GroupUsers groupUsers)
        {
            var groupUserJson = JsonSerializer.Serialize(groupUsers);
            bool isGroupUpdated = false;
            IDbDataParameter[] param = new[]
            {
                 _dbFactory.CreateParameter(DbType.String, 50, "@GroupUsers", ParameterDirection.Input,groupUserJson),
                  _dbFactory.CreateParameter(DbType.Boolean, 1, "@flag", ParameterDirection.Output, 0),

            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.UpdateGroup;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[1].Value != DBNull.Value && param[1].Value != null)
                {
                    isGroupUpdated = Convert.ToBoolean(param[1].Value);
                }
                // TO Audit Log 
                if (isGroupUpdated && groupUsers.userOperations != null)
                {
                    int groupID =(int) groupUsers.Group?.GroupId;
                    CreateAuditLog(groupUsers, groupID, ChatActionType.GroupUpdated.ToString());
                }

            }
            return await Task.FromResult(isGroupUpdated);
        }
        /// <summary>
        /// TO Delete a Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<bool>  DeleteGroup(int groupId)
        {
            string id = (string)(_httpContextAccessor.HttpContext?.Items["Authenticated"]);
            bool isGroupDeleted = false;
            IDbDataParameter[] param = new[]
            {
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@GroupIDValue", ParameterDirection.Input,groupId),
                      _dbFactory.CreateParameter(DbType.Boolean, 1, "@flag", ParameterDirection.Output, 0),
            };

            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.DeleteGroup;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[1].Value != DBNull.Value && param[1].Value != null)
                {
                    isGroupDeleted = Convert.ToBoolean(param[1].Value);
                }
                con.Close();

              

            }
            // TO Audit Log 
            if (isGroupDeleted)
            {
                _ = AuditGroup(groupId, ChatActionType.GroupDeleted.ToString(), groupId.ToString(), id);
            }
            return await  Task.FromResult(isGroupDeleted);
        }
        /// <summary>
        /// To Remove a User From a Specific Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveUserFromGroup(int groupId, int userId)
        {
            string id = (string)(_httpContextAccessor.HttpContext?.Items["Authenticated"]);
            bool IsDeleted = false;
            IDbDataParameter[] param = new[]
            {
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@GroupIDValue", ParameterDirection.Input,groupId),
                   _dbFactory.CreateParameter(DbType.Int32, 50, "@UserIDValue", ParameterDirection.Input,userId),
                  _dbFactory.CreateParameter(DbType.Boolean, 1, "@IsDeleted", ParameterDirection.Output, IsDeleted),
            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.RemoveUserGroup;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                //await Task.Run(() => 
                cmd.ExecuteNonQuery();
                if (param[2].Value != DBNull.Value && param[2].Value != null)
                {
                    IsDeleted = Convert.ToBoolean(param[2].Value);
                }
                con.Close();
            }
            // TO Audit Log 
            if (IsDeleted)
            {
                _ = AuditGroup(groupId, ChatActionType.UserDeleted.ToString(), userId.ToString(), id);
            }
            return await Task.FromResult(IsDeleted);
        }

        /// <summary>
        /// Return all exsting users 
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAllUsers( GetUserFilter getUserFilter)
        {
            var getFilterJson = JsonSerializer.Serialize(getUserFilter);
            FilterConditions? filterCondition =     getUserFilter.filterCondition;
            int filterOperator = 2;
            string? startDate = null;
            string? endDate = null;
            if (filterCondition != null)
            {
                filterOperator = (int)filterCondition.Operator;
                startDate = filterCondition.StartDate?.ToString("yyyy-MM-dd");
                
                endDate = filterCondition.EndDate?.ToString("yyyy-MM-dd");
            }

            IDbDataParameter[] param = new[]
            {
                 _dbFactory.CreateParameter(DbType.String, 1000, "@GetUserFilter", ParameterDirection.Input,getFilterJson),
                 _dbFactory.CreateParameter(DbType.Int32, 10, "@FilterOperator", ParameterDirection.Input,(int)filterOperator),
                   _dbFactory.CreateParameter(DbType.String, 1000, "@StartDate", ParameterDirection.Input,startDate==null? String.Empty: startDate),
                   _dbFactory.CreateParameter(DbType.String, 1000, "@EndDate", ParameterDirection.Input,endDate== null? string.Empty:endDate),

              };
            Task<List<User>> users = _dbFactory.Executeprocedure<User>(this._configuration.GetConnectionString("dbString"), Constants.GetUsers, param);
            return await users;
        }


        /// <summary>
        /// Return all exsting Groups
        /// </summary>
        /// <returns></returns>
        public async Task<List<Group>> GetAllGroups()
        {

            IDbDataParameter[] param = new IDbDataParameter[0];
            Task<List<Group>> groups = _dbFactory.Executeprocedure<Group>(this._configuration.GetConnectionString("dbString"), Constants.GetGroups, param);
            return await groups;
        }

        /// <summary>
        /// Return all groups where the user is part of the groups
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<List<Group>> GetAllGroups(int UserId)
        {
            IDbDataParameter[] param = new[]
            {
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@UserIdValue", ParameterDirection.Input,UserId),
            };
            Task<List<Group>> groups = _dbFactory.Executeprocedure<Group>(this._configuration.GetConnectionString("dbString"), Constants.GetGroupsByUser, param);
            return await groups;
        }
        /// <summary>
        /// Return all users from a Specific Group
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public async Task<List<User>> GetAllUser(int GroupId)
        {
            IDbDataParameter[] param = new[]
            {
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@GroupIDValue", ParameterDirection.Input,GroupId),

            };
            Task<List<User>> users = _dbFactory.Executeprocedure<User>(this._configuration.GetConnectionString("dbString"), Constants.GetGroupUsersList, param);
            return await users;
        }
        /// <summary>
        /// Block a User in a Specific Grouop 
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>

        public async Task<bool> BlockUser(int goupId, int userId)
        {
            bool IsBlocked = false;
            IDbDataParameter[] param = new[]
            {
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@GroupIDValue", ParameterDirection.Input,goupId),
                   _dbFactory.CreateParameter(DbType.Int32, 50, "@UserIDValue", ParameterDirection.Input,userId),
                  _dbFactory.CreateParameter(DbType.Boolean, 1, "@Isblocked", ParameterDirection.Output, IsBlocked),
            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.BlockUser;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                // await Task.Run(() =>
                cmd.ExecuteNonQuery();
                if (param[1].Value != DBNull.Value && param[1].Value != null)
                {
                    IsBlocked = Convert.ToBoolean(param[1].Value);
                }
                con.Close();
            }
            return await Task.FromResult(IsBlocked);
        }
        /// <summary>
        /// Method for Auditing of Chat Operations
        /// </summary>
        /// <param name="grpID"></param>
        /// <param name="action"></param>
        /// <param name="act"></param>
        /// <param name="changedBy"></param>
        /// <returns></returns>
        public  bool  AuditGroup(int grpID, string action, string act, string changedBy) {

            IDbDataParameter[] param = new[]
              {
                _dbFactory.CreateParameter(DbType.Int32, 100, "@GrpID", ParameterDirection.Input, grpID),
                  _dbFactory.CreateParameter(DbType.String, 100, "@ActType", ParameterDirection.Input, action),
                      _dbFactory.CreateParameter(DbType.String, 1000, "@Act", ParameterDirection.Input, act),
                       _dbFactory.CreateParameter(DbType.Int32, 100, "@EmployeeIDValue", ParameterDirection.Input, changedBy),
            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.AuditGroup;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                con.Close();
               }
            return true;

        }
        #endregion

        #region Private Methods
        private void AddParameter(IDbCommand cmd, IDbDataParameter[] param)
        {
            foreach (var p in param)
            {
                cmd.Parameters.Add(p);
            }
        }
        /// <summary>
        /// Audit Log
        /// </summary>
        /// <param name="groupUsers"></param>
        /// <param name="groupID"></param>
        private void CreateAuditLog(GroupUsers groupUsers, int groupID ,string actionType)
        {
            StringBuilder sb = new StringBuilder();
            string createdBy = groupUsers?.Group?.CreatedBy.ToString();
            groupUsers.userOperations.ForEach(x =>
                sb.AppendLine(x.user?.FirstName?.ToString() + ":" + x.action.ToString() + ","));

            bool flag = AuditGroup(groupID, actionType, sb.ToString(), createdBy);
        }
        #endregion


    }
}
