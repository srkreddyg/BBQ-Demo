using BBQN.IdentityManagement.API.Common;
using BBQN.IdentityManagement.API.DataAccess;

namespace BBQN.IdentityManagement.API.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginDBAdaptor _loginadaptor;
        public LoginService(ILoginDBAdaptor loginadaptor)
        {
            _loginadaptor = loginadaptor;
        }
        /// <summary>
        /// To Generate OT
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<int> GenerateOTP(string employeeID , LoginType type)
        {
           return await _loginadaptor.GenerateOTP(employeeID,type);
        }
        /// <summary>
        /// TO Check is User Authenticated
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> IsUserAuthenticated(string employeeID, int otp, LoginType type)
        {
            return await _loginadaptor.IsUserAuthenticated(employeeID,otp ,type);
        }
    }
}
