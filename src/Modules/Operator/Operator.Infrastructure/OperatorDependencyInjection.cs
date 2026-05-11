using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Operator.Application.Behaviors;
using Operator.Application.Interfaces;
using Operator.Contract.Interfaces;
using Operator.Infrastructure.Persistences;
using Operator.Infrastructure.Repositories;

namespace Operator.Infrastructure;

public static class OperatorDependencyInjection
{
      public static IServiceCollection AddOperator(
        this IServiceCollection services,
        IConfiguration configuration)
      {
            services.AddScoped<IOperator,OperatorBehaviors>();
            services.AddScoped<IOperatorRepository,OperatorRepository>();

            // ==========================
            // Database
            // ==========================
            services.AddDbContext<OperatorDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }

}
