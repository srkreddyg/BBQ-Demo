namespace BBQN.ChatManagement.API.Models
{
    public class FilterConditions
    {
       public  Operator Operator { get; set; }  

        public DateTime? StartDate { get; set; }

        public DateTime?  EndDate { get; set; }
    }

   public  enum Operator
    {
      Greaterthan =1,
      LessThan =2,
      GreaterThanOrEqual =3,
      LessThanOrEqual =4,
      Between=5 
    }

}