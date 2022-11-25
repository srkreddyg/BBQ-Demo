namespace BBQN.UserManagement.API.Models
{
    public class Users
    {
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Gender { get; set; }
        public int CompanyID { get; set; }
        public string? EmailID { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime LeavingDate { get; set; }
        public long MobileNumber { get; set; }
        public bool IsBlock { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsPrivilegedUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActiveStatus { get; set; }
        public int OutletID { get; set; }
        public int GradeID { get; set; }
        public int RoleID { get; set; }
        public int RegionID { get; set; }
        public int DepartmentID { get; set; }
        public int SubDepartmentID { get; set; }
        public int EmployeeCategoryID { get; set; }
        public int DivisionID { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
