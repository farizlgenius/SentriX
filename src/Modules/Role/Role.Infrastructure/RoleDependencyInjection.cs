using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Role.Application.Behaviors;
using Role.Application.Interfaces;
using Role.Contract.Interfaces;
using Role.Infrastructure.Persistences;
using Role.Infrastructure.Repositories;

namespace Role.Infrastructure;

public static class LocationDependencyInjection
{
       public static IServiceCollection AddRole(
        this IServiceCollection services,
        IConfiguration configuration)
      {

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRole, RoleBehavior>();


            // ==========================
            // Database
            // ==========================
            services.AddDbContext<RoleDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }
}
