using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.DAL;
using DataService.MySQL;
using Engine.Services;
using Engine.Interfaces;

namespace Engine.BL
{
    public class BaseBL <T> where T : BaseDAL, new()
    {        
        protected T Dal => new();        

        public BaseBL()
        {
            
        }

    }
}
