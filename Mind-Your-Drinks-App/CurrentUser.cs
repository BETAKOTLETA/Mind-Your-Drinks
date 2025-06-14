using Mind_Your_Drink_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mind_Your_Drinks_App
{
    public static class GlobalState
    {
        public static User CurrentUser { get; set; }
    }
}
