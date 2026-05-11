using System;
using Location.Application.Behaviors;
using Location.Application.Interfaces;
using Location.Contract.Interfaces;
using Location.Infrastructure.Persistences;
using Location.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Location.Infrastructure;

public static class LocationDependencyInjection
{
       public static IServiceCollection AddLocation(
        this IServiceCollection services,
        IConfiguration configuration)
      {

            services.AddScoped<ILocation,LocationBehavior>();
            services.AddScoped<ILocationRepository,LocationRepository>();

            // ==========================
            // Database
            // ==========================
            services.AddDbContext<LocationDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }
}
