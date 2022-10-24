using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Engine.Constants;

namespace Engine.BO
{
    public class ItemStats
    {
        public string Name { get; set; }
        public object Value { get; set; }
        private string JsonStr => JsonSerializer.Serialize( GetValue() );        

        public ItemStats(string? name, object value)
        {
            Value = value;
            Name = name ?? string.Empty;
        }

        private object? GetValue()
        {

            if (Utils.IsNumeric(Value))
            {
                return Convert.ToDouble(Value.ToString());

            }          

            return Value;
        }

        public JsonObject ToJsonObject() 
            => new(){
                [Name] = JsonNode.Parse(JsonStr)
            };

        public static JsonObject ToJsonObject(List<ItemStats> list)
        {
            JsonObject json = new ();

            foreach (var i in list)
                json[i.Name] = JsonNode.Parse(i.JsonStr);
            
            
            return json;
        }            

    }
}
