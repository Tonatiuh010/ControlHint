using Engine.BO;
using Engine.Constants;
using Engine.BO.FlowControl;
using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BL.Actuators2
{
    public class UserBL : BaseBL<FlowControlDAL>
    {
        public List<User> GetUsers(int? employeeId = null, string? userName = null) => Dal.GetUsers(employeeId, userName);

        public User? GetUser(int employeeId) => GetUsers(employeeId).FirstOrDefault();

        public User? GetUser(string user) => GetUsers(userName: user).FirstOrDefault();

        public ResultInsert SetUser(User user) => Dal.SetUser(user, C.GLOBAL_USER);
    }
}
