using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO
{
    public class Scholarly : BaseBO
    {
        public string? Name { get; set; }
        public Asset? Type { get; set; }
        public Address? Address { get; set; }
    }
}
