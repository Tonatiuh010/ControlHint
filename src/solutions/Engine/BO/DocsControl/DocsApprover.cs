using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.DocsControl
{
    public class DocsApprover : BaseBO
    {
        public DocFlow? DocFlow { get; set; }
        public Approver? Approver { get; set; }
        public int Sequence { get; set; }
        public string? Name { get; set; }
        public int Action { get; set; }
    }
}
