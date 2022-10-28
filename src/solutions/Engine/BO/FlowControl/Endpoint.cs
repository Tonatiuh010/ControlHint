using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.FlowControl
{
    public class Endpoint : BaseBO
    {
        public string? Route { get; set; }
        public string? RequestType { get; set; }
        public API Api { get; set; }
        public List<Parameter> Params { get; set; }

        public Endpoint()
        {
            Api = new API();
            Params = new List<Parameter>();
        }
    }
}
