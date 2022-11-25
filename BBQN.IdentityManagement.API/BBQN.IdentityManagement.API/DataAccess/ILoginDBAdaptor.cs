using BBQN.IdentityManagement.API.Common;

namespace BBQN.IdentityManagement.API.DataAccess
{
    public interface ILoginDBAdaptor
    {
        /// <summary>
        /// Data Base OTP Generation and Saving
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<int> GenerateOTP(string EmployeeID, LoginType type);
       
        /// <summary>
        /// TO Verify User OTP Validity 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        Task<bool> IsUserAuthenticated(string employeeID, int otp, LoginType type);
    }
}
