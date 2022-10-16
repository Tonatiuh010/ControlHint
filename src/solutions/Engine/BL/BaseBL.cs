using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.DAL;
using DataService.MySQL;
using D = Engine.BL.Delegates;
using Engine.Services;

namespace Engine.BL
{
    public abstract class BaseBL
    {
        protected static ClassroomDAL Dal => ClassroomDAL.Instance;        

        public static void SetErrorsCallback(MySqlDataBase.DataException onConnectionError)
        {
            DAL.Classes.DAL.OnDALError = ExceptionManager.CallbackException;
            MySqlDataBase.OnException = onConnectionError;
        }      

    }
}
