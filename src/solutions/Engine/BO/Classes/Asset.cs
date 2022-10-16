using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Engine.BO
{
    public class Asset : BaseBO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }

        [JsonIgnore]
        public string? Group { get; set; }

        [JsonIgnore]
        public string? Attr { get; set; }

        [JsonIgnore]
        public string? Attr2 { get; set; }

        [JsonIgnore]
        public string? Description { get; set; }

    }
}
