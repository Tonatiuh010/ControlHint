using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class EmployeeAccessLevel : AccessLevel
    {
        public int? EmployeeId { get; set; }
        public bool IsValidEmployeeAccessLevel() => IsValid() && EmployeeId != null && EmployeeId != 0;

        public static List<AccessLevel> GetAccessLevels(List<EmployeeAccessLevel> levels) =>
            levels.Select(x => (AccessLevel)x).ToList();
    }
}
