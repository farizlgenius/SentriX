using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Output.Infrastructure.Persistences;

namespace Output.Infrastructure;

public static class OutputDependencyInjection
{
      public static IServiceCollection AddOutput(
        this IServiceCollection services,
        IConfiguration configuration)
      {



            // ==========================
            // Database
            // ==========================
            services.AddDbContext<OutputDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }
}
