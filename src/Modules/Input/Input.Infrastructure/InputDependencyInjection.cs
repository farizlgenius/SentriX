using System;
using Input.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Input.Infrastructure;

public static class InputDependencyInjection
{
      public static IServiceCollection AddInput(
        this IServiceCollection services,
        IConfiguration configuration)
      {


            // ==========================
            // Database
            // ==========================
            services.AddDbContext<InputDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }
}
