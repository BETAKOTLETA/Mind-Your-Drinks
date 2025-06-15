using Mind_Your_Drink_Models;
using Mind_Your_Drink_Models.UserState;
using Mind_Your_Drink_Models.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mind_Your_Drink_Models.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string HashPassword { get; set; }

        public string? Email { get; set; } //Mb for future?

        [NotMapped]
        protected IUserState _state;

        public string StateName { get; set; }

        public User(string name, string hashPassword, IUserState userState)
        {
            Name = name;
            HashPassword = hashPassword;
            StateName = userState.Name;
            _state = userState;
        }

        public User()
        {
            _state = new ActiveState(); // or null if necessary
            StateName = _state.Name;
        }

        public static User CreateUser(string name, string password)
        {
            var hashPassword = password.ToHashPassword();
            return new User(name, hashPassword, new ActiveState());
        }

        public void StateAction(Func<IUserState, IUserState> action)
        {
            _state = action(_state);
            Console.Write(_state);
            StateName = _state.Name;
        }

        public void Initialize()
        {
            _state = UserStateFactory.Update(StateName);
        }

    }

    public class Admin : User
    {

        protected Admin() : base() { }

        protected Admin(string name, string password, IUserState userState)
            : base(name, password, userState) { }

        public static Admin CreateAdmin(string name, string password)
        {
            return new Admin(name, password.ToHashPassword(), new ActiveState());
        }

        public void Ban(User target)
        {
            target.StateAction(state => state.Ban());
        }

        public void UnBan(User target)
        {
            target.StateAction(state => state.UnBan());
        }

    }

    public class ChiefAdmin : Admin
    {
        protected ChiefAdmin(string name, string password, IUserState userState) 
            : base(name, password, userState) { }
    }
}
