using CArch.Domain.Entities;
using Library.Infrastructure.Context;
using Newtonsoft.Json;
using System.Text.Json;

namespace Library.WebAPI.Middlewares;
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
    {
        var requestTime = DateTime.Now;
        var request = context.Request;
        var originalBodyStream = context.Response.Body;

        string requestBodyText = "";
        if (request.ContentLength > 0 && request.Body.CanRead)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, leaveOpen: true);
            requestBodyText = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context); // request pipeline’a devam et

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
            ResponseTime = DateTime.Now
        };

        dbContext.ApiLogs.Add(log);
        await dbContext.SaveChangesAsync();

        await responseBody.CopyToAsync(originalBodyStream);
    }


    private string ToJsonSafe(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "null"; // veya "{}"

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
