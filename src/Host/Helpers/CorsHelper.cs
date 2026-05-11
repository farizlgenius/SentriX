using System;

namespace Host.Helpers;

public static class CorsHelper
{
      public static void Cors(WebApplicationBuilder builder)
      {
            builder.Services.AddCors(options =>
            {
                  options.AddPolicy("CorsPolicy", policy =>
                  {
                        policy.WithOrigins("http://localhost:5173")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // REQUIRED for cookies
                  });
            });
      }
}
