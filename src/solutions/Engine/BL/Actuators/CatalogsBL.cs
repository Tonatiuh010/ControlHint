using Engine.BO;
using Engine.DAL;
using Google.Protobuf.WellKnownTypes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Engine.BL
{
    public class CatalogsBL : BaseBL
    {        

        public static List<Address> GetAddresses(int? id = null)
        {
            var addresses = Dal.GetAddresses(id);

            foreach (var a in addresses)
                CompleteAddress(a);
            
            return addresses;
        }

        public static List<Asset> GetAssets(int? id = null, string? code = null, string? group = null)
            => Dal.GetAssets(id, code, group);

        public static List<Contact> GetContacts(int? id = null)
            => Dal.GetContacts(id);

        public static List<Labor> GetLabors(int? id = null)
            => Dal.GetLabors(id);        

        public static List<Location> GetLocations(int? id = null)
            => Dal.GetLocations(id);

        public static List<Major> GetMajors(int? id = null)
            => Dal.GetMajors(id);

        public static List<Scholarly> GetScholars(int? id = null)
            => Dal.GetScholarls(id);

        public static List<ContactFamilyStudent> GetContactFamilies(int? id = null, int? contactFamilyId = null, int? contactId = null )
        {
            var contactFamilies = Dal.GetFamilyContacts(id, contactFamilyId, contactId);

            foreach (var cf in contactFamilies)
                CompleteContactFamily(cf);                            

            return contactFamilies;
        }

        public static Address? GetAddress(int id)
           => GetAddresses(id: id).FirstOrDefault();

        public static Asset? GetAsset(int id)
            => GetAssets(id: id).FirstOrDefault();

        public static Asset? GetAsset(string code)
            => GetAssets(code: code).FirstOrDefault();

        public static Contact? GetContact(int id)
            => GetContacts(id: id).FirstOrDefault();

        public static ContactFamily? GetContactFamily(int id)
            => GetContactFamilies(id: id).FirstOrDefault();

        public static Labor? GetLabor(int id)
            => GetLabors(id: id).FirstOrDefault();

        public static Location? GetLocation(int id)
            => GetLocations(id: id).FirstOrDefault();

        public static Major? GetMajor(int id)
            => GetMajors(id: id).FirstOrDefault();

        public static Scholarly? GetScholarly(int id)
            => GetScholars(id: id).FirstOrDefault();

        public static JsonArray GetFullAsset()
        {
            var keys = Dal.GetAssetKeys();
            JsonArray list = new();

            foreach (var k in keys)
            {
                var assets = GetAssets(group: k);

                if (assets != null && assets.Count > 0)
                {
                    var json = JsonSerializer.Serialize(assets);
                    list.Add(new JsonObject()
                    {
                        [k] = JsonNode.Parse( json )
                    });
                }
            }

            return list;
        }

        private static void CompleteAddress(Address? a)
        {
            if(a != null && a.IsValid() && a.Location != null)
            {
                a.Location = GetLocation(a.Location.Id);
            }
        }

        private static void CompleteContactFamily(ContactFamily? c)
        {
            if(c != null && c.IsValid() && c.Contact != null && c.Contact.IsValid())
            {
                c.Contact = GetContact(c.Contact.Id);
            }
        }

    }
}
