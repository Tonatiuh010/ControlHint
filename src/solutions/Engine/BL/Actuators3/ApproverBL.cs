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
    }
}
