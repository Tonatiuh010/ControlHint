using Engine.BO;
using Engine.BO.DocsControl;
using Engine.Constants;
using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BL.Actuators3
{
    public class ApproverBL : BaseBL<DocsControlDAL>
    {
        public ResultInsert SetApprover(Approver approver) => Dal.SetApprover(approver, C.GLOBAL_USER);

        public List<Approver> GetApprovers(int? id = null) => Dal.GetApprovers(id);
        public Approver? GetApprover(int id) => GetApprovers(id).FirstOrDefault();

        public List<DocsApprover> GetDocsApprovers(int? id = null) => Dal.GetFlowsApprover(id);
        public DocsApprover? GetDocApprover(int id) => GetDocsApprovers(id).FirstOrDefault();
    }
}
