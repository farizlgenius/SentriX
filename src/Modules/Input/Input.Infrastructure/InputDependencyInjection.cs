using System;
using Input.Application.Behaviors;
using Input.Application.Interfaces;
using Input.Contract.Interfaces;
using Input.Infrastructure.Persistences;
using Input.Infrastructure.Repositories;
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

            services.AddScoped<IInputRepository,InputRepository>();
            services.AddScoped<IInput,InputBehavior>();

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
