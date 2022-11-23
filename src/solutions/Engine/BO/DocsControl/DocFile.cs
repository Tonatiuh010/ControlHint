using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.DocsControl
{
    public class DocFile : BaseBO
    {
        public Document? Document { get; set; }
        public byte[]? DocImg { get; set; }

    }
}
