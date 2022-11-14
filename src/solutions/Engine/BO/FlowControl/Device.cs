using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Engine.BL.Actuators2;

namespace Engine.BO.FlowControl
{
    public class Device : BaseBO
    {
        public static FlowBL bl = new ();

        public string? Name { get; set; }        
        public string? Ip { get; set; }
        public string? Model { get; set; }
        public bool IsActive { get; set; }

        [JsonPropertyName("last_update")]
        public DateTime? LastUpdate { get; set; }
        public Flow? Flow  => bl.GetFlowByDevice((int)Id);
    }
}
