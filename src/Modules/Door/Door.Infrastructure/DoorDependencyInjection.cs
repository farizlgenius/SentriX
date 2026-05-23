using System;
using Door.Infrastructure.Persistences;
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
