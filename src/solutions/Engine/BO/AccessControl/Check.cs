using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Engine.BO.FlowControl;
using Org.BouncyCastle.Asn1.Mozilla;

namespace Engine.BO.AccessControl
{
    public class CheckBase : BaseBO
    {        
        public DateTime? CheckDt { get; set; }
        public string? CheckType { get; set; }

    }

    public class Check : CheckBase
    {              
        public Employee? Employee { get; set; }

        [JsonPropertyName("device")]
        public Device? Device { get; set; }

        public static List<CheckBase> ToBase(List<Check> checks)
        {
            return checks.Select(x => (CheckBase)x).ToList();
        }
    }
}
