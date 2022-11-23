using Engine.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class WeeklyChecks
    {
        public Employee Employee { get; set; }
        public List<DayCheck> Checks { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Interval => GetInterval();

        public WeeklyChecks(Employee employee, List<CheckBase> checks)
        {
            Employee = employee;
            From = checks.Min(x => x.CheckDt);
            To = checks.Max(x => x.CheckDt);
            Checks = ParseChecks(checks);
        }

        private string GetInterval()
        {
            string intervalMsg = string.Empty;

            if (From != null)
            {
                intervalMsg += $"{From?.ToString("d")}";
            }

            if (To != null)
            {
                intervalMsg += $" - {To?.ToString("d")}";
            }

            return intervalMsg;
        }

        private static List<DayCheck> ParseChecks(List<CheckBase> checks) 
        {
            List<DayCheck> model = new List<DayCheck>();

            try {
                int counter = 0;
                DateTime firstDay = Utils.StartOfWeek( (DateTime)checks.Min(x => x.CheckDt), DayOfWeek.Sunday);
                List<DayOfWeek> days = new List<DayOfWeek>()
                {
                    DayOfWeek.Sunday,
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday
                };

                foreach(var day in days)
                {
                    DateTime dt = firstDay.AddDays(counter);
                    List<CheckBase>? _dayChecks = checks.FindAll(x => x.CheckDt?.DayOfWeek == day).ToList();
                    var inCheck = GetChecksMin(_dayChecks);
                    var outCheck = GetChecksMax(_dayChecks);

                    model.Add(new DayCheck(  
                        dt.Date,
                        inCheck,
                        outCheck,
                        _dayChecks.Count
                    ));

                    counter++;
                }

            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return model;
        }

        private static CheckBase? GetChecksMin(List<CheckBase> checks) 
        {
            try
            {
                return checks.MinBy(x => x.CheckDt);
            } catch
            {
                return null;
            }
        }

        private static CheckBase? GetChecksMax(List<CheckBase> checks)
        {
            try
            {
                return checks.MaxBy(x => x.CheckDt);
            }
            catch
            {
                return null;
            }
        }

    }
    
    public class DayCheck
    {
        public DateTime Dt { get; set; }
        public string Day => Dt.DayOfWeek.ToString();
        public CheckBase? In { get; set; }
        public CheckBase? Out { get; set; }
        public int Counter { get; set; }

        public DayCheck(DateTime dt, CheckBase? @in, CheckBase? @out, int counter)
        {
            Dt = dt;
            In = @in;
            Out = @out;
            Counter = counter;
        }

        

    }

}
