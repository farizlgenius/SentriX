using System;
using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Adapters;
using Adapter.Amico.Command;
using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using Adapter.Amico.Persistences;
using Adapter.Amico.Repositories;
using Adapter.Amico.Services;
using Adapter.Amico.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adapter.Amico;

public static class AmicoDependencyInjection
{
      public static IServiceCollection AddAmico(
        this IServiceCollection services,
        IConfiguration configuration)
      {
            services.AddOptions<AmicoSetting>().Bind(configuration.GetSection("Amico")).ValidateOnStart();

            services.AddSingleton<IAmicoSetting>(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<AmicoSetting>>().Value);

            services.AddHttpClient();
            services.AddScoped<IHttpClient, HttpClientService>();
            services.AddScoped<IAmicoDeviceAdapter,AmicoDeviceAdapter>();
            services.AddScoped<IDeviceCommand,DeviceCommand>();
            services.AddScoped<IAmicoRepository,AmicoRepository>();

            services.AddScoped<IAdapter, AmicoAdapter>();



            // ==========================
            // Database
            // ==========================
            services.AddDbContext<AmicoDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }
}
