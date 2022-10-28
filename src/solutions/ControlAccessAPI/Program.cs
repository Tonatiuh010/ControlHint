using Engine.BL;
using Engine.Constants;
using Engine.Services;
using BaseAPI;
using BaseAPI.Classes;

Builder.Build(new WebProperties("AccessControl", WebApplication.CreateBuilder(args))
    {
        ConnectionString = C.ACCESS_DB
    }   
);