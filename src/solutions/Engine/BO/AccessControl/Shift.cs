using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;
using Engine.Constants;

namespace Engine.BO {    

    public class Shift : BaseBO {
        public string? Name {get; set;}
        public TimeSpan? InTime {get; set;}
        public TimeSpan? OutTime {get; set;}
        public TimeSpan? LunchTime {get; set;}
        public int? DayCount {get; set;}

        public Shift() {
            Id = 0;
            Name = null;
            InTime = null;
            OutTime = null;
            LunchTime = null;
            DayCount = null;
        }

        public Shift(string? inTime, string? outTime) {
            InTime = ConvertTime(inTime);
            OutTime = ConvertTime(outTime);
        }

        public static TimeSpan ConvertTime(string? timeExpression) {
            try {
                if(timeExpression != null) {
                    return TimeSpan.Parse(timeExpression);
                } else {
                    return new TimeSpan();
                }
            } catch {
                return new TimeSpan();
            }
        }
    }

}