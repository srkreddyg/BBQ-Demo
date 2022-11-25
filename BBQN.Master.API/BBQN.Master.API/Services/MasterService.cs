using BBQN.Master.API.DataAccess;
using BBQN.Master.API.Models;
namespace BBQN.Master.API.Services
{
    public class MasterService : IMasterService
    {
        private readonly IMasterDBAdaptor _masterDBAdaptor;
        public MasterService(IMasterDBAdaptor masterDataDB)
        {
            _masterDBAdaptor = masterDataDB;
        }
        #region calling  dbadapter methods

        /// <summary>
        /// Get Master Data 
        /// </summary>
        /// <returns></returns>
        public async Task<MasterData> FetchMasterData()
        {
            return await _masterDBAdaptor.FetchMasterData();
        }
        /// <summary>
        /// Get SubDepartments data
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<List<SubDepartments>> GetSubDepartments(int departmentId)
        {
            return await _masterDBAdaptor.GetDepartments(departmentId);
        }
        /// <summary>
        /// Get outlet data 
        /// </summary>
        /// <param name="regionID"></param>
        /// <returns></returns>
        public async Task<List<Outlets>> GetOutlets(int regionID)
        {
            return await _masterDBAdaptor.GetOutlets(regionID);
        }
        /// <summary>
        /// Get all users who all are admins
        /// </summary>
        /// <returns></returns>
        public async Task<List<Privileges>> GetAllPrivileges()
        {
            return await _masterDBAdaptor.GetAllPrivileges();
            #endregion
        }
    }
}
