using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BBQN.ChatManagement.API.Models
{
    /// <summary>
    /// Model for handling Operations on User
    /// </summary>
    public class UserOperations
    {
        public User? user { get; set; }

        public ActionType action { get; set; } 
    }
    /// <summary>
    /// Action for Handling Group Users Operations
    /// </summary>
    public enum ActionType
    {
        Added=0,
        Deleted=1,
        Updated=2,
        Blocked = 3

    }
    /// <summary>
    /// Action for Handling  Chat API  Operations
    /// </summary>
    public enum ChatActionType
    {
        GroupAdded = 0,
        GroupDeleted = 1,
        GroupUpdated = 2,
        UserDeleted = 3

    }
}
