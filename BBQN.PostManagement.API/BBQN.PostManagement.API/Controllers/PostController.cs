using BBQN.PostManagement.API.Models;
using BBQN.PostManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BBQN.PostManagement.API.Controllers
{
    /// <summary>
    /// Controller for Managing Post
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PostController> _logger;
        private readonly IPostService _postService;
        public PostController(IConfiguration configuration, ILogger<PostController> logger, IPostService postService)
        {
            _configuration = configuration;
            _logger = logger;
            _postService = postService;
        }

        /// <summary>
        /// Create Post
        /// </summary>
        /// <returns></returns>
        [Route("CreatePost")]
        [HttpPost]
        public async Task<IActionResult> CreatePost(SocialPost post)
        {
            try
            {
                //Steps 1. To check Post is for Everyone or Specific Group
                //Get all Users First Written in Chat Management API
                var postID = await _postService.CreatePost(post);
                //Step2:Store Image in Azure Blob Storage
                //Step3 : Azure Redis Cache for Storing Data in Cache
                //Step4: Send Azure Signalr Notification to Group or if it is Priority Notification

                return Ok(new { Result = postID, Status = "Success", Message = "Post Created Successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreatePost  action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Updating Post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [Route("UpdatePost")]
        [HttpPut]
        public async Task<IActionResult> UpdatePost(SocialPost post)
        {
            try
            {
                var isUpdate =await _postService.UpdatePost(post);
                 
             //Step1: Send Azure Signalr Notification to Group,EveryOne
                return Ok(new { Status = "Success", Message = "Post Updated Successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside while Updating the Post by Post id action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Delete Post 
        /// </summary>
        /// <param name="PostID"></param>
        /// <returns></returns>
        [Route("DeletePost/{postID}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePost(int postID)
        {
            try
            {
           
                bool isDeleted = await _postService.DeletePost(postID);
                //Step1: Update Redis Cache
                return Ok(new { Status = "Success", Message = "Post Deleted Successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside while deleting the Post by Post id action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get Posts for a Specific User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("GetAllPosts/{UserId}")]
        [HttpGet]
        public async Task<IActionResult> GetAllPosts(int userId)
        {
            try
            {
                var posts = await _postService.GetAllPosts(userId);
                //Step1: Get Data From Redis cache like User is part of which Group
                //Step2: Everyone Notification+ Groups + Priority
                return Ok(new { Status = "Success", Result = posts });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllPosts by user id action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Like Post
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("LikePost")]
        [HttpPut]
        public async Task<IActionResult> LikePost(int postID, int userId)
        {
            try
            {
                var isUpdate = await _postService.LikePost(postID, userId);
                return Ok(new { Status = "Success", Message = "Post Updated Successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside while Updating the Post by Post id action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get all Reported Post
        /// </summary>
        /// <returns></returns>
        [Route("GetAllReportedPosts")]
        [HttpGet]
        public async Task<IActionResult> GetAllReportedPosts()
        {
            try
            {
                var posts = await _postService.GetAllReportedPosts();
                return Ok(new { Status = "Success", Result = posts });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllReportedPosts: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Report Post
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("ReportPost")]
        [HttpPut]
        public async Task<IActionResult> ReportPost(int postID, int userId)
        {
            try
            {
                var isReported= await _postService.ReportPost(postID, userId);
                return Ok(new { Status = "Success", Message = "Post Updated Successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside while Reporting  the Post action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }



    }
}
