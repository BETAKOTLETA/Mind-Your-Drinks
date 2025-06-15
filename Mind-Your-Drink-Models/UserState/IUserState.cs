using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mind_Your_Drink_Models.UserState
{
    public interface IUserState
    {
        string Name { get;}

        IUserState Ban();

        IUserState UnBan();

    }
}
