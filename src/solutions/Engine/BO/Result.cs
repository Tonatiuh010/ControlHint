using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Engine.BL;
using Engine.Constants;
using System.Collections;
using System.Reflection;

namespace Engine.BO {
    public class Result {
        public string? Status {get; set;}
        public string? Message { get; set;}
        public object? Data { get; set;} = null;
        public object? Data2 { get; set;} = null;
        public object? Data3 { get; set; } = null;
        public object? Data4 { get; set; } = null;

    }

    public class ResultInsert : Result
    {
        public InsertStatus? InsertDetails { get; set; } = new InsertStatus(new BaseBO());

        public object? GetInsertOrError() => Status != C.OK ? Data : InsertDetails;
    }

    public class InsertStatus : BaseBO
    {
        public object InsertedObject { get; }
        private BaseBO Base { get; set; }

        public string ObjectType => FromObject != null? FromObject.ToString() : "NOT ASSOCIATED OBJECT";
        [JsonIgnore]
        public Type FromObject => Base.GetType();
        public DateTime InsertDate { get; set; }

        public InsertStatus(BaseBO baseBO)
        {
            Id = baseBO.Id;
            Base = baseBO;            
            InsertDate = DateTime.Now;
            InsertedObject = baseBO;
        }

        public InsertStatus(int id, BaseBO @base)
        {
            @base.Id = id;

            Id = id;
            Base = @base;            
            InsertDate = DateTime.Now;
            InsertedObject= @base;
        }

        public T CastObject<T>() where T : BaseBO, new () {      
     
            if (typeof(T).FullName == FromObject.FullName)
            {
                return (T)InsertedObject;
            }            
            
            return new T();            
        }
    }
}