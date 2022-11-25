namespace BBQN.IdentityManagement.API.Services
{
    /// <summary>
    /// Sending SMS
    /// </summary>
    public interface ISMSService
    {
        /// <summary>
        /// Send SMS 
        /// </summary>
        /// <param name="otp"></param>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
       bool  SendSMS(int otp, long mobileNumber);
    }
}
