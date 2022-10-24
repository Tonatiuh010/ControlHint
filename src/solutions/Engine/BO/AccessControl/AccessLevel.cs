using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class AccessLevel : BaseBO
    {
        public string? Name { get; set; }
        [JsonIgnore]
        public string? Status { get; set; }
    }
}
