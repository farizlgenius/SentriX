using System.Threading.Channels;
using Adapter.Aero.Listener;
using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Application.ValueObjects;
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



    // ==========================
    // Worker
    // ==========================
    services.AddSingleton(
        Channel.CreateBounded<ReplyMapper>(
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
