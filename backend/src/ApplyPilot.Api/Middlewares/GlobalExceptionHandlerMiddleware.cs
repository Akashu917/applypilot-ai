using System.Net;
using System.Text.Json;
using ApplyPilot.Application.Common.Exceptions;
using ApplyPilot.Application.DTOs.Common;

namespace ApplyPilot.Api.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        int statusCode;
        ApiResponse<object> response;

        switch (ex)
        {
            case ValidationException valEx:
                statusCode = (int)HttpStatusCode.UnprocessableEntity;
                response = ApiResponse<object>.Fail("Validation failed.", valEx.Errors);
                break;

            case UnauthorizedException unauthEx:
                statusCode = (int)HttpStatusCode.Unauthorized;
                response = ApiResponse<object>.Fail(unauthEx.Message);
                break;

            case NotFoundException notFoundEx:
                statusCode = (int)HttpStatusCode.NotFound;
                response = ApiResponse<object>.Fail(notFoundEx.Message);
                break;

            case AppException appEx:
                statusCode = appEx.StatusCode;
                response = ApiResponse<object>.Fail(appEx.Message);
                break;

            default:
                _logger.LogError(ex, "An unhandled exception occurred during request execution: {Message}", ex.Message);
                statusCode = (int)HttpStatusCode.InternalServerError;
                response = ApiResponse<object>.Fail("An unexpected error occurred. Please try again later.");
                break;
        }

        context.Response.StatusCode = statusCode;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}

