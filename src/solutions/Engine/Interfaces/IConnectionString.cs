using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Interfaces
{
    public interface IConnectionString
    {
        public delegate string SearchConnection();

        public string? Name { get; set; }
        public SearchConnection GetConnection { get; }
        public string ConnectionString => GetConnection();

    }
}
