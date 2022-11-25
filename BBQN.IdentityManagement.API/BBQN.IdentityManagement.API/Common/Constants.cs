using System.Reflection.Metadata;

namespace BBQN.IdentityManagement.API.Common
{
    public static class Constants
    {
        public static readonly  string GenerateOTP = "BBQN.GenerateOTP";
        public static readonly string ValidateUser = "BBQN.ValidateUser";
        public static readonly string AuthKey = "383674AfrxB3MR63493e78P1";
        public static readonly string SenderID = "BARBEQ";
        public static readonly string RouteID = "4";
        public static readonly string CountryID = "91";
        public static readonly string DLT_TE_ID = "1607100000000061794";

    }
    public enum LoginType
    {
        Admin=1,
        NonAdmin=2,
        MobileApp=3
    }
}
