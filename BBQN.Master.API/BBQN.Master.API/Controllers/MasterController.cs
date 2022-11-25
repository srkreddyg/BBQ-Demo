using BBQN.Master.API.Models;
using BBQN.Master.API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BBQN.Master.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : Controller
    {
        /// <summary>
        /// readonly variables 
        /// </summary>
        private readonly IConfiguration _configuration;
        private readonly ILogger<MasterController> _logger;
        private readonly IMasterService _masterService;
        /// <summary>
        /// Constructor DI
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="masterService"></param>
        public MasterController(IConfiguration configuration, ILogger<MasterController> logger, IMasterService masterService)
        {
            _configuration = configuration;
            _logger = logger;
            _masterService = masterService;
        }
        #region IActionResult Methods
        /// <summary>
        /// Fetch master data like Company,Regions,,Grade,UserRole,EmployeeC,Divisions
        /// </summary>
        /// <returns></returns>
        [Route("GetMasterData")]
        [HttpGet]
        public async Task<IActionResult> GetMasterData()
        {
            try
            {
                
               var md = await _masterService.FetchMasterData();             
                _logger.LogInformation($"Returned all master date like Company,Regions,,Grade,UserRole,EmployeeC,Divisions from database.");               
              return Ok(new { MasterData = md, Status = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetMasterData action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Get SubDepartments based on  Department ID
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        [Route("GetSubDepartments/{departmentID}")]
        [HttpGet]
        public async Task<IActionResult> GetSubDepartments(int departmentID)
        {
            try
            {
               var subDepartments =await _masterService.GetSubDepartments(departmentID);
                _logger.LogInformation($"Returned all SubDepartments data from database.");
                return Ok(new { SubDepartments = subDepartments, Status = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetSubDepartments action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Get Outlets based on region ID
        /// </summary>
        /// <param name="RegionID"></param>
        /// <returns></returns>
        [Route("GetOutlet/{regionID}")]
        [HttpGet]
        public async Task<IActionResult> GetOutlets(int regionID)
        {
            try
            {
                var outlets = await _masterService.GetOutlets(regionID);
                _logger.LogInformation($"Returned all Outlets from database.");
                return Ok(new { Outlets = outlets, Status = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOutlets action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Get all privileges records
        /// </summary>
        /// <returns></returns>
        [Route("GetAllPrivileges")]
        [HttpGet]
        public async Task<IActionResult> GetAllPrivileges()
        {
            try
            {
                var privileges = await _masterService.GetAllPrivileges();
                return Ok(new { Results = privileges, Status = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllPrivileges action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion

    }
}
