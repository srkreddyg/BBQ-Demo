using BBQN.ChatManagement.API.Models;
using System.Data;

namespace BBQN.ChatManagement.API.Services
{
    /// <summary>
    /// Chat Group Interface
    /// </summary>
    public interface IChatGroupService
    {
        /// <summary>
        ///  Creating  chat Group 
        /// </summary>
        /// <param name="creategroup"></param>
        /// <returns></returns>
        Task<int> CreateGroup(GroupUsers groupUsers);
        /// <summary>
        /// Updating Chat Group
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        Task<bool> UpdateGroup(GroupUsers groupUsers);

        /// <summary>
        /// Delete Chat Group
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        Task<bool> DeleteGroup(int GroupId);

        /// <summary>
        /// Remove User From a Specific Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<bool> RemoveUserFromGroup(int groupId, int userID);

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
        /// Block User From a Specifc Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<bool> BlockUser(int groupId, int userID);

    }
}
