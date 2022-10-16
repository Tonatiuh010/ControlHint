using Engine.Interfaces.DataCollector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Services.DataCollector
{
    public class HistorialCollector : IHistorialCollector
    {
        public static IHistorialCollector.SearchHistorial SearchHistorial { get; set; } = id => throw new NotImplementedException();
        public static IHistorialCollector.SearchStats SearchStats { get; set; } = id => throw new NotImplementedException();
        public static IHistorialCollector Instance(int studentId) => new HistorialCollector(SearchHistorial, SearchStats, studentId);

        public int StudentId { get; set; }
        public IGroupStudentsCollector.SearchStudents? GetStudents { get; }        
        public IHistorialCollector.SearchHistorial GetHistorial { get; }
        public IHistorialCollector.SearchStats GetStats { get; }

        private HistorialCollector(IHistorialCollector.SearchHistorial callback, IHistorialCollector.SearchStats callback2 , int studentId)
        {
            StudentId = studentId;
            GetHistorial = callback;
            GetStats = callback2;
        }
    }
}
