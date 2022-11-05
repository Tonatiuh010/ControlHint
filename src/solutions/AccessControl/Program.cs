using Engine.BL;
using Engine.Constants;
using Engine.Services;
using BaseAPI;
using BaseAPI.Classes;
using AccessControl;

Builder.Build(new WebProperties("AccessControl", WebApplication.CreateBuilder(args), new SubscriptionAPI())
    {
        ConnectionString = C.ACCESS_DB,
        ConnectionStrings = new List<string>()
        {
            C.ACCESS_DB,
            C.HINT_DB
        }
    }
);