using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Services.DataCollector;
using Engine.BL;
using Engine.DAL;
using Engine.Services;
using DataService.MySQL;

namespace Engine.BL
{
    public static class BinderBL
    {
        public static void Start()
        {
            BaseDAL.OnDALError = ExceptionManager.CallbackException;
        }

        public static void SetDalError(MySqlDataBase.DataException onConnectionError)
        {
            MySqlDataBase.OnException = onConnectionError;
        }
    }
}
