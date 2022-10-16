using Engine.Interfaces.DataCollector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Services.DataCollector
{
    public class ContactFamilyCollector : IContactFamilyCollector
    {
        public static IContactFamilyCollector.SearchContactFamily SearchContactFamily { get; set; } = id => throw new NotImplementedException();
        public static IContactFamilyCollector Instance(int studentId) => new ContactFamilyCollector(SearchContactFamily, studentId);

        public int StudentId { get; set; }
        public IContactFamilyCollector.SearchContactFamily GetContacts { get; }

        private ContactFamilyCollector(IContactFamilyCollector.SearchContactFamily callback, int studentId)
        {
            StudentId = studentId;
            GetContacts = callback;
        }
    }
}
