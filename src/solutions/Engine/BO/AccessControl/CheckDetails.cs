using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class CheckDetails : CheckBase
    {
        public Position? Position { get; set; }


        public static List<IntervalDepto> GetChecksByDepto(List<CheckDetails> checks, DateTime pivot)
        {
            List<IntervalDepto> intervalDeptos = new();

            List<Period> periods = new()
            {
                new Period()
                {
                    From = pivot.AddHours(6),
                    To = pivot.AddHours(10)
                },
                new Period()
                {
                    From = pivot.AddHours(10),
                    To = pivot.AddHours(16)
                },
                new Period()
                {
                    From = pivot.AddHours(16),
                    To = pivot.AddHours(20)
                }
            };

            foreach (var x in checks.GroupBy(x => x.Position.Department.Code))
            {
                var list = x.ToList();
                var deptoInterval = new IntervalDepto()
                {
                    Name = x.Key
                };

                foreach (var p in periods)
                {
                    DeptoCounter deptoStats = new();

                    var periodChecks = list.Where(
                        x => p.IsRange((DateTime)x.CheckDt)
                    ).ToList();

                    if (periodChecks.Count > 0)
                    {
                        var depto = periodChecks[0].Position?.Department;

                        if (depto != null)
                        {
                            deptoStats.Department = (Department)depto;
                        }

                        deptoStats.Period = p;                        
                        deptoStats.Checks = periodChecks.Count;
                    }

                    deptoInterval.Sets.Add(deptoStats);
                }

                intervalDeptos.Add(deptoInterval);
            }

            return intervalDeptos;
        }

    }
}
