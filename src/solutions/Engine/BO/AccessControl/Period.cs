using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.AccessControl
{
    public class Period
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public bool IsRange(DateTime dt) => dt >= From && dt <= To;
    }
}
