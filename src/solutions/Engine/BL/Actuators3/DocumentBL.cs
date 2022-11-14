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
    public class DocumentBL : BaseBL<DocsControlDAL>
    {
        public ResultInsert SetDocument(Document document, string txnUser) => Dal.SetDocument(document, txnUser);
    }
}
