using BBQN.UserManagement.API.Models;

namespace BBQN.UserManagement.API.Services
{
    public interface IAdminService
    {

        /// <summary>
        /// update admin details 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="adminModel"></param>
        /// <returns></returns>
        Task<bool> UpdateAdmin(Admin admin);
        /// <summary>
        /// Get all users who all are admins
        /// </summary>
        /// <returns></returns>
        Task<List<Users>> GetAllAdmins();
      
        /// <summary>
        /// Get admins details by id
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
        Task<bool> PrivilegeUser(int userID,bool isPrivilege);
        /// <summary>
        /// Blok or Unblok User
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="adminModel"></param>
        /// <returns></returns>
        Task<bool> BlockUnblockUser(Admin admin);
    }
}
