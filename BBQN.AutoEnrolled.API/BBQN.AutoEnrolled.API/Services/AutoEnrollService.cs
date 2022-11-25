using BBQN.AutoEnrolled.API.DataAccess;
using BBQN.AutoEnrolled.API.Models;

namespace BBQN.AutoEnrolled.API.Services
{
    public class AutoEnrollService : IAutoEnrollService
    {
        private readonly IAutoEnrollDbAdaptor  _autoEnrollDbAdaptor;

        public AutoEnrollService(IAutoEnrollDbAdaptor autoEnrollDbAdaptor)
        {
            _autoEnrollDbAdaptor = autoEnrollDbAdaptor;

        }
        /// <summary>
        /// AddAutoEnrollUser
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> AddAutoEnrollUser(int groupId, User user)
        {
            return await _autoEnrollDbAdaptor.AddAutoEnrollUser(groupId,  user);
        }
        /// <summary>
        /// GetMatchedGroupIds
        /// </summary>
        /// <param name="getUserFilter"></param>
        /// <returns></returns>
        public async Task<List<Group>> GetMatchedGroupIds(GetUserFilter getUserFilter)
        {
            return await _autoEnrollDbAdaptor.GetMatchedGroupIds( getUserFilter);
        }
        /// <summary>
        /// RemoveUserAutoEnroll
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<bool> RemoveUserAutoEnroll(int userID)
        {
          return await _autoEnrollDbAdaptor.RemoveUserAutoEnroll(userID);
        }
    }
}
