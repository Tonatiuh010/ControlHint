using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO
{
    public class Major : BaseBO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public Asset? Level { get; set; }
    }
}
