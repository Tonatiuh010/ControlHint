using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.FlowControl
{
    public class Flow : BaseBO
    {
        public string? Name { get; set; }
        public List<Step> Steps { get; set; }

        public Flow()
        {
            Name = string.Empty;
            Steps = new List<Step>();
        }

    }
}
