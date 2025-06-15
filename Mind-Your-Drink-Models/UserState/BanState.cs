using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mind_Your_Drink_Models.UserState
{
    public class BanState : IUserState
    {
        public string Name => "Banned";

        public IUserState Ban() => this;

        public IUserState UnBan() => new ActiveState();
    }
}
