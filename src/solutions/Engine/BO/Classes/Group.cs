using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Engine.Services.DataCollector;
using Engine.Interfaces.DataCollector;
using System.Text.Json.Nodes;

namespace Engine.BO
{
    public class Group : BaseBO
    {
        private static IGroupStatsCollector ServiceStats => GroupStatsCollector.Instance;
        private static List<ItemStats> Historial => ServiceStats.Stats;

        private IGroupStudentsCollector ServiceGroup => GroupStudentsCollector.Instance(Id);        

        public string? Code { get; set; }
        public string? Key { get; set; }
        public int? Quarter { get; set; }
        public string? Description { get; set; }
        public Major? Major { get; set; }
        public Asset? Field { get; set; }
        public Asset? Period { get; set; }                
        public List<StudentExt>? Students => ServiceGroup.Students;        
        public JsonObject Stats => ItemStats.ToJsonObject( Historial );

        public JsonObject? Grades
        {
            get
            {
                JsonObject? model = null;
                var students = Students;

                if ( students != null && students.Count > 0 )
                {
                    var stats = students.Select(x => x.Stats).ToList();
                               
                    List<double> tsuGrades = new();
                    List<double> engGrades = new();

                    foreach (var s in stats)
                    {
                        var tsu = s["tsu"];
                        var eng = s["eng"];
                        
                        if (tsu != null )
                        {
                            var tsuG = (JsonArray)tsu["tsuGrades"];
                            for(int i = 0; i < tsuG.Count; i++)
                            {
                                var d = double.Parse(tsuG[i].ToString());
                                if (tsuGrades.Count> i )
                                {
                                    tsuGrades[i] += d;
                                }
                                else
                                {
                                    tsuGrades.Add(d);
                                }
                            }                                        
                            
                        }

                        if (eng != null)
                        {
                            var engG = (JsonArray)eng["engGrades"];
                            for (int i = 0; i < engG.Count; i++)
                            {
                                var d = double.Parse(engG[i].ToString());
                                if (engGrades.Count> i)
                                {
                                    engGrades[i] += d;
                                }
                                else
                                {
                                    engGrades.Add(d);
                                }
                            }                                
                        }                        
                    }

                    for(int i = 0; i < tsuGrades.Count; i++)
                    {
                        tsuGrades[i] /= students.Count;                        
                    }

                    for (int i = 0; i < engGrades.Count; i++) { 
                        engGrades[i] /= students.Count; 
                    }

                    model = ItemStats.ToJsonObject( new List<ItemStats>() {
                        new (
                            "tsu",
                            ItemStats.ToJsonObject( new List<ItemStats>() {                                
                                new ("tsuGrades", tsuGrades),
                                new ("tsuAvg", tsuGrades.Average()),
                            })
                        ),
                        new (
                            "eng",
                            ItemStats.ToJsonObject( new List<ItemStats>() {
                                new ("engGrades", engGrades),
                                new ("engAvg", engGrades.Average())
                            })
                        ),

                    });

                }

                return model;
            }
        }

    }
}
