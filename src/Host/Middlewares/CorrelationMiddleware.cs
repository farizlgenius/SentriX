using System;
using Serilog.Context;

namespace Host.Middlewares;

public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.TraceIdentifier;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}
