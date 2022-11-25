using BBQN.IdentityManagement.API.Common;

namespace BBQN.IdentityManagement.API.Services
{
    public interface ILoginService
    {
        /// <summary>
        /// Service Method to Generate OTP
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<int> GenerateOTP(string employeeID, LoginType type);
        
      /// <summary>
      /// Authenticating User
      /// </summary>
      /// <param name="employeeID"></param>
      /// <param name="otp"></param>
      /// <returns></returns>
        Task<bool> IsUserAuthenticated(string employeeID, int otp, LoginType type);
    }
}
