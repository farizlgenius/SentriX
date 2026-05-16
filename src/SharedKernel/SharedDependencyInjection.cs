using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SharedKernel.Logging;
using SharedKernel.Messaging;

namespace SharedKernel;

public static class SharedDependencyInjection
{

      public static IServiceCollection AddShared(
        this IServiceCollection services,
        IConfiguration configuration)
      {

            services.AddScoped<IMessageBus, MessageBus>();

            services.Scan(scan => scan
                .FromApplicationDependencies()
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan
                .FromApplicationDependencies()
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan
                .FromApplicationDependencies()
                .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            

            return services;
      }

}

// public static class SharedDependencyInjection
// {
//     public static IServiceCollection AddShared(
//         this IServiceCollection services,
//         IConfiguration configuration)
//     {
//         // MessageBus must be scoped
//         services.AddScoped<IMessageBus, MessageBus>();

//         var assemblies = new[]
//         {
//             typeof(Location.Application.AssemblyReference).Assembly,
//             typeof(Operator.Application.AssemblyReference).Assembly,
//             typeof(Device.Application.AssemblyReference).Assembly
//         };

//         // Command handlers
//         services.Scan(scan => scan
//             .FromAssemblies(assemblies)
//             .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
//             .AsImplementedInterfaces()
//             .WithScopedLifetime());

//         // Query handlers
//         services.Scan(scan => scan
//             .FromAssemblies(assemblies)
//             .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
//             .AsImplementedInterfaces()
//             .WithScopedLifetime());

//         // Event handlers
//         services.Scan(scan => scan
//             .FromAssemblies(assemblies)
//             .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
//             .AsImplementedInterfaces()
//             .WithScopedLifetime());

//         return services;
//     }
// }