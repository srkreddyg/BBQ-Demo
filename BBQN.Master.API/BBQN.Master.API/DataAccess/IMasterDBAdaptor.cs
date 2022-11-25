using BBQN.Master.API.Models;

namespace BBQN.Master.API.DataAccess
{
    public interface IMasterDBAdaptor
    {
        // interface method for FetchMasterData   
        Task<MasterData> FetchMasterData();
        // interface method for GetOutlets 
        public Task<List<Outlets>> GetOutlets(int regionID);

        // interface method for GetDepartments 
        public Task<List<SubDepartments>> GetDepartments(int departmentId);
        /// <summary>
        /// Get all Privileges data
        /// </summary>
        /// <returns></returns>
        Task<List<Privileges>> GetAllPrivileges();
    }
}
