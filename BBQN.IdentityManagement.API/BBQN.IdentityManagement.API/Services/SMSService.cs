using BBQN.IdentityManagement.API.Common;
using System.Net;
using System.Text;
using System.Web;
using static System.Net.WebRequestMethods;

namespace BBQN.IdentityManagement.API.Services
{
    public class SMSService : ISMSService
    {
        /// <summary>
        /// SMS Sending Service
        /// </summary>
        /// <param name="otp"></param>
        /// <param name="mobNumber"></param>
        /// <returns></returns>
        public bool SendSMS(int otp, long mobNumber)
        {
            //Multiple mobiles numbers separated by comma
            string mobileNumber = mobNumber.ToString();
           string message = HttpUtility.UrlEncode($"Your%20Saathi%20App%20login%20OTP%20is {otp}%20Regards%2C%20BBQ%20Nation");
            //"Your Saathi App login OTP is {otp} Regards  BBQ Nation");

            //Prepare you post parameters
            StringBuilder sbPostData = new StringBuilder();
            sbPostData.AppendFormat("authkey={0}", Constants.AuthKey);
            sbPostData.AppendFormat("&mobiles={0}", mobileNumber);
            sbPostData.AppendFormat("&message={0}", message);
            sbPostData.AppendFormat("&sender={0}", Constants.SenderID);
            sbPostData.AppendFormat("&route={0}", Constants.RouteID);
           sbPostData.AppendFormat("&country={0}", Constants.CountryID);
            sbPostData.AppendFormat("&DLT_TE_ID={0}", Constants.DLT_TE_ID);
            try
            {
                //Call Send SMS API
                string sendSMSUri = "http://smsp.myoperator.co/api/sendhttp.php";
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                //Close the response
                reader.Close();
                response.Close();
                return true;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }
    }
}
