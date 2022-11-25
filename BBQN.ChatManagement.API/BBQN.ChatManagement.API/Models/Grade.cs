namespace BBQN.ChatManagement.API.Models
{
    public class Grade
    {
        public int GradeID { get; set; }
        public string? GradeCode { get; set; }
        public string? GradeDescription { get; set; }
        public int LastModifiedBy { get; set; }
        public int LastModifiedData { get; set; }
        
    }
}
