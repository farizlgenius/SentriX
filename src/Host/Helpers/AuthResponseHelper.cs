using System;
using System.Text.Json;

namespace Host.Helpers;

public class AuthResponseHelper
{
  public static async Task Write401(HttpResponse response, string message)
  {
    response.StatusCode = StatusCodes.Status401Unauthorized;
    response.ContentType = "application/json";

    var json = JsonSerializer.Serialize(new
    {
      status = 401,
      timestamp = DateTime.UtcNow,
      message
    });

    await response.WriteAsync(json);
  }

  public static async Task Write403(HttpResponse response)
  {
    response.StatusCode = StatusCodes.Status403Forbidden;
    response.ContentType = "application/json";

    var json = JsonSerializer.Serialize(new
    {
      status = 403,
      timestamp = DateTime.UtcNow,
      message = "You don't have permission to access this resource"
    });

    await response.WriteAsync(json);
  }
}