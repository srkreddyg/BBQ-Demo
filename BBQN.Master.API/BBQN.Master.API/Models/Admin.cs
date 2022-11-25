using System.ComponentModel.DataAnnotations.Schema;

namespace BBQN.Master.API.Models
{
   
    public class Admin
    {       

        public List<UserOperations>? userOperations { get; set; }         
               
    }
  
}
