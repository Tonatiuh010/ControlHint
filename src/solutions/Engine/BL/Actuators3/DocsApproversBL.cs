using Engine.BO.DocsControl;
using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BL.Actuators3
{
    public class DocsApproversBL : BaseBL<DocsControlDAL>
    {
        public List<DocsApprover> GetDocsApprovers(int id) => Dal.GetFlowsApprover(id);
        public DocsApprover? GetDocApprover(int id) => GetDocsApprovers(id).FirstOrDefault();
    }
}
