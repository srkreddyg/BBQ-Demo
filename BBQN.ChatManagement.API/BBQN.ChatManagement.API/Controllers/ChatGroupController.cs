using BBQN.ChatManagement.API.Models;
using BBQN.ChatManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBQN.ChatManagement.API.Controllers
{
    /// <summary>
    /// Controller for Chat Group ,Crud group
    /// </summary>
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatGroupController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ChatGroupController> _logger;
        private readonly IChatGroupService _chatService;


        public ChatGroupController(IConfiguration configuration, ILogger<ChatGroupController> logger, IChatGroupService chatService)
        {
            _configuration = configuration;
            _logger = logger;
            _chatService = chatService;
        }
        /// <summary>
        /// Create Group
        /// </summary>
        /// <returns></returns>
        [Route("CreateGroup")]
        [HttpPost]
        public async Task<IActionResult> CreateGroup(GroupUsers groupUser)
        {
            try
            {
                 var groupID = await _chatService.CreateGroup(groupUser);
                return Ok(new { Result = groupID, Status = "Success", Message = "Group Created Successfully" });
               }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateGroup action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update record from group table  based on group id 
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        [Route("UpdateGroup")]
        [HttpPut]
        public async Task<IActionResult> UpdateGroup(GroupUsers groupUser)
        {
            try
            {
                var isUpdate = await _chatService.UpdateGroup(groupUser);
                 return Ok(new { Status = "Success", Message = "Group Updated Successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside while deleting the group by group id action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Delete Group 
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        [Route("DeleteGroup/{groupID}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGroup(int groupID)
        {
            try
            {
    
                bool isDeleted = await _chatService.DeleteGroup(groupID);
                return Ok(new { Status = "Success", Message = "Group Deleted Successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside while deleting the group by group id action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        ///// <summary>
        /////Delete User From a Specific Group
        ///// </summary>
        ////<param name="groupId"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        [Route("DeleteUserGroup/{groupId}/{userId}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveUserFromGroup(int groupId, int userId)
        {
            try
            {
                bool isDeleted = await _chatService.RemoveUserFromGroup(groupId, userId);
                return Ok(new { Status = "Success", Message = "User Removed From Group Successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside while deleting the group by group id action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Get all Users Based on  Filter Conditions
        /// </summary>
        /// <param name="filterConditions"></param>
        /// <returns></returns>
        [Route("GetAllUsers")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetUserFilter filterConditions)
        {
            try
            {
                var users = await _chatService.GetAllUsers(filterConditions);
                return Ok(new { Status = "Success", Result = users });
             }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllUsers action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Get All groups details 
        /// </summary>
        /// <returns></returns>
        [Route("GetAllGroups")]
        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            try
            {
                var groups = await _chatService.GetAllGroups();
                return Ok(new { Status = "Success", Result = groups });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllGroups action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Get Groups for a Specific User
        /// </summary>
        /// <param name="UsreId"></param>
        /// <returns></returns>
        [Route("GetAllGroups/{UserId}")]
        [HttpGet]
        public async Task<IActionResult> GetAllGroups(int UserId)
        {
            try
            {
                var groups = await _chatService.GetAllGroups(UserId);
                return Ok(new { Status = "Success", Result = groups });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllGroups by user id action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// get users based on group 
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        [Route("GetAllUsers/{groupId}")]
        [HttpGet]
        public async Task<IActionResult> GetAllUser(int groupId)
        {
            try
            {
                var users = await _chatService.GetAllUser(groupId);
                return Ok(new { Status = "Success", Result = users });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllUsers by group id action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Block a User From a Group
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("BlockUser/{groupId}/{userId}")]
        public async Task<IActionResult> BlockUser(int groupId, int userId)
        {
            try
            {
                bool isBlocked = await _chatService.BlockUser(groupId, userId);
                return Ok(new { Status = "Success", Message = "User has been  Blocked  For Group." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong While Block Operation: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

    }
}
