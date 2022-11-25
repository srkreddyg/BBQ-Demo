namespace BBQN.Master.API.Models
{
    public class UserOperations
    {

        public Users? user { get; set; }

        public AdminAccess action { get; set; }
    }

    public enum AdminAccess
    {
        Added = 1,
        Removed = 2

    }
}
