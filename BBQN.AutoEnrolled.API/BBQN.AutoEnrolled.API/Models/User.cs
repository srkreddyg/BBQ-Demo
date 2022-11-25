namespace BBQN.AutoEnrolled.API.Models
{
    public class SBUser
    {
        public string user_id { get; set; }
        public string nickname { get; set; }
       public string profile_url { get; set; }
    }

    public class SBGroupUser
    {
      public string[] user_ids { get; set; }
    }
    public class User
    {
        public int UserID { get; set; }
        public string ImageUrl { get; set; }
        public string UserLoginID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Gender { get; set; }
        public int CompanyID { get; set; }
        public string? EmailID { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public long MobileNumber { get; set; }
        public bool IsBlock { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsPrivilegedUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActiveStatus { get; set; }
        public int OutletID { get; set; }
        public string? OutletName { get; set; }
        public int GradeID { get; set; }
        public string? GradeName { get; set; }
        public int RoleID { get; set; }

        public string? RoleName { get; set; }
        public int RegionID { get; set; }

        public string? RegionName { get; set; }
        public int DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
        public int SubDepartmentID { get; set; }
        public string? SubDepartmentName { get; set; }
        public int EmployeeCategoryID { get; set; }

        public string? EmployeeCategoryName { get; set; }
        public int DivisionID { get; set; }

        public string? DivisionName { get; set; }
        public int LastModifiedBy { get; set; }

        public string? LastModifiedName { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
