using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifier.Client.Services;
using Notifier.Contract.Interfaces;

namespace Notifier.Client;

public static class NotifierDependencyInjection
{
      public static IServiceCollection AddNotifyModule(
        this IServiceCollection services,
        IConfiguration configuration)
      {
            // ==========================
            // Adding Repository
            // ==========================
            services.AddSignalR();
            services.AddScoped<INotifier, NotifierService>();

            return services;
      }

}
