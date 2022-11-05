using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.FlowControl
{
    public class DeviceAction
    {
        public string DeviceName { get; set; }
        public string ActionName { get; set; }
        public Result? ActionResult { get; set; }

        public DeviceAction(string deviceName, string actionName)
        {
            DeviceName = deviceName;
            ActionName = actionName;            
        }
    }
}
