using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mind_Your_Drink_Models.DTO_s
{
    public class DrinkByDayRequest
    {
        public string Name { get; set; }           // ID of the user making the request
        public DateTime Date { get; set; }        // The specific day to get drink data for
    }
}
