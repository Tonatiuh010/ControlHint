using Engine.BO;
using Engine.BO.FlowControl;
using Engine.Constants;
using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BL.Actuators2
{
    public class DeviceBL : BaseBL<FlowControlDAL>
    {
        public List<Device> GetDevices(int? id = null, string? name = null, bool? status = true) => Dal.GetDevices(id, name, status);

        public Device? GetDevice(int id) => GetDevices(id).FirstOrDefault();        

        public ResultInsert SetDevice(Device device) => Dal.SetDevice(device, C.GLOBAL_USER);

        public List<DeviceHintConfig> GetDeviceHintConfigs(string deviceName, int? employeeId = null, int? hintKey = null )
        {
            int deviceId = Dal.GetDeviceId(deviceName);

            if (deviceId == 0)
                throw new Exception($"Device {deviceName} not founded!");

            return Dal.GetDeviceEmployeeHints(deviceId, employeeId, hintKey);
        }        

        public List<DeviceHintConfig> GetDeviceHintConfigs(int? deviceId = null, int? employeeId = null, int? hintKey = null ) 
            => Dal.GetDeviceEmployeeHints(deviceId, employeeId, hintKey);

        public DeviceHintConfig? GetDeviceHintConfig(string deviceName, int employeeId) 
            => GetDeviceHintConfigs(deviceName, employeeId).FirstOrDefault();


        public Result SetDeviceEmployeeHint(string deviceName, int employeeId, int hintKey)
        {
            int deviceId = Dal.GetDeviceId(deviceName);

            if (deviceId == 0)
                throw new Exception($"Device {deviceName} not founded!");

            return Dal.SetDeviceEmployeeHint(deviceId, employeeId, hintKey, C.GLOBAL_USER);
        }

        public Result SetDeviceEmployeeHint(int deviceId, int employeeId, int hintKey) 
            => Dal.SetDeviceEmployeeHint(deviceId, employeeId, hintKey, C.GLOBAL_USER);
    }
}
