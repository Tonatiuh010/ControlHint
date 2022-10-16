using Engine.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Interfaces.DataCollector
{
    public interface IContactFamilyCollector
    {
        public delegate List<ContactFamily> SearchContactFamily(int studentId);

        public int StudentId { get; set; }
        public SearchContactFamily GetContacts { get; }
        public List<ContactFamily> Contacts => GetContacts(StudentId);
    }
}
