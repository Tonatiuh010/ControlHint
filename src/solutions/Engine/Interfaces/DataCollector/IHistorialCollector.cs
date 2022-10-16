using Engine.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Interfaces.DataCollector
{
    public interface IHistorialCollector
    {
        public delegate List<StudentHistory> SearchHistorial(int studentId);
        public delegate List<ItemStats> SearchStats(int studentId);

        public int StudentId { get; set; }
        public SearchHistorial GetHistorial { get; }
        public SearchStats GetStats { get; }
        public List<StudentHistory> Historial => GetHistorial(StudentId);
        public List<ItemStats> Stats => GetStats(StudentId);
        
    }
}
