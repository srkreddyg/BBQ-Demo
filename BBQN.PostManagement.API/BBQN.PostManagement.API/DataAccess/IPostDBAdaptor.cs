using BBQN.PostManagement.API.Models;
using System.Text.RegularExpressions;

namespace BBQN.PostManagement.API.DataAccess
{
    public interface IPostDBAdaptor
    {
        /// <summary>
        /// Create Post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        Task<int> CreatePost(SocialPost post);

        /// <summary>
        /// Updating Post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        Task<bool> UpdatePost(SocialPost post);

        /// <summary>
        /// Deleting Post
        /// </summary>
        /// <param name="postID"></param>
        /// <returns></returns>
        Task<bool> DeletePost(int postID);

        /// <summary>
        /// Get all Posts Specific to a User
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<List<SocialPost>> GetAllPosts(int userID);

        /// <summary>
        /// Get all Posts Reported
        /// </summary>
            /// <returns></returns>
        Task<List<SocialPost>> GetAllReportedPosts();
        /// <summary>
        /// Comment Post
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<bool> CommentPost(int postID, int userId, string comment);
        /// <summary>
        /// Like Post
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> LikePost(int postID, int userId);
        /// <summary>
        /// Report  Post
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> ReportPost(int postID, int userId);
        
    }
}
