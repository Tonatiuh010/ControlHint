using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.DocsControl
{
    public class Approver : BaseBO
    {
        public string? FullName { get; set; }
        public int PositionID { get; set; }
        public int DeptoID { get; set; }
    }
}
