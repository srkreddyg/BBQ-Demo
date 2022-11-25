using BBQN.DBFactory;
using BBQN.IdentityManagement.API.Common;
using BBQN.IdentityManagement.API.Services;
using System.Data;

namespace BBQN.IdentityManagement.API.DataAccess
{
    public class LoginDbAdaptor : ILoginDBAdaptor
    {
        private readonly IDBFactory _dbFactory;
        private readonly IConfiguration _configuration;
        private readonly ISMSService _smsService;



        public LoginDbAdaptor(IDBFactory dbFactory, IConfiguration configuration, ISMSService smsService)
        {
            _dbFactory = dbFactory;
            _configuration = configuration;
            _smsService = smsService;

        }
        /// <summary>
        /// To Generate OTP and Saving
        /// </summary>
        /// <param name="employeeID"></param>
       /// <param name="type"></param>
        /// <returns></returns>
        public async Task<int> GenerateOTP(string employeeID, LoginType type)
        {
            int otpGenerated = 0;
            long mobileNumber = 0;
            IDbDataParameter[] param = new[]
            {
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@EmployeeID", ParameterDirection.Input, employeeID),
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@LType", ParameterDirection.Input,(int) type),
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@OTPGenerated", ParameterDirection.Output, 0),
                   _dbFactory.CreateParameter(DbType.Int64, 50, "@MobileNumberout", ParameterDirection.Output, 0),
            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.GenerateOTP;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                await Task.Run(() => cmd.ExecuteNonQuery());
                if (param[2].Value != DBNull.Value && param[2].Value != null)
                {
                    otpGenerated = Convert.ToInt32(param[2].Value);
                }
                if (param[3].Value != DBNull.Value && param[3].Value != null)
                {
                    mobileNumber = Convert.ToInt64(param[3].Value);
                }
                con.Close();
            }
            //Call SMS Sending API
            // if Successfully Sent then OTP therwise No OTP
            if (mobileNumber == 0) return 0;
            if (_smsService.SendSMS(otpGenerated, mobileNumber))
            {
                return otpGenerated;
            }
             return 0;

        }
     /// <summary>
     /// TO Check Is User Authenticated
     /// </summary>
     /// <param name="employeeID"></param>
     /// <param name="otp"></param>
     /// <returns></returns>
        public async Task<bool> IsUserAuthenticated(string employeeID, int otp, LoginType type)
        {
            bool isUserAuthenticated = false;
            IDbDataParameter[] param = new[]
            {
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@EmployeeID", ParameterDirection.Input,employeeID),
                   _dbFactory.CreateParameter(DbType.Int32, 50, "@OTPValue", ParameterDirection.Input,otp),
                   _dbFactory.CreateParameter(DbType.Int32, 50, "@LType", ParameterDirection.Input,(int) type),
                  _dbFactory.CreateParameter(DbType.Boolean, 1, "@IsUserAuthenticated", ParameterDirection.Output, 0),
            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.ValidateUser;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[3].Value != DBNull.Value && param[3].Value != null)
                {
                    isUserAuthenticated = Convert.ToBoolean(param[3].Value);
                }
                con.Close();
            }
            return isUserAuthenticated;
        }

        #region Private Methods
        /// <summary>
        /// Execute procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
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
