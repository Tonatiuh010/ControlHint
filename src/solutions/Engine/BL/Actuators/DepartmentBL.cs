using Engine.BO.AccessControl;
using Engine.BO;
using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BL.Actuators
{
    public class DepartmentBL : BaseBL<AccessControlDAL>
    {
        public List<Department> GetDepartments(int? deptoId = null) => Dal.GetDepartments(deptoId);

        public Department? GetDepartment(int id) => GetDepartments(id).FirstOrDefault();

        public ResultInsert SetDepartament(Department departament, string txnUser) => Dal.SetDepartment(departament, txnUser);
    }
}
