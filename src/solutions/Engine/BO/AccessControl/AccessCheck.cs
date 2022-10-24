using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class AccessCheck
    {
        public Check? Check { get; set; }
        public bool IsValid { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }
    }
}
