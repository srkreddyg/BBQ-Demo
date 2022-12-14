using BBQN.AutoEnrolled.API.Models;

namespace BBQN.AutoEnrolled.API.DataAccess
{
    public interface IAutoEnrollDbAdaptor
    {
        /// <summary>
        /// add new employee
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> AddAutoEnrollUser(int groupId, User user);
        /// <summary>
        /// Remove the employee who left organization
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Task<bool> RemoveUserAutoEnroll(int userID);

        /// <summary>
        /// Get Matched GroupIDs
        /// </summary>
        /// <returns></returns>
        public Task<List<Group>> GetMatchedGroupIds(GetUserFilter getUserFilter);
    }
}
