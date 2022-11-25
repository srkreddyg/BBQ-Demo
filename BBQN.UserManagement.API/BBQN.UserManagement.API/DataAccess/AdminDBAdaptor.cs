using BBQN.DBFactory;
using BBQN.UserManagement.API.Common;
using System.Data;
using BBQN.UserManagement.API.Models;
using System.Text.Json;
using System.Text;

namespace BBQN.UserManagement.API.DataAccess
{
    public class AdminDBAdaptor : IAdminDBAdaptor
    {
        private readonly IDBFactory _dbFactory;
        private IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminDBAdaptor(IDBFactory dBFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbFactory = dBFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// Get Admin by Id
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<List<Users>> GetUser(int UserID)
        {
            IDbDataParameter[] param = new[]
            {
                _dbFactory.CreateParameter(DbType.String, 50, "@UserID", ParameterDirection.Input,UserID),

            };
            Task<List<Users>> users = _dbFactory.Executeprocedure<Users>(this._configuration.GetConnectionString("dbString"), Constants.GetAdminById, param);
            return await users;
        }
        /// <summary>
        /// Fetch all admin records from user table 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Users>> GetAllAdmins()
        {
            IDbDataParameter[] param = new IDbDataParameter[0];
            Task<List<Users>> groups = _dbFactory.Executeprocedure<Users>(this._configuration.GetConnectionString("dbString"), Constants.GetAllAdmins, param);
            return await groups;
        }
        /// <summary>
        /// Add/remove PrivilegeRights To Admin
        /// </summary>
        /// <param name="privilegeIDs"></param>
        /// <param name="userID"></param>
        /// <param name="createdBy"></param>
        /// <param name="lastModifiedBy"></param>
        /// <returns></returns>
        public async Task<int> PrivilegeRightsToAdmin(string privilegeIDs, int userID)
        {
            string id = (string)(_httpContextAccessor.HttpContext?.Items["Authenticated"]);
            int isGroupUpdated = 0;
            IDbDataParameter[] param = new[]
            {
                 _dbFactory.CreateParameter(DbType.String, 50, "@PrivilegeIDsval", ParameterDirection.Input,privilegeIDs),
                 _dbFactory.CreateParameter(DbType.Int32, 50, "@UserIDval", ParameterDirection.Input,userID),
                  _dbFactory.CreateParameter(DbType.String, 50, "@EmployeeIDValue", ParameterDirection.Input,id),
                  _dbFactory.CreateParameter(DbType.Int32, 1, "@flag", ParameterDirection.Output, 0),

            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.PrivilegeRightsToAdmin;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[3].Value != DBNull.Value && param[3].Value != null)
                {
                    isGroupUpdated = Convert.ToInt32(param[3].Value);
                }
                string privilege = Enum.GetName(typeof(UserActionType), isGroupUpdated);
                // TO Audit Log 
                if (isGroupUpdated >= 0)
                {
                    _ = AuditUserPrivileges(userID, privilege, userID.ToString(), id);
                }
            }
            return await Task.FromResult(isGroupUpdated);
        }
        /// <summary>
        /// Add/remove Privilege To User
        /// </summary>
        /// <param name="privilegeIDs"></param>
        /// <param name="userID"></param>
        /// <param name="createdBy"></param>
        /// <param name="lastModifiedBy"></param>
        /// <returns></returns>
        public async Task<bool> PrivilegeUser(int userID, bool isPrivilege)
        {
            string id = (string)(_httpContextAccessor.HttpContext?.Items["Authenticated"]);
            bool isPrivilegeUpdated = false;
            IDbDataParameter[] param = new[]
            {

                 _dbFactory.CreateParameter(DbType.Int32, 50, "@UserIDval", ParameterDirection.Input,userID),
                 _dbFactory.CreateParameter(DbType.Boolean, 1, "@isPrivilege", ParameterDirection.Input,isPrivilege),
                   _dbFactory.CreateParameter(DbType.String, 1, "@EmployeeIDValue", ParameterDirection.Input,id),
                  _dbFactory.CreateParameter(DbType.Boolean, 1, "@flag", ParameterDirection.Output, 0),

            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.PrivilegeToUser;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[3].Value != DBNull.Value && param[3].Value != null)
                {
                    isPrivilegeUpdated = Convert.ToBoolean(param[3].Value);
                }
                // TO Audit Log 
                if (isPrivilegeUpdated)
                {
                    _ = AuditUserPrivileges(userID, UserActionType.PrivilegeAdded.ToString(), userID.ToString(), id);
                }
            }
            return await Task.FromResult(isPrivilegeUpdated);
        }
        /// <summary>
        /// Update Admin data
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="adminModel"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAdmin(Admin users)
        {
            var UserJson = JsonSerializer.Serialize(users);
            bool flag = false;
            IDbDataParameter[] param = new[]
            {
                _dbFactory.CreateParameter(DbType.String, 50, "@AdminUsers", ParameterDirection.Input,UserJson),
               _dbFactory.CreateParameter(DbType.Boolean, 1, "@flag", ParameterDirection.Output, 0),
            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.UpdateAdmin;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[1].Value != DBNull.Value && param[1].Value != null)

                {
                    flag = Convert.ToBoolean(param[1].Value);
                }
            }
            return await Task.FromResult(flag);
        }
        /// <summary>
        /// Method for Auditing of User Operations
        /// </summary>
        /// <param name="grpID"></param>
        /// <param name="action"></param>
        /// <param name="act"></param>
        /// <param name="changedBy"></param>
        /// <returns></returns>
        public bool AuditUserPrivileges(int UserId, string action, string act, string changedBy)
        {

            IDbDataParameter[] param = new[]
              {
                _dbFactory.CreateParameter(DbType.Int32, 100, "@UserIDval", ParameterDirection.Input, UserId),
                  _dbFactory.CreateParameter(DbType.String, 100, "@Actiontype", ParameterDirection.Input, action),
                      _dbFactory.CreateParameter(DbType.String, 1000, "@Act", ParameterDirection.Input, act),
                       _dbFactory.CreateParameter(DbType.Int32, 100, "@EmployeeIDValue", ParameterDirection.Input, changedBy),
            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.AuditUserPrivileges;
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
        /// <summary>
        /// Update Admin data
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="adminModel"></param>
        /// <returns></returns>
        public async Task<bool> BlockUnblockUser(Admin users)
        {
            string id = (string)(_httpContextAccessor.HttpContext?.Items["Authenticated"]);
            var UserJson = JsonSerializer.Serialize(users);
            bool flag = false;
            IDbDataParameter[] param = new[]
            {
                _dbFactory.CreateParameter(DbType.String, 50, "@AdminUsers", ParameterDirection.Input,UserJson),
                  _dbFactory.CreateParameter(DbType.String, 50, "@EmployeeIDValue", ParameterDirection.Input,id),
               _dbFactory.CreateParameter(DbType.Boolean, 1, "@flag", ParameterDirection.Output, 0),
            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.BlokUnblokUser;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[1].Value != DBNull.Value && param[1].Value != null)

                {
                    flag = Convert.ToBoolean(param[1].Value);
                }   
            }
            return await Task.FromResult(flag);
        }

        #region Private Methods
        /// <summary>
        /// Add parameter
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
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
