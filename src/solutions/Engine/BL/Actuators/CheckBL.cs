using Engine.BO.AccessControl;
using Engine.BO;
using Engine.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.DAL;
using Engine.BO.FlowControl;

namespace Engine.BL.Actuators
{
    public class CheckBL : BaseBL<AccessControlDAL>
    {
        public List<Check> GetChecks(int? checkId = null, int? employeeId = null) => Dal.GetChecks(checkId, employeeId);

        public Check? GetCheck(int id) => GetChecks(id).FirstOrDefault();

        public ResultInsert SetCheck(Check check, int employeeId) => Dal.SetCheck(check, employeeId, C.GLOBAL_USER);
        
        public List<CheckDetails> GetCheckDetails(DateTime from, DateTime to) => Dal.GetCheckDetails(from, to);
    }
}
