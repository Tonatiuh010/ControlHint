using Engine.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class ImageData
    {
        [JsonIgnore]
        public byte[]? Bytes { get; set; }
        [JsonIgnore]
        public string? B64 => Bytes != null ? AddB64Header(Convert.ToBase64String(Bytes)) : string.Empty;
        [JsonIgnore]
        public string? Hex
        {
            get
            {
                if (Bytes != null)
                {
                    return BitConverter.ToString(Bytes).Replace("-", "");
                }
                else
                {
                    return null;
                }
            }
        }

        public string? Url { get; set; }

        public ImageData() => Bytes = null;

        public ImageData(byte[] bytes) => Bytes = bytes;

        public ImageData(string? b64) =>
            Bytes = string.IsNullOrEmpty(b64) ? null : Convert.FromBase64String(RemoveHeaderB64(b64));

        public static byte[] GetBytesFromUrl(string url) => Utils.GetImage(url);

        private static string RemoveHeaderB64(string b64)
        {
            var indexOf = b64.IndexOf(",");

            if (indexOf == -1)
            {
                return b64;
            }
            else
            {
                return b64.Substring(indexOf + 1);
            }
        }

        private static string AddB64Header(string b64)
        {
            var indexOf = b64.IndexOf(",");

            if (indexOf != -1)
            {
                return b64;
            }
            else
            {
                return $"data:image/png;base64,{b64}";
            }
        }

    }
}
