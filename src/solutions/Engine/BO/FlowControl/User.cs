using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.FlowControl
{
    public class User : BaseBO
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? UserType { get; set; }
    }
}
