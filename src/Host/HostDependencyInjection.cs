using System;
using Host.Middlewares;

namespace Host;

public static class HostDependencyInjection
{
      public static IServiceCollection AddHost(
        this IServiceCollection services,
        IConfiguration configuration)
      {

            
           services.AddTransient<GlobalException>();


            return services;
      }

}
