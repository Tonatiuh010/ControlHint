using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class Employee : BaseBO
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public ImageData? Image { get; set; }
        public Position? Job { get; set; }
        public List<AccessLevel>? AccessLevels { get; set; }
        public Shift? Shift { get; set; }        
        public string? Status { get; set; }
    }
}
