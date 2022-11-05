using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Engine.Constants;
using Engine.Interfaces;

namespace Engine.Services
{
    public class ConnectionString : IConnectionString
    {
        #region Static Methods 
        private static ConnectionString _Instance { get; set; } = new (() => string.Empty);
        private static List<ConnectionString> ConnectionStrings { get; set; } = new();        

        public static ConnectionString Instance => _Instance;
        public static ConnectionString? InstanceByName(string name) => FindConnection(name);
        public static ConnectionString InstanceName(IConnectionString.SearchConnection callback, string name) => new(callback, name);

        public static void AddConnectionString(IConnectionString connectionService)
        {
            int index = ConnectionStrings.FindIndex(x => x.Name == connectionService.Name);
            ConnectionString conn = new(connectionService);

            if (index != -1)
            {
                ConnectionStrings[index] = conn;
            }
            else
            {
                ConnectionStrings.Add(conn);
            }
        }

        public static ConnectionString? FindConnection(string name)
        {
            if (Instance.Name == name)
                return Instance;

            return ConnectionStrings.Find(x => x.Name == name);
        }
        
        public static void SetConnectionString(IConnectionString.SearchConnection callback, string name) 
        {
            var connectionString = new ConnectionString (callback, name);
            _Instance = connectionString;
            AddConnectionString(connectionString);
        }

        #endregion

        #region Public
        public string? Name { get; set; }
        public IConnectionString.SearchConnection GetConnection { get; set; }

        private ConnectionString(IConnectionString.SearchConnection callback) => GetConnection = callback;

        private ConnectionString(IConnectionString connection)
        {            
            Name = connection.Name;
            GetConnection = connection.GetConnection;
        }

        private ConnectionString(IConnectionString.SearchConnection callback, string name) {
            GetConnection = callback; 
            Name = name;
        }
        #endregion        

    }
}
