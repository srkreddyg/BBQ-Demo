namespace BBQN.Master.API.Common
{
    public class Constants
    {
        /// <summary>
        /// Master Data sp's
        /// </summary>
        public static readonly string GetAllPrivileges = "BBQN.GetAllPrivileges";
        public static readonly string GetMasterData = "BBQN.GetMasterData";
        public static readonly string GetSubDepartmentsMasterData = "BBQN.GetSubDepartmentsMasterData";
        public static readonly string GetOutletsMasterData = "BBQN.GetOutletsMasterData";

        /// <summary>
        /// connection string 
        /// </summary>
        // public static readonly string constring = "Server=HMECL004896\\SQLEXPRESS01;DataBase=BBQN;Trusted_Connection=true;MultipleActiveResultSets=true";
        public static readonly string constring = "server=localhost;port=3306;database=bbqn;user=root;Password=Winter@3638";
    }
}
