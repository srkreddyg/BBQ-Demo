namespace BBQN.Master.API.Models
{
    public class MasterData
    {
        public List<Company> Company { get; set; }
        public List<Regions> Regions { get; set; }
        public List<Departments> Departments { get; set; }
        public List<Grade> Grade { get; set; }
        public List<UserRole> UserRole { get; set; }
        public List<EmployeeCategory> EmployeeCategory { get; set; }
        public List<Divisions> Divisions { get; set; }
    }
}
