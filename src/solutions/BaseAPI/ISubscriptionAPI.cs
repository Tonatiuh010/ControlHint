using Engine.BL.Actuators2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.BO.FlowControl;

namespace BaseAPI
{
    public interface ISubscriptionAPI
    {
        public API GetAPI();

        public List<Endpoint> GetEndpoints();

        public void SendAPI();        
    }
}
