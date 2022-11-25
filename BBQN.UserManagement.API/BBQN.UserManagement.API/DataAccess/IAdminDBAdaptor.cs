using BBQN.UserManagement.API.Models;
namespace BBQN.UserManagement.API.DataAccess
{
    public interface IAdminDBAdaptor
    {
        /// <summary>
        /// update admin data
        /// </summary>       
        /// <param name="adminModel"></param>
        /// <returns></returns>
        Task<bool> UpdateAdmin(Admin adminModel);
        /// <summary>
        /// get all admins data
        /// </summary>
        /// <returns></returns>
        Task<List<Users>> GetAllAdmins();
       
        /// <summary>
        /// Get Admin detils by id 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        Task<List<Users>> GetUser(int UserID);
        /// <summary>
        /// Add/remove PrivilegeRights To Admin
        /// </summary>
        /// <param name="privilegeIDs"></param>
        /// <param name="userID"></param>
        /// <param name="createdBy"></param>
        /// <param name="lastModifiedB"></param>
        /// <returns></returns>
        Task<int> PrivilegeRightsToAdmin(string privilegeIDs, int userID);

       
        /// <summary>
        /// Add/remove Privilege To User
        /// </summary>
        /// <param name="privilegeIDs"></param>
        /// <param name="userID"></param>
        /// <param name="createdBy"></param>
        /// <param name="lastModifiedB"></param>
        /// <returns></returns>
        Task<bool> PrivilegeUser(int userID, bool isPrivilege);
        /// <summary>
        /// Method Loggin Audit for admin
        /// </summary>
        /// <param name="grpID"></param>
        /// <param name="actType"></param>
        /// <param name="act"></param>
        /// <param name="changedBy"></param>
        /// <returns></returns>
        bool AuditUserPrivileges(int grpID, string actType, string act, string changedBy);

        /// <summary>
        /// update admin data
        /// </summary>      
        /// <param name="adminModel"></param>
        /// <returns></returns>
        Task<bool> BlockUnblockUser(Admin adminModel);
    }
}
