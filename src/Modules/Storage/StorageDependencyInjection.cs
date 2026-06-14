using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Storage.Behaviors;
using Storage.Contract.Interfaces;
using Storage.Interfaces;

namespace Storage;

public static class StorageDependencyInjection
{
      public static IServiceCollection AddStorage(
       this IServiceCollection services,
       IConfiguration configuration)
      {

            services.AddScoped<IStorage, StorageBehavior>();
            services.AddScoped<IFilePathProvider,PathProviderBehavior>();


            return services;
      }
}