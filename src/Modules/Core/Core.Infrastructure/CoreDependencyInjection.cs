using System;
using Core.Application.Interfaces;
using Core.Application.Services;
using Core.Contract.Interfaces;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Repositories;
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

    // Location
    services.AddScoped<ILocation, LocationService>();
    services.AddScoped<ILocationRepository, LocationRepository>();

    // Company
    services.AddScoped<ICompany, CompanyService>();
    services.AddScoped<ICompanyRepository, CompanyRepository>();

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
