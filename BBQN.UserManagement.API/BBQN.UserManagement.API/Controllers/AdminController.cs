using Microsoft.AspNetCore.Mvc;
using BBQN.UserManagement.API.Services;
using BBQN.UserManagement.API.Models;

namespace BBQN.UserManagement.API.Controllers
{
    /// <summary>
    /// Controller For Manage Admins And Add/Remove PrivilegeRights To Admin
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _adminService;    
        public AdminController(IConfiguration configuration, ILogger<AdminController> logger, IAdminService adminService)
        {
            _configuration = configuration;
            _logger = logger;
            _adminService = adminService;            
        }
        /// <summary>
        /// Get all admin records
        /// </summary>
        /// <returns></returns>
        [Route("GetAllAdmins")]
        [HttpGet]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                var admins = await _adminService.GetAllAdmins();
                return Ok(new { Results = admins, Status = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllAdmins action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get all admin records
        /// </summary>
        /// <returns></returns>
        [Route("GetUser")]
        [HttpGet]
        public async Task<IActionResult> GetUser(int UserID)
        {
            try
            {
                var admin = await _adminService.GetUser(UserID);           
                return Ok(new { Results = admin, Status = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAdmin action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }       

        /// <summary>
        /// Update details of the selected admin
        /// </summary>
        /// <param name="adminModel"></param>
        /// <returns></returns>
        [Route("SetAdmin")]
        [HttpPut]
        public async Task<IActionResult> SetAdmin(Admin admin)
        {
            try
            {
                var isAdminUpdated = await _adminService.UpdateAdmin(admin);               
                return Ok(new { Result = isAdminUpdated, Status = "Success", Message = "User(s) Privilege Updated Successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside SetAdmin action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Add/remove PrivilegeRights To Admin
        /// </summary>
        /// <param name="privilegeID"></param>
        /// <param name="userID"></param>      
        /// <returns></returns>
        [Route("PrivilegeRightsToAdmin")]
        [HttpPut]
        public async Task<IActionResult> PrivilegeRightsToAdmin(List<int> privilegeID, int userID)
        {
            try
            {
                string privilegeIDs = String.Join(",", privilegeID);
                var PrivilegeRights = await _adminService.PrivilegeRightsToAdmin(privilegeIDs, userID);            
                return Ok(new { Result = PrivilegeRights, Status = "Success", Message = "Added privilege rights to admin Successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PrivilegeRightsToAdmin action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Add/remove PrivilegeRights To Admin
        /// </summary>
        /// <param name="privilegeID"></param>
        /// <param name="userID"></param>      
        /// <returns></returns>
        [Route("PrivilegeUser")]
        [HttpPut]
        public async Task<IActionResult> PrivilegeUser(int userID, bool isPrivilege)
        {
            try
            {
                //string privilegeIDs = String.Join(",", privilegeID);
                var PrivilegeRights = await _adminService.PrivilegeUser(userID, isPrivilege);
                return Ok(new { Result = PrivilegeRights, Status = "Success", Message = "Updated privilege to User Successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PrivilegeUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Blok or Unblok User
        /// </summary>
        /// <param name="adminModel"></param>
        /// <returns></returns>
        [Route("BlockUnblockUser")]
        [HttpPut]
        public async Task<IActionResult> BlockUnblockUser(Admin admin)
        {
            try
            {
                var isAdminUpdated = await _adminService.BlockUnblockUser(admin);
                return Ok(new { Result = isAdminUpdated, Status = "Success", Message = "User(s)  Updated Successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside SetAdmin action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
