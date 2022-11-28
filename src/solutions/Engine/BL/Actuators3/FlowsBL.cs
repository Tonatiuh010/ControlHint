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
    public class FlowsBL : BaseBL<DocsControlDAL>
    {
        public ResultInsert SetDocFlow(DocFlow DocFlowId) => Dal.SetDocFlow(DocFlowId, C.GLOBAL_USER);
        public List<DocFlow> GetDocFlows(int? id = null) => Dal.GetDocFlows(id);
        public DocFlow? GetDocFlow(int id) => GetDocFlows(id).FirstOrDefault();
    }
}
