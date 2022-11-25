using BBQN.AutoEnrolled.API.Common;
using BBQN.AutoEnrolled.API.Models;
using BBQN.DBFactory;
using System.Data;
using System.Text.Json;
using Group = BBQN.AutoEnrolled.API.Models.Group;

namespace BBQN.AutoEnrolled.API.DataAccess
{
    public class AutoEnrollDBAdaptor : IAutoEnrollDbAdaptor
    {
     
        private readonly IDBFactory _dbFactory;
        private IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AutoEnrollDBAdaptor(IDBFactory dbFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbFactory = dbFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// Add new employee
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> AddAutoEnrollUser(int groupId, User user)
        {
          //  string id = (string)(_httpContextAccessor.HttpContext?.Items["Authenticated"]);
            bool result = false;
            var userJson = JsonSerializer.Serialize(user);
            IDbDataParameter[] param = new[]
            {
                 _dbFactory.CreateParameter(DbType.String, 500, "@CreateUsers ", ParameterDirection.Input,userJson),
                 _dbFactory.CreateParameter(DbType.Int32, 1, "@GroupID", ParameterDirection.Input, groupId),                 
                  _dbFactory.CreateParameter(DbType.Boolean, 1, "@flag", ParameterDirection.Output, 0),
            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.CreateUser;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[2].Value != DBNull.Value && param[2].Value != null)
                {
                    result = Convert.ToBoolean(param[2].Value);
                }
                con.Close();

            }
            // TO Audit Log 
            if (result)
            {
                _ = BGServiceAuditLog(user.UserID, ActionType.Added.ToString(),"New Employee Added", "1");
            }

            return await Task.FromResult(true); 
        }

        /// <summary>
        /// Remove the employee who left organization
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<bool> RemoveUserAutoEnroll(int userID)
        {
              bool isUserRemoved = false;
            IDbDataParameter[] param = new[]
            {
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@UserID", ParameterDirection.Input,userID),
                      _dbFactory.CreateParameter(DbType.Boolean, 1, "@isUserRemoved", ParameterDirection.Output,0),
            };

            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.RemoveExistUser;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[1].Value != DBNull.Value && param[1].Value != null)
                {
                    isUserRemoved = Convert.ToBoolean(param[1].Value);
                }
                con.Close();


            }
            // TO Audit Log 
            if (isUserRemoved)
            {
                _ = BGServiceAuditLog(userID, ActionType.Added.ToString(), "Employee left", "1");
            }

            return await Task.FromResult(isUserRemoved);
        }

        /// <summary>
        /// Get criteria matched groups
        /// </summary>
        /// <param name="getUserFilter"></param>
        /// <returns></returns>
        public async Task<List<Group>> GetMatchedGroupIds(GetUserFilter getUserFilter)
        {
            var getFilterJson = JsonSerializer.Serialize(getUserFilter);

            IDbDataParameter[] param = new[]
            {
                 _dbFactory.CreateParameter(DbType.String, 1000, "@GetUserFilter", ParameterDirection.Input,getFilterJson),

              };
            Task<List<Group>> users = _dbFactory.Executeprocedure<Group>(this._configuration.GetConnectionString("dbString"), Constants.GetMatchedGroupIds, param);
            return await users;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grpID"></param>
        /// <param name="action"></param>
        /// <param name="act"></param>
        /// <param name="changedBy"></param>
        /// <returns></returns>
        public bool BGServiceAuditLog(int grpID, string action, string act, string changedBy)
        {

            IDbDataParameter[] param = new[]
              {
                _dbFactory.CreateParameter(DbType.Int32, 100, "@UserIDval", ParameterDirection.Input, grpID),
                  _dbFactory.CreateParameter(DbType.String, 100, "@Actiontype", ParameterDirection.Input, action),
                      _dbFactory.CreateParameter(DbType.String, 1000, "@Act", ParameterDirection.Input, act),
                       _dbFactory.CreateParameter(DbType.Int32, 100, "@EmployeeIDValue", ParameterDirection.Input, changedBy),
            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.BGServiceAuditLog;
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

        
        #region private 
        private void AddParameter(IDbCommand cmd, IDbDataParameter[] param)
        {
            foreach (var p in param)
            {
                cmd.Parameters.Add(p);
            }
        }
        #endregion
    }
}
