using System.Threading.Channels;
using Adapter.Aero.Listener;
using Adapter.Contract.Interfaces;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Application.Services;
using Aero.Application.ValueObjects;
using Aero.Domain.Entities;
using Aero.Infrastructure.Adapter;
using Aero.Infrastructure.Repositories;
using Aero.Infrastructure.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Aero.Infrastructure;

public static class AeroDependencyInjection
{
  public static IServiceCollection AddAero(
    this IServiceCollection services,
    IConfiguration configuration)
  {

    services.AddOptions<ScpDeviceSpecification>()
    .Bind(configuration.GetSection("Aero:ScpDeviceSpecification"))
    .ValidateOnStart();

    services.AddSingleton<IScpDeviceSpecification>(sp =>
        sp.GetRequiredService<IOptions<ScpDeviceSpecification>>().Value);

    services.AddOptions<ScpDeviceSpecification>()
    .Bind(configuration.GetSection("Aero:CreateChannel"))
    .ValidateOnStart();

    services.AddSingleton<ICreateChannel>(sp =>
        sp.GetRequiredService<IOptions<CreateChannel>>().Value);

    //       services.AddHttpClient<AeroHttpClient>(c =>
    //   {
    //       c.BaseAddress = new Uri("https://aero-api/");
    //       c.Timeout = TimeSpan.FromSeconds(30);
    //   });

    services.AddHostedService<ReplyWorker>();
    services.AddSingleton<ReplyMessageListener>();
    services.AddScoped<IObjectMapper, ReplyMapper>();

    services.AddScoped<IBaseRepository,BaseRepository>();

    services.AddScoped<IAdapter, AeroAdapter>();

    // IdReport
    services.AddScoped<IIdReportService,IdReportService>();

    // Device
    services.AddScoped<IDeviceRepository,DeviceRepostory>();
    services.AddScoped<IDeviceAdapter,DeviceService>();

    services.AddScoped<IDeviceModuleRepository,DeviceModuleRepository>();

    // Utility
    services.AddScoped<IUtilityAdapter,UtilityService>();
    services.AddScoped<CommandDecoder>();
    // services.AddScoped<IDeviceAdapter,DeviceService>();


    // ==========================
    // Worker
    // ==========================
    services.AddSingleton(
        Channel.CreateBounded<ReplyMessage>(
         new BoundedChannelOptions(10_000)
         {
           FullMode = BoundedChannelFullMode.DropOldest,
           SingleReader = true,
           SingleWriter = false
         }
        )
     );


    return services;
  }
}
