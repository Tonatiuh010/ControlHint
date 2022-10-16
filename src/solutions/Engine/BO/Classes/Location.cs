using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO
{
    public class Location : BaseBO
    {
        public Asset? Country { get; set; }
        public Asset? City { get; set; }
        public string? CityName { get; set; }
    }
}
