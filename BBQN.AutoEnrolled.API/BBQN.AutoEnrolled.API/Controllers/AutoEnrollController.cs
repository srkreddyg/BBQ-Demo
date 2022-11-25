using BBQN.AutoEnrolled.API.Common;
using BBQN.AutoEnrolled.API.Integrations;
using BBQN.AutoEnrolled.API.Models;
using BBQN.AutoEnrolled.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;

namespace BBQN.AutoEnrolled.API.Controllers
{
    /// <summary>
    /// Controller for Adding/Removing User in Group
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AutoEnrollController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AutoEnrollController> _logger;
        private readonly IAutoEnrollService _autoEnrollService;
        private readonly IIntegration _integration;

        public AutoEnrollController(IConfiguration configuration, ILogger<AutoEnrollController> logger, IAutoEnrollService autoEnrollService, IIntegration integration)
        {
            _configuration = configuration;
            _logger = logger;
            _autoEnrollService = autoEnrollService;
            _integration = integration;
        }

        /// <summary>
        ///Assign Group to Criteria Matching User
        /// </summary>
        /// <returns></returns>
        [Route("AddAutoEnrollUser")]
        [HttpGet]
        public async Task<IActionResult> AddAutoEnrollUser()
        {
            try
            {


                ////Step1: Interact with ZingHR
                var streamData = _integration.GetData("", null, false).Result;
                var users = JsonSerializer.DeserializeAsync<List<User>>(streamData).Result;
                //var users = await JsonSerializer.DeserializeAsync<List<User>>(_integration.GetData("", null,false).Result);
                 if (users == null) return Ok();
                #region  Unit Testing
                ////Step2:Convert it into SendBirdUser
                //SBUser user = GetSBUser(users[0]);
                ////User Created in SendBird
                //var isUsreCreated = _integration.PostData("v3/users", new Dictionary<string, string>() { { "Api-Token", Constants.APIToKen } }, user);
                ////Step3:   Get ALL Groups with Filter Criteria/Auto Enroll On From DB
                //GetUserFilter getUserFilter = GetFilterUser(users[0]);
                //var groups = _autoEnrollService.GetMatchedGroupIds(getUserFilter);
                //foreach (var group in groups.Result)
                //{

                //    //     userIDs. Add(user.user_id);

                //    SBGroupUser sbGroup = new SBGroupUser()
                //    {
                //        user_ids = new string[] { "Vamsi" }
                //        //user_ids = new string[] { user.user_id } ;
                //    };

                //    //User Added  in SendBird Server Group
                //   // var isUserAdded = await _integration.PostData($"v3/group_channels/sendbird_group_channel_8983_7c1a078f1991829828244b7aa643e9ebe6ed46a7/invite", new Dictionary<string, string>() { { "Api-Token", Constants.APIToKen } }, sbGroup);
                //    var isUserAdded = await _integration.PostData($"v3/group_channels/{group.GroupURL}/invite", new Dictionary<string, string>() { { "Api-Token", Constants.APIToKen } }, sbGroup);
                //    //Add Users in DB and in Group as well

                //    var addGroupuser = _autoEnrollService.AddAutoEnrollUser(group.GroupId, users.Find(x => x.UserLoginID.Equals(user.user_id)));
                //}
                #endregion
                Parallel.ForEach(users, userM =>
                {
                   //Create User in SendBird
                    var isUsreCreated = _integration.PostData("v3/users", new Dictionary<string, string>() { { "Api-Token", _configuration.GetSection("ApiUrls")["APIToKen"] } }, GetSBUser(userM));
                    //Step3:   Get ALL Groups with Filter Criteria/Auto Enroll On From DB
                    GetUserFilter getUserFilter = GetFilterUser(userM);
                  
                    var groups = _autoEnrollService.GetMatchedGroupIds(getUserFilter);
                    foreach (var group in groups.Result)
                    {
                     SBGroupUser sbGroup = new SBGroupUser()
                        {
                             user_ids = new string[] { userM.UserLoginID } 
                        };

                        //User Added  in SendBird Server Group
                        // var isUserAdded = await _integration.PostData($"v3/group_channels/sendbird_group_channel_8983_7c1a078f1991829828244b7aa643e9ebe6ed46a7/invite", new Dictionary<string, string>() { { "Api-Token", Constants.APIToKen } }, sbGroup);
                        var isUserAdded =  _integration.PostData($"v3/group_channels/{group.GroupURL}/invite", new Dictionary<string, string>() { { "Api-Token", _configuration.GetSection("ApiUrls")["APIToKen"] } }, sbGroup);
                        //Add Users in DB and in Group as well

                        var addGroupuser = _autoEnrollService.AddAutoEnrollUser(group.GroupId,userM );
                    }

                });
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside AddAutoEnrollUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Remove User From Group
        /// </summary>
        /// <returns></returns>
        [Route("RemoveUserAutoEnroll")]
        [HttpPost]
        public async Task<IActionResult> RemoveUserAutoEnroll()
        {
            try
            {
                var streamData = _integration.GetData("", null, false).Result;
                var users = JsonSerializer.DeserializeAsync<List<User>>(streamData).Result;

                //Step1: Interact with ZingHR
                //Step2:Convert it into User
                //var users = await JsonSerializer.DeserializeAsync<List<User>>(_integration.GetData("", null).Result);
                Parallel.ForEach(users, user =>
                {
                    //Step3: Get ALl Groups which Users Part of 
                    //Step4: Remove from all groups

                    //Remove User, Groups  in SendBird
                    var isUserDeleted = _integration.DeleteData($"v3/users/{user.UserLoginID}", new Dictionary<string, string>() { { "Api-Token", _configuration.GetSection("ApiUrls")["APIToKen"] } });
                    var removeGroupuser = _autoEnrollService.RemoveUserAutoEnroll( 112);
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside RemoveUserAutoEnroll action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        #region private Methods
        /// <summary>
        /// Set Send Bird Users Details
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>        
        private SBUser GetSBUser(User user)
        {
            return new SBUser
            {
                user_id = user.UserLoginID,
                nickname= user.UserLoginID,
                profile_url = user.ImageUrl

            };
        }
        /// <summary>
        /// Get Filter Criterias
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private GetUserFilter GetFilterUser(User user)
        {
            GetUserFilter getUserFilter = new GetUserFilter();
            getUserFilter.CompanyID = user.CompanyID;
            getUserFilter.Department = user.DepartmentID;
            getUserFilter.SubDepartment = user.SubDepartmentID;
            getUserFilter.OutletID = user.OutletID;
            getUserFilter.GradeID = user.GradeID;
            getUserFilter.RoleID = user.RoleID;
            getUserFilter.DateofJoining = user.JoiningDate;
            getUserFilter.EmployeeCategoryID = user.EmployeeCategoryID;
            getUserFilter.DivisionID = user.DivisionID;
            getUserFilter.RegionID = user.RegionID;           
            return getUserFilter;
        }
        #endregion

    }


}
