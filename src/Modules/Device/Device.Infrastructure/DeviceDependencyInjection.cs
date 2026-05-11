using System;
using Device.Application.Interfaces;
using Device.Infrastructure.Persistences;
using Device.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Device.Infrastructure;

public static class DeviceDependencyInjection
{
      public static IServiceCollection AddDevice(
        this IServiceCollection services,
        IConfiguration configuration)
      {

            services.AddScoped<IDeviceRepository, DeviceRepository>();

             // ==========================
            // ==========================
            // Database
            // ==========================
            services.AddDbContext<DeviceDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }

}