using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.DocsControl
{
    public class Document : BaseBO
    {
        public string? Name { get; set; }
        public DocType? DocType { get; set; }
    }
}
