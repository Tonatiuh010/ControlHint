using Engine.BO.DocsControl;
using Engine.BO;
using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BL.Actuators3
{
    public class DocFlowBL : BaseBL<DocsControlDAL>
    {
        public ResultInsert SetDocFlow(DocFlow docFlow, string txnUser) => Dal.SetDocFlow(docFlow, txnUser);
    }
}
