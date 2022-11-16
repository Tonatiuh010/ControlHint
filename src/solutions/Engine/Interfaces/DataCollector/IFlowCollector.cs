using Engine.BO;
using Engine.BO.FlowControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Interfaces.DataCollector
{
    public interface IFlowCollector
    {
        public delegate Flow SearchFlow();

        public int Id { get; set; }
        public SearchFlow GetFlow { get; }
        public Flow Flow => GetFlow();
    }
}
