using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Interfaces
{
    public interface IHttpContentConverter
    {
        public object Params { get; }
        public HttpContent GetContent();
    }
}
