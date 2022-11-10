using Engine.BO.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.FlowControl
{
    public class DeviceHintConfig
    {
        public Employee Employee { get; set; }
        public Device Device { get; set; }
        public int HintKey { get; set; }

        public DeviceHintConfig(int employeeId, int deviceId, int hintKey)
        {
            Employee = new Employee() { Id = employeeId };
            Device = new Device() { Id = deviceId };
            HintKey = hintKey;
        }

        public DeviceHintConfig(Employee employee, Device device, int hintKey)
        {
            Employee = employee;
            Device = device;
            HintKey = hintKey;
        }
    }
}
