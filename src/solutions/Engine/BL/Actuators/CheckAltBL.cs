using Engine.BO;
using Engine.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.DAL;
using Engine.BO.AccessControl;

namespace Engine.BL.Actuators
{
    public class CheckAltBL : BaseBL<AccessControlDAL>
    {
        public ResultInsert SetCheckAlt(CheckAlt checkAlt) => Dal.SetCheckAlt(checkAlt, C.GLOBAL_USER);
    }
}
