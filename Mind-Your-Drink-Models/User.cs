using Mind_Your_Drink_Models.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Mind_Your_Drink_Server.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string HashPassword { get; set; }

        public string? Email { get; set; } //Mb for future?

        protected User(string name, string hashPassword)
        {
            Name = name;
            HashPassword = hashPassword;
        }

        public static User CreateUser(string name, string password)
        {
            var hashPassword = password.ToHashPassword();
            return new User(name, hashPassword);

        }
    }

    public class Admin : User
    {
        public Admin(string name, string password) : base(name, password.ToHashPassword()) { }
    }

    public class ChiefAdmin : Admin
    {
        public ChiefAdmin(string name, string password) : base(name, password) { }
    }
}
