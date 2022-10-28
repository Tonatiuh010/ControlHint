using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.FlowControl
{
    public class Step : BaseBO
    {

        public int Sequence { get; set; }
        public bool IsRequired { get; set; }
        public string? Description { get; set; }        
        public Endpoint Endpoint { get; set; }        

        public Step()
        {
            Endpoint = new Endpoint();
        }
    }
}
