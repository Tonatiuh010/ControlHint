using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Engine.BO.FlowControl;

namespace Engine.BO.AccessControl
{
    public class CheckBase : BaseBO
    {
        public Employee? Employee { get; set; }
        public DateTime? CheckDt { get; set; }
        public string? CheckType { get; set; }

    }

    public class Check : CheckBase
    {                
        //public CardEmployee? Card { get; set; }        

        [JsonPropertyName("device")]
        public Device? Device { get; set; }
    }
}
