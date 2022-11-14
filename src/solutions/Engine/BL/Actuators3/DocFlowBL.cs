using Engine.BO.DocsControl;
using Engine.BO;
using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Constants;

namespace Engine.BL.Actuators3
{
    public class DocFlowBL : BaseBL<DocsControlDAL>
    {
        public ResultInsert SetDocFlow(DocFlow docFlow) => Dal.SetDocFlow(docFlow, C.GLOBAL_USER);
    }
}
