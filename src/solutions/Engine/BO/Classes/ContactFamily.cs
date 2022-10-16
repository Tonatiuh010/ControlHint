using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Services.DataCollector;
using Engine.Interfaces.DataCollector;
using System.Text.Json.Serialization;

namespace Engine.BO
{    
    public class ContactFamily : BaseBO 
    {
        public Contact? Contact { get; set; }
        public string? Name { get; set; }
        public string? Kinship { get; set; }
        public string? Work { get; set; }
        public bool? IsEmergency { get; set; }

        public ContactFamily()
        {
            Contact = null;
            Name = string.Empty;
            Kinship = string.Empty;
            Work = string.Empty;
        }
    }

    public class ContactFamilyStudent : ContactFamily
    {
        private IStudentCollector StudentService => StudentCollector.Instance(StudentId);
        private int StudentId { get; set; }
        public Student? Student => StudentService.Student;
        public ContactFamilyStudent(int studentId) => StudentId = studentId;
        public ContactFamily GetBase() => this;
    }

}
