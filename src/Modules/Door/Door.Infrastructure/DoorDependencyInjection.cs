using System;
using Door.Application.Behaviors;
using Door.Application.Interfaces;
using Door.Contract.Interfaces;
using Door.Infrastructure.Persistences;
using Door.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Door.Infrastructure;

public static class InputDependencyInjection
{
      public static IServiceCollection AddDoor(
        this IServiceCollection services,
        IConfiguration configuration)
      {

            services.AddScoped<IDoor,DoorBehavior>();
            services.AddScoped<IDoorRepository,DoorRepository>();

            // ==========================
            // Database
            // ==========================
            services.AddDbContext<DoorDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }
}
