using System;

namespace Host.Helpers;

public static class AppSettingHelper
{
      public static void ReadAppSetting(WebApplicationBuilder builder)
      {
            // ==========================
            // Read config from appsetting.json
            // ==========================

            // builder.Services.Configure<RedisSettings>(
            //   builder.Configuration.GetSection("Redis")
            // );

            // builder.Services
            //        .AddOptions<JwtData>()
            //        .Bind(builder.Configuration.GetSection("JwtSetting"))
            //        .ValidateOnStart();

            // builder.Services.AddSingleton<IJwtData>(sp => sp.GetRequiredService<
            //     Microsoft.Extensions.Options.IOptions<JwtData>>().Value);
      }
}
