using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Output.Application.Behaviors;
using Output.Application.Interfaces;
using Output.Infrastructure.Persistences;
using Output.Infrastructure.Repositories;

namespace Output.Infrastructure;

public static class OutputDependencyInjection
{
      public static IServiceCollection AddOutput(
        this IServiceCollection services,
        IConfiguration configuration)
      {

            services.AddScoped<IOutputRepository,OutputRepository>();
            services.AddScoped<IOutput,OutputBehavior>();

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
