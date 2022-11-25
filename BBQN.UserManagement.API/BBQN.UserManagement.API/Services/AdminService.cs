using BBQN.UserManagement.API.DataAccess;
using BBQN.UserManagement.API.Models;

namespace BBQN.UserManagement.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminDBAdaptor _adminDBAdaptor;
        public AdminService(IAdminDBAdaptor adminDBAdaptor)
        {
            _adminDBAdaptor = adminDBAdaptor;

        }

        /// <summary>
        ///  update admin details 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="adminModel"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAdmin(Admin admin)
        {
            return await _adminDBAdaptor.UpdateAdmin(admin);

        }
        /// <summary>
        /// Get all users who all are admins
        /// </summary>
        /// <returns></returns>
        public async Task<List<Users>> GetAllAdmins()
        {
            return await _adminDBAdaptor.GetAllAdmins();
        }

      
        /// <summary>
        /// Get admins details by id
        /// </summary>
        /// <returns></returns>
        public async Task<List<Users>> GetUser(int UserID)
        {
            return await _adminDBAdaptor.GetUser(UserID);
        }
        /// <summary>
        /// Add/remove PrivilegeRights To Admin
        /// </summary>
        /// <param name="privilegeIDs"></param>
        /// <param name="userID"></param>
        /// <param name="createdBy"></param>
        /// <param name="lastModifiedB"></param>
        /// <returns></returns>
        public async Task<int> PrivilegeRightsToAdmin(string privilegeIDs, int userID)
        {
            return await _adminDBAdaptor.PrivilegeRightsToAdmin(privilegeIDs, userID);
        }

        /// <summary>
        /// Add/remove PrivilegeRights To Admin
        /// </summary>
        /// <param name="privilegeIDs"></param>
        /// <param name="userID"></param>
        /// <param name="createdBy"></param>
        /// <param name="lastModifiedB"></param>
        /// <returns></returns>
        public async Task<bool> PrivilegeUser( int userID,bool isPrivilege)
        {
            return await _adminDBAdaptor.PrivilegeUser( userID,isPrivilege);
        }

        /// <summary>
        ///   Blok or Unblok User
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="adminModel"></param>
        /// <returns></returns>
        public async Task<bool> BlockUnblockUser(Admin admin)
        {
            return await _adminDBAdaptor.BlockUnblockUser(admin);

        }
    }
}
