using Microsoft.Extensions.Hosting;

namespace BBQN.PostManagement.API.Models
{
    public enum PostType
    {
        Post=1,
        Poll=2,
    }
    public class SocialPost
    {
        public int PostID { get; set; }
        public string PostDetails { get; set; }
        public string PostTitle { get; set; }
          public PostType PostType { get; set; }
       public string? PostOptions { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsPostReported { get; set; }
        public bool IsResultDisplayed { get; set; }
        
        public bool IsPostDeleted { get; set; }
        public bool IsResponseRequired { get; set; }
        public bool IsPriorityNotification { get; set; }
        public bool IsCommentRequired { get; set; }
        public bool IsLikesRequired { get; set; }
        public int GroupID { get; set; }
        public int UserID { get; set; }
        public int   PostActiveDays  { get; set; }
        public string ImageURL { get; set; }

   }
   }

