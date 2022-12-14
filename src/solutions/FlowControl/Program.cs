using Engine.BL;
using Engine.Constants;
using Engine.Services;
using BaseAPI;
using BaseAPI.Classes;
using FlowControl.Hubs;
using FlowControl;
using FlowControl.Controllers;
using Microsoft.AspNetCore.WebSockets;

Builder.Build(new WebProperties("FlowControl", WebApplication.CreateBuilder(args), new SubscriptionAPI())
    {
        ConnectionString = C.HINT_DB,
        ConnectionStrings = new List<string>()
        {
            C.ACCESS_DB,
            C.HINT_DB,
            C.DOCS_DB
        }
    },
    builderCallback: builder =>
    {
        builder.Services.AddSignalR();
        //builder.Services.AddWebSockets();
    },
    appCallback: app =>
    {
        app.UseWebSockets(new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromMinutes(2)
        });
        app.MapHub<CheckHub>("/Check").RequireCors("SignalR");
        app.MapHub<DeviceHub>("/Device").RequireCors("SignalR");
    }
);