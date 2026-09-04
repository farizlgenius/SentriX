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

    // 1. Bind LicenseSetting (automatically binds nested LicenseEndpointSetting too)
    services.AddOptions<LicenseSetting>()
        .Bind(configuration.GetSection("License"))
        .ValidateOnStart();

    // 2. Register ILicenseSetting
    services.AddSingleton<ILicenseSetting>(sp =>
        sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<LicenseSetting>>().Value);

    // 3. Register ILicenseEndpointSetting by referencing the nested Endpoint
    services.AddSingleton<ILicenseEndpointSetting>(sp =>
        sp.GetRequiredService<ILicenseSetting>().Endpoint);

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

    services.AddScoped<IModuleRepository, ModuleRepository>();

    // License
    services.AddScoped<ILicense, LicenseService>();
    services.AddScoped<ILicenseRepository, LicenseRepostory>();
    services.AddScoped<IMachine, MachineService>();

    // User
    services.AddScoped<IUser, UserService>();
    services.AddScoped<IUserRepository, UserRepository>();

    // Group
    // services.AddScoped<IGroup, GroupService>();
    services.AddScoped<IGroupRepository, GroupRepository>();

    // Operator
    services.AddScoped<IOperator, OperatorService>();
    services.AddScoped<IOperatorRepository, OperatorRepository>();

    // ComponentMapping
    services.AddScoped<IComponentMapping, ComponentMappingService>();
    services.AddScoped<IComponentMappingRepository, ComponentMappingRepository>();

    // Device
    services.AddScoped<IDevice, DeviceService>();
    services.AddScoped<IDeviceRepository, DeviceRepository>();


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
