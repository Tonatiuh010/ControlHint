using BaseAPI.Classes;
using Engine.Constants;
using DocsControl;
using DocsControl.Ocr;

BaseAPI.Builder.Build(new WebProperties("DocsControl", WebApplication.CreateBuilder(args), new SubscriptionAPI())
{
    ConnectionString = C.ACCESS_DB,
    ConnectionStrings = new List<string>()
    {
        C.ACCESS_DB,
        C.HINT_DB,
        C.DOCS_DB
    },    
}, builderCallback: builder  =>
{
    OcrScan.SetLicense();
});