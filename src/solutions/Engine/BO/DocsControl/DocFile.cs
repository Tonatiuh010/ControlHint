using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Engine.Constants;

namespace Engine.BO.DocsControl
{

    public class DocFile : BaseBO
    {
        [JsonIgnore]
        public byte[]? DocImg { get; set; }
        [JsonIgnore]
        public string? HexImg {
            get
            {
                if (DocImg != null)
                {
                    return BitConverter.ToString(DocImg).Replace("-", "");
                }
                else
                {
                    return null;
                }
            }
        }
        [JsonIgnore]
        public string? Params { get; set; }

        public object? Parameters => ParamsToJson();

        private object? ParamsToJson()
        {
            try {
                return JsonSerializer.Deserialize<object>(Params, options: C.CustomJsonOptions );
            } 
            catch
            {
                return new { };
            }
        }

        public static string RemoveHeaderB64(string b64)
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

        public static string AddB64Header(string b64)
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

    public class DocFileExt : DocFile
    {
        public Document? Document { get; set; }
    }

}
