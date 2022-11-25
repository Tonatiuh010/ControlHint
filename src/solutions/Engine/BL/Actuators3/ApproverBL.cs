using Engine.BL.Actuators;
using Engine.BO;
using Engine.BO.AccessControl;
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
        private PositionBL BL = new PositionBL();
        
        public ResultInsert SetApprover(Approver approver, string txnUser) => Dal.SetApprover(approver, txnUser);

        public List<Approver> GetApprovers(int? approverid = null) {
            var Approvers = Dal.GetApprovers(approverid);
            foreach (var approver in Approvers) { 
                CompleteApprover (approver);
            }
            return Approvers;
                }
        public Approver? GetApprover(int approver) => GetApprovers(approver).FirstOrDefault();

        public List<DocsApprover> GetDocsApprovers(int? id = null) => Dal.GetFlowsApprover(id);
        public DocsApprover? GetDocApprover(int id) => GetDocsApprovers(id).FirstOrDefault();
        private void CompleteApprover (Approver approver)
        {
            if(approver.Position != null && approver.Position.IsValidPosition())
            {
                approver.Position = BL.GetPosition((int)approver.Position.PositionId);
            }
        }
    }
}
