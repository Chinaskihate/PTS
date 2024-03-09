using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using PTS.Backend.Exceptions.Common;
using PTS.Contracts.Common;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace PTS.Backend.Middlewares;
internal class CustomExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;
        switch (exception)
        {
            case ValidationException:
                code = HttpStatusCode.BadRequest;
                break;
            case BadRequestException:
                code = HttpStatusCode.BadRequest;
                break;
            case NotFoundException:
                code = HttpStatusCode.NotFound;
                break;
        }

        context.Response.ContentType = "Application/json";
        context.Response.StatusCode = (int)code;

        Log.Error($"URL: {context.Request.Method} {context.Request.GetDisplayUrl()}" +
            $"{Environment.NewLine}{exception.Message}" +
            $"{Environment.NewLine}{exception.StackTrace}");
        if (string.IsNullOrEmpty(result))
        {
            result = JsonConvert.SerializeObject(new ResponseDto()
            {
                IsSuccess = false,
                Message = exception.Message,
            });
        }

        return context.Response.WriteAsync(result);
    }
}