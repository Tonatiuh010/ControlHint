using Engine.BO;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UttClassroom.Classes.Converters
{
    public class JsonResultConverter : JsonConverter<Result>
    {
        internal class DataHistorial {
            public object? Value { get; set; }
            public string? TempValue { get; }
            public bool WasEdited { get; set; } = false;
            public bool IsNull => Value != null;

            public DataHistorial(object? value)
            {
                Value = value;
                TempValue = value?.ToString();
            }
        }

        public override Result? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Result value, JsonSerializerOptions options)
        {            
            List<KeyValuePair<string, DataHistorial>> keys = new()
            {
                new KeyValuePair<string, DataHistorial>("DATA1", new (value.Data)),
                new KeyValuePair<string, DataHistorial>("DATA2", new (value.Data2)),
                new KeyValuePair<string, DataHistorial>("DATA3", new (value.Data3)),
                new KeyValuePair<string, DataHistorial>("DATA4", new (value.Data4))
            };


            for (int i = 0; i < keys.Count; i++)
            {
                var k = keys[i];
                DataHistorial xKey = keys[i].Value;

                if (SetResultKey(ref xKey, k.Key))
                {
                    xKey.WasEdited = true;
                } else
                {
                    xKey.WasEdited = false;
                }
            }

            //value.Data = keys[0].Value.Value;
            //value.Data2 = keys[1].Value.Value;
            //value.Data3 = keys[2].Value.Value;
            //value.Data4 = keys[3].Value.Value;

            //var str = JsonSerializer.Serialize(value, new JsonSerializerOptions()
            //{
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //    UnknownTypeHandling = JsonUnknownTypeHandling.JsonNode,
            //});

            //foreach (var s in keys)
            //{
            //    var xKey = s.Value;
            //    if (xKey.WasEdited)
            //        str = str.Replace("\"{{" + s.Key + "}}\"", xKey.TempValue);
            //}

            //writer.WriteStringValue(,);


            writer.WriteStartObject();
            writer.WriteString("status", value.Status);
            writer.WriteString("message", value.Message);

            for (int x = 0; x < keys.Count; x ++)
            {
                var xValue = keys[0].Value;
                string propName = $"data{(x == 0 ? "" : x + 1)}";

                if (xValue.TempValue != null)
                {
                    writer.WriteString(
                        JsonEncodedText.Encode( propName ),
                        JsonEncodedText.Encode( xValue.TempValue.ToString() )
                    );

                } else
                {
                    writer.WritePropertyName(propName);
                    writer.WriteNullValue();
                }

            }

            writer.WriteEndObject();
        }

        private static bool SetResultKey(ref DataHistorial obj, string key)
        {
            if(obj.Value != null)
            {
                obj.Value = "{{" + key + "}}";
                return true;
            } else
            {
                return false;
            }
        }

    }
}
