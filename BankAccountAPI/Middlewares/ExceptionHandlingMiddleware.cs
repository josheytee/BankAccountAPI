using System;
using System.Net;
using System.Text.Json;
using BankAccountAPI.Models;

namespace BankAccountAPI.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        var errorResponse = new Response<Object>();
        switch (exception)
        {
            case ApplicationException ex:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = ex.Message;
                errorResponse.Code = HttpStatusCode.BadRequest.ToString();
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Code = HttpStatusCode.InternalServerError.ToString();
                errorResponse.Message = "Internal server error!";
                break;
        }
        _logger.LogError(exception.Message);
        var result = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(result);
    }
}
