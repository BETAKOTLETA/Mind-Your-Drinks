using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mind_Your_Drink_Models.UserState
{
    public class ActiveState : IUserState
    {
        public string Name => "Active";

        public IUserState Ban() => new BanState();
    }  
}
