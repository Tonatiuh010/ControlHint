using System;
using Engine.BO;
using Engine.BL;
using D = Engine.BL.Delegates;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Classes {
    public static class JsonProperty<T> {
        public static T? GetValue(string name, JsonObject jObj, D.CallbackExceptionMsg? onMissingProperty = null) {
            T? result = default;

            try {
                var jKey = jObj[name];
                if(jKey != null) {
                    result = jKey.GetValue<T?>();
                } 
                
                if(onMissingProperty != null && result == null)
                {
                    throw new Exception($"Invalid Property... JSON Property `{ name }` is missing or invalid");
                }
            } catch (Exception ex){                
                onMissingProperty?.Invoke(ex, ex.Message);
                throw;
            }

            return result;
        }
    }
}