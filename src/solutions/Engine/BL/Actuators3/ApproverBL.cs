using Engine.BO;
using Engine.BO.DocsControl;
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
        public ResultInsert SetApprover(Approver approver, string txnUser) => Dal.SetApprover(approver, txnUser);
    }
}
