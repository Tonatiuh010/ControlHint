using Engine.Interfaces.DataCollector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Services.DataCollector
{
    public class FlowCollector : IFlowCollector
    {
        public static IFlowCollector.SearchFlow SearchFlow { get; set; } = () => throw new NotImplementedException();
        public static IFlowCollector Instance(int id) => new FlowCollector(SearchFlow, id);

        public int Id { get; set; }
        public IFlowCollector.SearchFlow GetFlow { get; }
        
        private FlowCollector(IFlowCollector.SearchFlow callback, int id)
        {
            GetFlow = callback;
            Id = id;
        }

    }
}
