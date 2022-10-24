using Engine.BO.AccessControl;
using Engine.BO;
using Engine.Constants;
using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BL.Actuators
{
    public class EmployeeBL : BaseBL<AccessControlDAL>
    {
        public List<Employee> GetEmployees(int? employeeId = null)
        {
            List<Employee> employees = new();
            employees = Dal.GetEmployees(employeeId);

            foreach (var employee in employees)
            {
                employee.AccessLevels = EmployeeAccessLevel.GetAccessLevels(
                    GetEmployeeAccessLevels(employee.Id)
                );

                if (employee.Image != null)
                    employee.Image.Url = String.Empty;
            }


            return employees;
        }

        public Employee? GetEmployee(int? id) => GetEmployees(id).FirstOrDefault();

        public ResultInsert SetEmployee(Employee employee, string txnUser) => Dal.SetEmployee(employee, txnUser);

        public Result SetEmployeeAccessLevel(int employeeId, int accessLevelId, bool status, string txnUser) => Dal.SetEmployeeAccessLevel(employeeId, accessLevelId, status ? C.ENABLED : C.DISABLED, txnUser);

        public Result SetDownEmployee(int employeeId, string txnUser) => Dal.SetDownEmployee(employeeId, txnUser);

        public List<EmployeeAccessLevel> GetEmployeeAccessLevels(int? employeeId) => Dal.GetEmployeeAccessLevels(employeeId);        
        
    }
}
