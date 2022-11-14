using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.DocsControl
{
    public class DocsApprover : BaseBO
    {
        public int DocFlowID { get; set; }
        public int ApproverID { get; set; }
        public string Sequence { get; set; }
        public string Name { get; set; }
        public int Action { get; set; }
    }
}
