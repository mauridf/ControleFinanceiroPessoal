using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Net;

namespace ControleFinanceiroPessoal.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        string message = "An unexpected error occurred.";

        // Tratamento de erros específicos (MongoDB, etc)
        if (exception is MongoException)
        {
            statusCode = HttpStatusCode.ServiceUnavailable;
            message = "Database error occurred.";
        }
        else if (exception is FormatException)
        {
            statusCode = HttpStatusCode.BadRequest;
            message = "Invalid data format.";
        }

        var response = new
        {
            StatusCode = (int)statusCode,
            Message = message,
            Detailed = exception.Message // Pode remover em produção para maior segurança
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
