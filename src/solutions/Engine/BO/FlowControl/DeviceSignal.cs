using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.FlowControl
{
    public class DeviceSignal
    {        
        public DeviceHintConfig HintConfig { get; set; }
        public string StatusFinger { get; set; }
        public int Confidence { get; set; }
        
        public DeviceSignal(DeviceHintConfig device, string status, int confidence)
        {
            HintConfig = device;
            StatusFinger = status;
            Confidence = confidence;
        }

    }
}
