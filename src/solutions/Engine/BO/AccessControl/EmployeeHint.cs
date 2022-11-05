using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class EmployeeHint : BaseBO
    {
        public ImageData ImageData { get; set; } = new ImageData();
        public Employee Employee { get; set; } = new();
    }
}
