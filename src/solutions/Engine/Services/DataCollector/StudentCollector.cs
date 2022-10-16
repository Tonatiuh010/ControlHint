using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.BO;
using Engine.Interfaces.DataCollector;

namespace Engine.Services.DataCollector
{
    public class StudentCollector : IStudentCollector
    {
        public static IStudentCollector.SearchStudent SearchStudent { get; set; } = id => throw new NotImplementedException();
        public static IStudentCollector Instance(int id) => new StudentCollector(SearchStudent, id);

        public int Id { get; set; }
        public IStudentCollector.SearchStudent GetStudent { get; }

        private StudentCollector(IStudentCollector.SearchStudent callback, int id) { 
            GetStudent = callback;
            Id = id;
        }
    }
}
