using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Constants;
using Engine.Interfaces;

namespace Engine.Services
{
    public class ClassroomCredentials : IConectionString
    {
        public static IConectionString.SearchConnection ConnectionCallback { get; set; } 
            = () => string.Empty;

        public static ClassroomCredentials Instance 
            => new(ConnectionCallback);

        public IConectionString.SearchConnection GetConnection { get; }

        private ClassroomCredentials(IConectionString.SearchConnection callback) 
            => GetConnection = callback;
    }
}
