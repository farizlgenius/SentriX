using System;
using Adapter.Abstraction.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adapter.Abstraction;

public static class AdapterDependencyInjection
{
      public static IServiceCollection AddAdapter(
            this IServiceCollection services,
            IConfiguration configuration)
            {
                  services.AddScoped<IAdapterFactory, AdapterFactory>();

                  return services;
            }
}
