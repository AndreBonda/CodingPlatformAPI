using System.Net;
using System.Text.Json;
using CodingPlatform.Domain.Exception;

namespace CodingPlatform.Web.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            //Custom error handling
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case BadRequestException:
                    response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                case ForbiddenException:
                    response.StatusCode = (int) HttpStatusCode.Forbidden;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new {message = error?.Message});
            await response.WriteAsync(result);
        }
    }
}