using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mind_Your_Drink_Models.UserState;
using Mind_Your_Drink_Models.Models;

namespace Mind_Your_Drink_Models
{
    public static class UserStateFactory
    {
        public static IUserState Update(string stateName)
        {
            return stateName switch
            {
                "Banned" => new BanState(),
                "Active" => new ActiveState(),

                //_ => new UnPaidState(),
            };
        }
    }
}
