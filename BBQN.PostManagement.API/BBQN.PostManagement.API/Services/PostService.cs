using BBQN.PostManagement.API.DataAccess;
using BBQN.PostManagement.API.Models;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BBQN.PostManagement.API.Services
{
    public class PostService : IPostService
    {
        private readonly IPostDBAdaptor _postDBAdaptor;
        public PostService(IPostDBAdaptor postDBAdaptor)
        {
            _postDBAdaptor = postDBAdaptor;

        }
        /// <summary>
        /// Create Post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public  async Task<int> CreatePost(SocialPost post)
        {
            return  await _postDBAdaptor.CreatePost(post);
          }
        /// <summary>
        /// Updating a Post
        /// </summary>
        /// <param name="groupUsers"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePost(SocialPost post)
        {
            return await _postDBAdaptor.UpdatePost(post);
        }

        /// <summary>
        /// Deleting Post
        /// </summary>
        /// <param name="postID"></param>
        /// <returns></returns>
        public async Task<bool> DeleteGroup(int postID)
        {
            return await _postDBAdaptor.DeletePost(postID);
        }

        /// <summary>
        /// Get all Posts Specific to User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<SocialPost>> GetAllPosts(int userID)
        {
            return await _postDBAdaptor.GetAllPosts(userID);

        }


        /// <summary>
        /// Get all Reported Posts 
        /// </summary>
        /// <returns></returns>
        public async Task<List<SocialPost>> GetAllReportedPosts()
        {
            return await _postDBAdaptor.GetAllReportedPosts();

        }
        /// <summary>
        /// Delete Post
        /// </summary>
        /// <param name="postID"></param>
        /// <returns></returns>
        public async Task<bool> DeletePost(int postID)
        {
            return await _postDBAdaptor.DeletePost(postID);
        }
        /// <summary>
        /// Commenting a Post
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task<bool> CommentPost(int postID, int userId, string comment)
        {
            return await _postDBAdaptor.CommentPost(postID,userId,comment);
        }
        /// <summary>
        /// Like A Post
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> LikePost(int postID, int userId)
        {
            return await _postDBAdaptor.LikePost(postID, userId);
        }

        /// <summary>
        /// Report A Post
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> ReportPost(int postID, int userId)
        {
            return await _postDBAdaptor.ReportPost(postID, userId);
        }
    }
}
