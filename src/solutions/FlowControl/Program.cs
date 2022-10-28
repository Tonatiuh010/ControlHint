using Engine.BL;
using Engine.Constants;
using Engine.Services;
using BaseAPI;
using BaseAPI.Classes;
using FlowControl.Hubs;

Builder.Build(new WebProperties("FlowControl", WebApplication.CreateBuilder(args))
    {
        ConnectionString = C.HINT_DB,
        ConnectionStrings = new List<string>()
        {
            C.HINT_DB,
            C.ACCESS_DB
        }
    },
    builderCallback: builder =>
    {
         builder.Services.AddSignalR();
    },
    appCallback: app =>
    {
        app.MapHub<CheckHub>("/checkMonitor");
        app.MapHub<DeviceHub>("/deviceMonitor");
    }
);