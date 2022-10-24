using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class Department : BaseBO
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
    }

    public class IntervalDepto
    {
        public string? Name { get; set; }
        public List<DeptoCounter> Sets { get; set; } = new();

        public Period Period => new()
        {
            From = Sets.Min(x => x.Period.From),
            To = Sets.Max(x => x.Period.To)
        };
    }

    public class DeptoCounter
    {
        public Department? Department { get; set; }
        public Period Period { get; set; } = new();
        public int Checks { get; set; }
    }

}
