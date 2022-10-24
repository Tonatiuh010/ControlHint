using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Engine.BO;
using Engine.Constants;
using D = Engine.BL.Delegates;
using Engine.Services;
using UttClassroom.Classes;
using Engine.BL;

namespace Classes;

public abstract class CustomController : ControllerBase
{
    protected List<RequestError> ErrorsRequest { get; set; } = new List<RequestError>();
    protected D.CallbackExceptionMsg? OnMissingProperty => ErrorManager.Subscription;
    private ExceptionManager ErrorManager { get; set; }

    public CustomController() { 
        ErrorManager = new(SetErrorOnRequest); 

    }

    protected Result RequestResponse(
        D.ActionResult action,
        D.ActionResult? action2 = null,
        D.ActionResult? action3 = null,
        D.ActionResult? action4 = null
    ) => RequestBlock(result => {
        if (result != null)
        {
            result.Data = action();

            if (action2 != null)
                result.Data2 = action2();

            if (action3 != null)
                result.Data3 = action3();

            if (action4 != null)
                result.Data4 = action4();

            result.Message = C.COMPLETE;

        }

        return result ?? throw new Exception("RequestResponse result is Empty! RequestResponse()");
    });   

    private Result RequestBlock(D.CallbackResult action)
    {        
        Result result = new() { Status = C.OK };

        try
        {
            result = action(result);

            if (ErrorsRequest != null && ErrorsRequest.Count > 0)
            {
                throw new Exception("Errores en request");
            }

        }
        catch (Exception ex)
        {
            ErrorResult(this, result, ex);
        }

        ErrorManager.Dispose();

        return result;
    }

    protected void SetErrorOnRequest(Exception ex, string msg) => ErrorsRequest.Add(new RequestError()
    {
        Info = msg,
        Exception = ex
    });

    public static void ErrorResult(CustomController controller, Result result, Exception? ex = null)
    {
        result.Status = C.ERROR;
        result.Data = RequestError.FormatErrors(controller.ErrorsRequest);        

        if (ex != null)
        {
            result.Message = ex.Message;
        }
        else
        {
            result.Message = "Error doing something!";
        }
    }    

    public static T? GetItem<T>(List<T> list, string? emptyMsg = null)
    {
        if (list != null && list.Count > 0)
        {
            return list[0];
        }

        return default;
    }
    
}