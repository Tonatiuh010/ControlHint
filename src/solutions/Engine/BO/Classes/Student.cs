using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Engine.Interfaces.DataCollector;
using Engine.Services.DataCollector;
using System.Text.Json.Nodes;

namespace Engine.BO
{
    public class Student : BaseBO
    {

        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }

        [JsonIgnore]
        public byte[]? Image { get; set; }

        [JsonPropertyName("image")]
        public string ImageUrl { get; set; } = string.Empty;

        public DateTime? Birth { get; set; }

        public int? Age { get {
                if (Birth.HasValue)
                {
                    var today = DateTime.Today;
                    return today.Year - Birth.Value.Year;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Location? BirthPlace { get; set; }
        public Asset? Genre { get; set; }
        public Asset? Marital { get; set; }
        public Contact? Contact { get; set; }
        public Address? Address { get; set; }
        public Scholarly? Scholarly { get; set; }
        public Labor? Labor { get; set; }

    }

    public class StudentExt : Student 
    {
        private IHistorialCollector ServiceHistorial => HistorialCollector.Instance(Id);
        private IContactFamilyCollector ServiceContact => ContactFamilyCollector.Instance(Id);
        private List<StudentHistory> Historial => ServiceHistorial.Historial;
        private List<ItemStats> HistorialStats => ServiceHistorial.Stats;
        public JsonObject Stats => ItemStats.ToJsonObject( HistorialStats );
        public List<ContactFamily> Contacts => ServiceContact.Contacts;
        public Student GetBase() => this;
    }

}
