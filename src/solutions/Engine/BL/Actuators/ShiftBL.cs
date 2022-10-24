using Engine.BO;
using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BL.Actuators
{
    public class ShiftBL : BaseBL<AccessControlDAL>
    {
        public List<Shift> GetShifts(int? shiftId = null) => Dal.GetShifts(shiftId);

        public Shift? GetShift(int id) => Dal.GetShifts(id).FirstOrDefault();

        public ResultInsert SetShift(Shift shift, string txnUser) => Dal.SetShift(shift, txnUser);        
    }
}
