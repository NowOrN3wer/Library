using Library.Infrastructure.Context;
using Newtonsoft.Json;
using System.Text.Json;
using Library.Domain.Entities;

namespace Library.WebAPI.Middlewares;

public class RequestResponseLoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
    {
        var requestTime = DateTimeOffset.UtcNow;
        var request = context.Request;
        var originalBodyStream = context.Response.Body;

        var requestBodyText = string.Empty;
        if (request.ContentLength > 0 && request.Body.CanRead)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, leaveOpen: true);
            requestBodyText = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await next(context); // request pipeline’a devam et

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        var log = new ApiLog
        {
            IPAddress = context.Connection.RemoteIpAddress?.ToString(),
            Path = request.Path,
            Method = request.Method,
            RequestBody = ToJsonSafe(requestBodyText),
            ResponseBody = ToJsonSafe(responseText),
            StatusCode = context.Response.StatusCode,
            RequestTime = requestTime,
            ResponseTime = DateTimeOffset.UtcNow // 🔁 Uygun şekilde düzeltildi
        };

        dbContext.ApiLogs.Add(log);
        await dbContext.SaveChangesAsync();

        await responseBody.CopyToAsync(originalBodyStream);
    }

    private static string ToJsonSafe(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return "null";
        }

        try
        {
            JsonDocument.Parse(input);
            return input;
        }
        catch
        {
            return JsonConvert.SerializeObject(input);
        }
    }
}