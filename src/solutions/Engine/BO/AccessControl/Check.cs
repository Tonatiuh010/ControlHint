using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class CheckBase : BaseBO
    {
        public DateTime? CheckDt { get; set; }
        public string? Type { get; set; }

    }

    public class Check : CheckBase
    {
        public delegate Device? FindDevice(int? deviceId);
        private FindDevice? GetDevice { get; set; }
        //public CardEmployee? Card { get; set; }

        [JsonIgnore]
        public int? Device { get; set; }

        [JsonPropertyName("device")]
        public Device? DeviceDetails => GetDevice != null ? GetDevice(Device) : null;
        public void SetDeviceFinder(FindDevice deviceCallback) => GetDevice = deviceCallback;
    }
}
