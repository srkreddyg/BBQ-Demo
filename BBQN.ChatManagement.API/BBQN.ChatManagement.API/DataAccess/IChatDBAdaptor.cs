using BBQN.ChatManagement.API.Models;
namespace BBQN.ChatManagement.API.DataAccess
{
    /// <summary>
    ///Interface for handling   Group Management in DB-DAL
    /// </summary>
    public interface IChatDBAdaptor
    {
        /// <summary>
        /// To Create Group
        /// </summary>
        /// <param name="groupUsers"></param>
        /// <returns></returns>
        Task<int> CreateGroup(GroupUsers groupUsers);

        /// <summary>
        /// To Update Chat Group
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        Task<bool> UpdateGroup(GroupUsers groupUsers);

        /// <summary>
        /// Deleting Group
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        Task<bool> DeleteGroup(int groupId);

        /// <summary>
        /// Remove User From a Specific Group
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<bool> RemoveUserFromGroup(int groupId, int userId);

        /// <summary>
        ///  Get all User Based on Filter Condition
        /// </summary>
        /// <param name="getUserFilter"></param>
        /// <returns></returns>
        Task<List<User>> GetAllUsers(GetUserFilter getUserFilter);

        /// <summary>
        ///  Get all Existing Groups in System
        /// </summary>
        /// <returns></returns>
        Task<List<Group>> GetAllGroups();

        /// <summary>
        /// Get all Groups Specific to a User
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<List<Group>> GetAllGroups(int userID);

        /// <summary>
        /// Get all User Specific to a Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<List<User>> GetAllUser(int groupId);

        /// <summary>
        ///  Blocking User from a Group
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        Task<bool> BlockUser(int goupId, int userId);
        /// <summary>
        /// Method Loggin Audit for Chat Group
        /// </summary>
        /// <param name="grpID"></param>
        /// <param name="actType"></param>
        /// <param name="act"></param>
        /// <param name="changedBy"></param>
        /// <returns></returns>
        bool AuditGroup(int grpID , string actType , string act , string  changedBy );
    }
}
