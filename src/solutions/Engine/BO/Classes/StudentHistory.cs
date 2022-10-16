using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.BO;
using Engine.Interfaces;
using Engine.Interfaces.DataCollector;
using Engine.Services;
using Engine.Services.DataCollector;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Engine.BO
{
    public class StudentHistory : BaseBO
    {
        private IStudentCollector StudentService => StudentCollector.Instance(StudentId);

        public StudentHistory(int studentId)
        {
            StudentId = studentId;
            Score = string.Empty;
        }

        public StudentHistory()
        {
            Score = string.Empty;
            ScoreDate = null;
            Group = null;
            SubGroup = null;
            Value = null;
            ValueAlt = null;
        }

        private int StudentId { get; set; }
        public string Score { get; set; }
        public DateTime? ScoreDate { get; set; }
        public string? Group { get; set; }
        public string? SubGroup { get; set; }
        public string? Value { get; set; }
        public string? ValueAlt { get; set; }

        public double GetScore()
        {
            try
            {                
                return double.Parse(Score);
            } catch
            {
                return 0;
            }
        }

        [JsonIgnore]
        public Student? Student => StudentService.Student;
    }
}
