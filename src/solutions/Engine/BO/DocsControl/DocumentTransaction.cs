using Engine.BO.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.DocsControl
{
    public class DocumentTransaction
    {
        public Document? Document { get; set; }
        public List<ApproverStep>? Approvers { get; set; }
    }

    public class ApproverStep
    {
        public DocApprover? DocumentDetail { get; set; }
        public string? Status { get; set; }
        public string? Depto { get; set; }
        public string? Comments { get; set; }

    }
}
