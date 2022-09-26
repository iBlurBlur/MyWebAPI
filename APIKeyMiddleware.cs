using MyWebAPI.Constants;

namespace MyWebAPI;

public class APIKeyMiddleware
{
    private readonly RequestDelegate _next;
    public APIKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(SecurityConfig.APIKeyHeader, out var extractedApiKey))
        {
            await Unauthorized(context, "API Key was not provided");
            return;
        }
        var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = appSettings.GetValue<string>(SecurityConfig.ConfigAPIKey);
        if (!apiKey.Equals(extractedApiKey))
        {
            await Unauthorized(context, "Unauthorized");
            return;
        }
        await _next(context);
    }
    private async Task Unauthorized(HttpContext context, string message)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new
        {
            Message = message
        });
    }
}