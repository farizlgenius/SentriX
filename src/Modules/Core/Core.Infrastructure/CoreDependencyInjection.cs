using System;
using System.Threading.Channels;
using Core.Application.Interfaces;
using Core.Application.Services;
using Core.Application.ValueObjects;
using Core.Contract.DTOs.Events.Audit;
using Core.Contract.DTOs.Events.Event;
using Core.Contract.Interfaces;
using Core.Infrastructure.Interceptors;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Persistences.Entities;
using Core.Infrastructure.Repositories;
using Core.Infrastructure.Workers;
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

    // Time
    services.AddScoped<ITime, TimeService>();
    services.AddScoped<ITimeRepository, TimeRepository>();

    // Holiday
    services.AddScoped<IHoliday, HolidayService>();
    services.AddScoped<IHolidayRepository, HolidayRepository>();

    // Interval
    services.AddScoped<IInterval, IntervalService>();
    services.AddScoped<IIntervalRepository, IntervalRepository>();

    // ModuelDevice
    services.AddScoped<IDeviceModuleRepository, DeviceModuleRepository>();

    // Door
    services.AddScoped<IDoor, DoorService>();
    services.AddScoped<IDoorRepository, DoorRepository>();

    // Turnstile
    services.AddScoped<ITurnstile, TurnstileService>();
    services.AddScoped<ITurnstileRepository, TurnstileRepository>();

    // Group
    services.AddScoped<IGroup, GroupService>();
    services.AddScoped<IGroupRepository, GroupRepository>();

    // Module Device
    services.AddScoped<IDeviceModule, DeviceModuleService>();
    services.AddScoped<IDeviceModuleRepository, DeviceModuleRepository>();

    // Temp Device
    services.AddSingleton<ITempDevice, TempDeviceService>();

    // Event
    services.AddScoped<IEvent,EventService>();
    services.AddScoped<IEventRepository,EventRepostory>();

    // Utility
    services.AddScoped<IUtility,UtilityService>();

    // ==========================
    // Worker
    // ==========================
    services.AddHostedService<AuditTrailWorker>();
    services.AddHostedService<EventWorker>();
    services.AddSingleton(
        Channel.CreateBounded<AuditTrailInsert>(
         new BoundedChannelOptions(10_000)
         {
           FullMode = BoundedChannelFullMode.DropOldest,
           SingleReader = true,
           SingleWriter = false
         }
        )
     );

     services.AddSingleton(
        Channel.CreateBounded<CreateEventDto>(
         new BoundedChannelOptions(10_000)
         {
           FullMode = BoundedChannelFullMode.DropOldest,
           SingleReader = true,
           SingleWriter = false
         }
        )
     );

     // 2. Register Interceptor & HttpContextAccessor
    services.AddHttpContextAccessor();
    services.AddScoped<AuditSaveChangesInterceptor>();

    


    // ==========================
    // Database
    // ==========================
    services.AddDbContext<CoreDbContext>((sp,options) =>
    {
      var interceptor = sp.GetRequiredService<AuditSaveChangesInterceptor>();

      options.UseNpgsql(
        configuration.GetConnectionString("PostgresConnection"),
        npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        ).AddInterceptors(interceptor);
    });

    return services;
  }
}
