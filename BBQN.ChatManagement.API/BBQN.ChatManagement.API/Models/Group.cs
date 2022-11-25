namespace BBQN.ChatManagement.API.Models
{
    /// <summary>
    /// Group Model For capturing Crud Group
    /// </summary>
    public class Group
    {      
    
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? GroupDescription { get; set; }
        public string? GroupURL { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int FilterOperator { get; set; }
        public bool IsAutoEnroled {get; set; }
	    public int CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public int RoleID { get; set; }
        public string? RoleName { get; set; }
        public int GradeID { get; set; }
        public string? GradeName { get; set; }
        public int RegionID { get; set; }

        public string? RegionName { get; set; }
        public int OutletID { get; set; }

        public string? OutletName { get; set; }
        public int DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
        public int SubDepartmentID { get; set; }

        public string? SubDepartmentName { get; set; }
        public int EmployeeCategoryID { get; set; }

        public string? EmployeeCategoryName { get; set; }
        public int DivisionID { get; set; }
        public string? DivisionName { get; set; }
        public bool  IsGroupDeleted { get; set; }
        public bool IsExEmployeesGroup { get; set; }
        public bool IsActiveStatus { get; set; }
       
        public int  CreatedBy { get; set; }

        public string? CreatedByName { get; set; }
        public int  LastModifiedBy { get; set; }

        public string? LastModifiedName { get; set; }
        public DateTime LastModifiedDate { get; set; }


       
    }
}
