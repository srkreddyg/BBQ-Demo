namespace BBQN.ChatManagement.API.Models
{
    public class GetUserFilter
    {
       public FilterConditions? filterCondition { get; set;   }
        public int  CompanyID { get; set; }
        public int  LegalEntity { get; set; }
        public int  OutletID { get; set; }
        public int  Department { get; set; }
        public int  SubDepartment { get; set; }
        public int  GradeID { get; set; }
        public int  RoleID { get; set; }
         public DateTime? DateofJoining { get; set; }
        public int  EmployeeCategoryID { get; set; }
        public int DivisionID { get; set; }
        public int  RegionID { get; set; }

    }
}
