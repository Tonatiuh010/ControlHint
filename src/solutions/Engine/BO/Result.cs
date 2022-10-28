using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Engine.BL;
using System.Collections;
using System.Reflection;

namespace Engine.BO {
    public class Result {
        public string? Status {get; set;}
        public string? Message {get; set;}
        public object? Data {get; set;} = null;
        public object? Data2 {get; set;} = null;
        public object? Data3 { get; set; } = null;
        public object? Data4 { get; set; } = null;

    }

    public class ResultInsert : Result
    {
        public InsertStatus? InsertDetails { get; set; } = new InsertStatus(new BaseBO());
    }

    public class InsertStatus : BaseBO
    {
        private BaseBO Base { get; set; }

        public string ObjectType => FromObject != null? FromObject.ToString() : "NOT ASSOCIATED OBJECT";
        [JsonIgnore]
        public Type FromObject { get => FromObject.GetType(); } 
        public DateTime InsertDate { get; set; }

        public InsertStatus(BaseBO baseBO)
        {
            Id = baseBO.Id;
            Base = baseBO;            
            InsertDate = DateTime.Now;
        }

        public InsertStatus(int id, BaseBO @base)
        {
            Id = id;
            Base = @base;            
            InsertDate = DateTime.Now;
        }

        public T CastObject<T>() where T : BaseBO, new () {      
     
            if (typeof(T).FullName == FromObject.FullName)
            {
                return (T)Base;
            }            
            
            return new T();            
        }
    }
}