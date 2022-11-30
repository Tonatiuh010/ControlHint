using Engine.BL;
using Engine.Constants;
using Engine.DAL;
using Engine.Interfaces;
using Engine.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace Base
{
    public static class Builder
    {
        public static string URL { get; set; } = string.Empty;

        public static void Build(
            List<string> connections,
            IConfigurationRoot builder
        )
        {
            SetConnections(connections, builder);
            SetErrorsCallback();
        }        

        private static void SetConnections(List<string> props, IConfigurationRoot builder)
        {

            if (props != null && props.Count > 0)
            {
                foreach (var connectionName in props)
                {
                    var conn = builder.GetConnectionString(connectionName);
                    ConnectionString.AddConnectionString(
                        ConnectionString.InstanceName(() => conn, connectionName)
                    );
                }
            }
        }

        private static void SetErrorsCallback()
        {
            BinderBL.SetDalError((ex, msg) => Console.WriteLine($"Error Opening connection {msg} - {ex.Message}"));
        }        

    }
}
