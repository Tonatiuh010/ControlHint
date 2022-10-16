using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO
{
    public class Labor : BaseBO 
    {
        public string? Department { get; set; }
        public string? Business { get; set; }
        public string? Job { get; set; }
        public Address? Address { get; set; }
        public Contact? Contact { get; set; }
        public bool? IsStudy { get; set; }
    }
}
