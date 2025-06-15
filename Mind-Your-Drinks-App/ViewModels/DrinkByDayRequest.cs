using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mind_Your_Drinks_App.ViewModels
{
    public class DrinkByDayRequest
    {
        public string PasswordHash { get; set; }           // ID of the user making the request
        public DateTime Date { get; set; }        // The specific day to get drink data for
    }
}
