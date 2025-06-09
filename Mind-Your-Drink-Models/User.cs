using System.ComponentModel.DataAnnotations;

namespace Mind_Your_Drink_Server.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public required string Name { get; set; }

        public string? Email { get; set; } //Mb for future?

    }

    public class Admin : User {
        
    } 

    public class ChiefAdmin : Admin { 
    
    }
}
