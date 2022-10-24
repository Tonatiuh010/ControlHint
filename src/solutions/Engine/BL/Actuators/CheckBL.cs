using Engine.BO.AccessControl;
using Engine.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.DAL;

namespace Engine.BL.Actuators
{
    public class CheckBL : BaseBL<AccessControlDAL>
    {
        public List<Check> GetChecks(int? checkId = null, int? employeeId = null) => Dal.GetChecks(checkId, employeeId);

        public Check? GetCheck(int id) => GetChecks(id).FirstOrDefault();

        public ResultInsert SetCheck(int? device, string txnUser) => Dal.SetCheck(device, txnUser);        
        
        public List<CheckDetails> GetCheckDetails(DateTime from, DateTime to) => Dal.GetCheckDetails(from, to);
    }
}
