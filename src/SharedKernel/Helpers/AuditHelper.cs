using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedKernel.Domain;
using System.Security.Claims;
using System.Text.Json;

public static class AuditHelper
{
    public static string GetClientIpAddress(HttpContext context)
    {
        // 1. Check 'X-Forwarded-For' header (used when behind proxies/load balancers)
        string? forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(forwardedFor))
        {
            // 'X-Forwarded-For' can contain a comma-separated chain: "client, proxy1, proxy2"
            // The first IP is the original client IP.
            string clientIp = forwardedFor.Split(',')[0].Trim();
            if (!string.IsNullOrEmpty(clientIp))
            {
                return clientIp;
            }
        }

        // 2. Check 'X-Real-IP' header (commonly used by Nginx)
        string? realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(realIp))
        {
            return realIp.Trim();
        }

        // 3. Fallback to direct connection IP address
        var remoteIp = context.Connection.RemoteIpAddress;
        if (remoteIp != null)
        {
            // Map IPv4-mapped IPv6 addresses (e.g., ::ffff:192.168.1.1) to clean IPv4
            return remoteIp.IsIPv4MappedToIPv6
                ? remoteIp.MapToIPv4().ToString()
                : remoteIp.ToString();
        }

        return "UnknownIP";
    }
    // public static string  GetEntityChanges(EntityEntry entry)
    // {
    //     // var beforeDict = new Dictionary<string, object?>();
    //     // var afterDict = new Dictionary<string, object?>();
    //     var diffDict = new Dictionary<string, object?>();

    //     foreach (var prop in entry.Properties)
    //     {
    //         // Skip temporary or unassigned primary keys
    //         if (prop.IsTemporary) continue;

    //         string propertyName = prop.Metadata.Name;
    //         object? originalValue = prop.OriginalValue;
    //         object? currentValue = prop.CurrentValue;

    //         switch (entry.State)
    //         {
    //             case EntityState.Added:
    //                 // afterDict[propertyName] = currentValue;
    //                 diffDict[propertyName] = new { old = (object?)null, @new = currentValue };
    //                 break;

    //             case EntityState.Deleted:
    //                 // beforeDict[propertyName] = originalValue;
    //                 diffDict[propertyName] = new { old = originalValue, @new = (object?)null };
    //                 break;

    //             case EntityState.Modified:
    //                 // beforeDict[propertyName] = originalValue;
    //                 // afterDict[propertyName] = currentValue;

    //                 // Capture only properties that actually changed
    //                 if (prop.IsModified && !Equals(originalValue, currentValue))
    //                 {
    //                     diffDict[propertyName] = new
    //                     {
    //                         old = originalValue,
    //                         @new = currentValue
    //                     };
    //                 }
    //                 break;
    //         }
    //     }

    //     return JsonSerializer.Serialize(diffDict);

    //     // return (
    //     //     Before: JsonSerializer.Serialize(beforeDict),
    //     //     After: JsonSerializer.Serialize(afterDict),
    //     //     Diff: JsonSerializer.Serialize(diffDict)
    //     // );
    // }

    public static EntityDiffResponse GetEntityChanges(EntityEntry entry)
    {
        var response = new EntityDiffResponse
        {
            EntityName = entry.Entity.GetType().Name,
            EntityState = entry.State.ToString(),
            Changes = new Dictionary<string, PropertyDiff>()
        };

        foreach (var prop in entry.Properties)
        {
            if (prop.IsTemporary) continue;

            string propertyName = prop.Metadata.Name;
            object? originalValue = prop.OriginalValue;
            object? currentValue = prop.CurrentValue;

            switch (entry.State)
            {
                case EntityState.Added:
                    response.Changes[propertyName] = new PropertyDiff(null, currentValue);
                    break;

                case EntityState.Deleted:
                    response.Changes[propertyName] = new PropertyDiff(originalValue, null);
                    break;

                case EntityState.Modified:
                    if (prop.IsModified && !Equals(originalValue, currentValue))
                    {
                        response.Changes[propertyName] = new PropertyDiff(originalValue, currentValue);
                    }
                    break;
            }
        }

        return response;
    }

    // Helper to get serialized JSON with frontend-friendly camelCase
    public static string GetEntityChangesJson(EntityEntry entry)
    {
        var result = GetEntityChanges(entry);
        
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        return JsonSerializer.Serialize(result, options);
    }

    public static (string Username, string TenantId) GetUserAndTenant(ClaimsPrincipal user)
    {
        if (user?.Identity == null || !user.Identity.IsAuthenticated)
        {
            return ("Anonymous", "None");
        }

        // 1. Get 'sub' (ASP.NET Core maps 'sub' to ClaimTypes.NameIdentifier by default)
        string username = user.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? user.FindFirstValue("sub")
                          ?? "UnknownUser";

        // 2. Get custom 'tenants' claim
        string tenantId = user.FindFirstValue("tenants")
                          ?? "UnknownTenant";

        return (username, tenantId);
    }
}