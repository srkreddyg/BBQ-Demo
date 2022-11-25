namespace BBQN.ChatManagement.API.Models
{
    /// <summary>
    /// DTO for Handling Group Specific Operation
    /// </summary>
    public class GroupUsers
    {
        public Group? Group { get; set; }
        public List<UserOperations>? userOperations { get; set; }

    }
}
