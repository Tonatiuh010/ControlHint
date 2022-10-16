using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Interfaces
{
    public interface IConectionString
    {
        public delegate string SearchConnection();

        public SearchConnection GetConnection { get; } 
        public string ConnectionString => GetConnection();    

    }
}
