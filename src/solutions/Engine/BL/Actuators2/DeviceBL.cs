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
    }
}
