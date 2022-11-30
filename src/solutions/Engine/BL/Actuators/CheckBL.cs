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
using Engine.BL.Actuators2;

namespace Engine.BL.Actuators
{
    public class CheckBL : BaseBL<AccessControlDAL>
    {
        private readonly DeviceBL bl = new DeviceBL();
        private readonly EmployeeBL blEmployee = new EmployeeBL();

        public List<Check> GetChecks(int? checkId = null, int? employeeId = null) 
        {
            var checks = Dal.GetChecks(checkId, employeeId);

            foreach (var check in checks)
                CompleteDevice(check);

            return checks;
        }

        public Check? GetCheck(int id) => GetChecks(id).FirstOrDefault();

        public void CompleteDevice(Check check)
        {
            if(check.Device != null && check.Device.IsValid())
            {
                check.Device = bl.GetDevice((int)check?.Device?.Id);
            }

            if(check.Employee != null && check.Employee.IsValid())
            {
                check.Employee = blEmployee.GetEmployee((int)check?.Employee?.Id);
            }
        }

        public ResultInsert SetCheck(Check check) => Dal.SetCheck(check, C.GLOBAL_USER);
        
        public List<CheckDetails> GetCheckDetails(DateTime from, DateTime to) => Dal.GetCheckDetails(from, to);
        public ResultInsert SetCheckEmployee(Check check) => Dal.SetCheckEmlpoyee(check, C.GLOBAL_USER);
    }
}
