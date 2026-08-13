using System;
using Core.Application.Interfaces;
using Core.Application.Services;
using Core.Application.ValueObjects;
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
    // App Setting
    services.AddOptions<LicenseSetting>().Bind(configuration.GetSection("License")).ValidateOnStart();
    services.AddSingleton<ILicenseSetting>(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<LicenseSetting>>().Value);

    // Location
    services.AddScoped<ILocation, LocationService>();
    services.AddScoped<ILocationRepository, LocationRepository>();

    // Company
    services.AddScoped<ICompany, CompanyService>();
    services.AddScoped<ICompanyRepository, CompanyRepository>();

    // Department
    services.AddScoped<IDepartment, DepartmentService>();
    services.AddScoped<IDepartmentRepository, DepartmentRepository>();

    // Position
    services.AddScoped<IPosition, PositionService>();
    services.AddScoped<IPositionRepository, PositionRepository>();

    // Feature
    services.AddScoped<IFeature, FeatureService>();
    services.AddScoped<IFeatureRepository, FeatureRepository>();

    // Role
    services.AddScoped<IRole, RoleService>();
    services.AddScoped<IRoleRepository, RoleRepository>();

    // Operator
    services.AddScoped<IOperator, OperatorService>();
    services.AddScoped<IOperatorRepository, OperatorRepository>();

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
