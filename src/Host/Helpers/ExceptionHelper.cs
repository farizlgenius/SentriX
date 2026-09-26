using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Host.Helpers;

public static class ExceptionHelper
{
    private const int MaxRequestBodyLength = 32_000; // Cap log payload size (e.g., 32 KB)

    public static async Task<string> GetRequestBodyAsync(HttpContext context)
    {
        // 1. If content length exceeds cap (e.g., file uploads), skip full reading
        if (context.Request.ContentLength > MaxRequestBodyLength)
        {
            return $"[Payload exceeds maximum read size of {MaxRequestBodyLength} bytes]";
        }

        // 2. Enable stream buffering so ASP.NET Core MVC can re-read the body later
        context.Request.EnableBuffering();
        context.Request.Body.Position = 0;

        // 3. Explicitly define UTF-8 encoding and prevent automatic BOM stripping
        using var reader = new StreamReader(
            context.Request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true
        );

        var body = await reader.ReadToEndAsync();

        // 4. Always reset position back to 0 for downstream binders
        context.Request.Body.Position = 0;

        return MaskSensitiveData(body);
    }

    public static void LogException(
        ILogger logger,
        HttpContext context,
        Exception ex,
        LogLevel logLevel,
        string message,
        string requestBody)
    {
        logger.Log(
            logLevel,
            ex,
            "{Message}. {Method} {Path} {QueryString} {RequestBody}",
            message,
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString.Value,
            requestBody
        );
    }

    /// <summary>
    /// Basic sanitizer to mask sensitive fields before writing to logs.
    /// </summary>
    private static string MaskSensitiveData(string body)
    {
        if (string.IsNullOrWhiteSpace(body)) return body;

        // Simple string replacement for basic auth keys
        // Can be expanded with regex or JSON AST sanitizers if needed
        return body
            .Replace("\"password\"", "\"*** MASKED ***\"", StringComparison.OrdinalIgnoreCase)
            .Replace("\"token\"", "\"*** MASKED ***\"", StringComparison.OrdinalIgnoreCase)
            .Replace("\"secret\"", "\"*** MASKED ***\"", StringComparison.OrdinalIgnoreCase);
    }
}