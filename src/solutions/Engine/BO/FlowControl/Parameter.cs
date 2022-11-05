using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Engine.BO.FlowControl
{
    public class Parameter : BaseBO
    {
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public object? Value { get; set; }
        public string? Description { get; set; }
        public bool IsRequired { get; set; }
        
    }
}
