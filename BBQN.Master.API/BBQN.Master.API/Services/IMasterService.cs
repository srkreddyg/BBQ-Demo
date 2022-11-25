
using BBQN.Master.API.Models;

namespace BBQN.Master.API.Services
{
    public interface IMasterService
    {
        /// <summary>
        ///  FetchMasterData function
        /// </summary>
        /// <returns></returns>
        Task<MasterData> FetchMasterData();
        /// <summary>
        /// declare GetOutletsMasterData function
        /// </summary>
        /// <param name="RegionID"></param>
        /// <returns></returns>
        public Task<List<Outlets>> GetOutlets(int regionID);
        /// <summary>
        ///  GetSubDepartmentsMasterData function
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public Task<List<SubDepartments>> GetSubDepartments(int departmentId);

        /// <summary>
        /// Get all Privileges data
        /// </summary>
        /// <returns></returns>
        Task<List<Privileges>> GetAllPrivileges();
    }
}
