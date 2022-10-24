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
    public class AccessLevelBL : BaseBL<AccessControlDAL>
    {
        public List<AccessLevel> GetAccessLevels() => Dal.GetAccessLevels();

        public AccessLevel? GetAccessLevel(int id) => GetAccessLevels().Find(x => x.Id == id);

        public AccessLevel? GetAccessLevel(string name) => GetAccessLevels().Find(x => x.Name == name);

        public ResultInsert SetAccessLevel(AccessLevel level, string txnUser) => Dal.SetAccessLevel(level, txnUser);        
    }
}
