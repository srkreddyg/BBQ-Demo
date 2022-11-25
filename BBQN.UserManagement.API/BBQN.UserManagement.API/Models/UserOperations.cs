namespace BBQN.UserManagement.API.Models
{
    public class UserOperations
    {
        public int UserID { get; set; }
        public AdminAccess action { get; set; }
    }
    public enum AdminAccess
    {
        Added = 1,
        Removed = 2

    }
    public enum UserActionType
    {
        PrivilegeAdded = 0,
        PrivilegeUpdated = 1,
        PrivilegeRemoved = 2    

    }
}
