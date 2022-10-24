using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class Device : BaseBO
    {

        public string? Name { get; set; }
        public string? Status { get; set; }

        [JsonPropertyName("last_update")]
        public DateTime? LastUpdate { get; set; }

        public int? Activations { get; set; }
        public int? Unsuccessful { get; set; }
    }
}
