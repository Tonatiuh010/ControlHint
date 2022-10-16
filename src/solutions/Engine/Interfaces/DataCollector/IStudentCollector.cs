using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.BO;

namespace Engine.Interfaces.DataCollector
{
    public interface IStudentCollector
    {
        public delegate Student? SearchStudent(int id);

        public int Id { get; set; }
        public SearchStudent GetStudent { get; }
        public Student? Student => GetStudent?.Invoke(Id);
        
    }
}
