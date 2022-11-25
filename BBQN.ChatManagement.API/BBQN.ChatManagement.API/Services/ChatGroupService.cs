using BBQN.ChatManagement.API.DataAccess;
using BBQN.ChatManagement.API.Models;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System.Data;
using System.Text.RegularExpressions;
using Group = BBQN.ChatManagement.API.Models.Group;

namespace BBQN.ChatManagement.API.Services
{
    /// <summary>
    /// Service for Handling Chat Group
    /// </summary>
    public class ChatGroupService : IChatGroupService
    {
        private readonly IChatDBAdaptor _chatDBAdaptor;
        public ChatGroupService(IChatDBAdaptor chatDBAdaptor)
        {
            _chatDBAdaptor = chatDBAdaptor;

        }
        /// <summary>
        /// Creating Group from a Service
        /// </summary>
        /// <param name="groupUsers"></param>
        /// <returns></returns>
        public async Task<int> CreateGroup(GroupUsers groupUsers)
        {
            return await _chatDBAdaptor.CreateGroup(groupUsers);
        }
        /// <summary>
        /// Updating a Group
        /// </summary>
        /// <param name="groupUsers"></param>
        /// <returns></returns>
        public async Task<bool> UpdateGroup(GroupUsers groupUsers)
        {
            return await _chatDBAdaptor.UpdateGroup(groupUsers);
        }

        /// <summary>
        /// Deleting Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteGroup(int groupId)
        {
            return await _chatDBAdaptor.DeleteGroup(groupId);
        }
        /// <summary>
        /// Removing User From a Specific Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveUserFromGroup(int groupId, int userId)
        {
            return await _chatDBAdaptor.RemoveUserFromGroup(groupId, userId);
        }

        /// <summary>
        /// Get all Users Based on filter Criteria Provided
        /// </summary>
        /// <param name="getUserFilter"></param>
        /// <returns></returns>
        public async Task<List<User>> GetAllUsers(GetUserFilter getUserFilter)
        {
            return await _chatDBAdaptor.GetAllUsers(getUserFilter);

        }
        /// <summary>
        /// Get all Groups Irrespective of System
        /// </summary>
        /// <returns></returns>
        public async Task<List<Group>> GetAllGroups()
        {
            return await _chatDBAdaptor.GetAllGroups();

        }
        /// <summary>
        /// Get all Groups Specific to User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Group>> GetAllGroups(int userID)
        {
            return await _chatDBAdaptor.GetAllGroups(userID);

        }
        /// <summary>
        /// Get all User Specific to a Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<List<User>> GetAllUser(int groupId)
        {
            return await _chatDBAdaptor.GetAllUser(groupId);
        }
        /// <summary>
        ///  Block User From a Specifc Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<bool> BlockUser(int groupId, int userID)
        {
            return await _chatDBAdaptor.BlockUser(groupId, userID);
        }
    }
}
