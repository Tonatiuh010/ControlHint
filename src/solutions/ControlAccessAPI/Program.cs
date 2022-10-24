using Engine.BL;
using Engine.Constants;
using Engine.Services;
using BaseAPI;
using BaseAPI.Classes;
using ControlAccess.Hubs;

Builder.Build(new WebProperties("AccessControl", WebApplication.CreateBuilder(args))
    {
        ConnectionString = C.ACCESS_DB
    },
    builderCallback: builder =>
    {
        builder.Services.AddSignalR();
    },
    appCallback: app =>
    {
        app.MapHub<CheckHub>("/checkMonitor");
    }
);