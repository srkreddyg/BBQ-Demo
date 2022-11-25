namespace BBQN.AutoEnrolled.API.Models
{
    public class GetUserFilter
    {       
        public int CompanyID { get; set; }
        public int OutletID { get; set; }
        public int Department { get; set; }
        public int SubDepartment { get; set; }
        public int GradeID { get; set; }
        public int RoleID { get; set; }
         public DateTime? DateofJoining { get; set; }
        public int EmployeeCategoryID { get; set; }
        public int DivisionID { get; set; }
        public int RegionID { get; set; }
    }
   

    public enum ActionType
    {
        Added = 0,
        Deleted = 1     

    }
}
