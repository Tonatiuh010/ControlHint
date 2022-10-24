using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class Position : Job
    {
        public int? PositionId { get; set; }
        public string? Alias { get; set; }
        public Department? Department { get; set; }

        public bool IsValidPosition() => IsValid() && PositionId != null && PositionId != 0;
    }
}
