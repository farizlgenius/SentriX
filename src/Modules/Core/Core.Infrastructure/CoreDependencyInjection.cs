using System;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure;

public static class CoreDependencyInjection
{
  public static IServiceCollection AddCore(
    this IServiceCollection services,
    IConfiguration configuration)
  {

    // services.AddScoped<IDoor, DoorBehavior>();

    // ==========================
    // Database
    // ==========================
    services.AddDbContext<CoreDbContext>(options =>
        options.UseNpgsql(
        configuration.GetConnectionString("PostgresConnection"),
        npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        ));

    return services;
  }
}
