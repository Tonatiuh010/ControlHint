using Engine.BO.FlowControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class CheckAlt : BaseBO
    {
        public Device? Device { get; set; }
        public Employee? Employee { get; set; }
        public DateTime? CheckDt { get; set; }
        public string? CheckType { get; set; }

    }
}
