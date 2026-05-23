using System;
using System.Threading.Channels;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Command;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Listener;
using Adapter.Aero.Mapper;
using Adapter.Aero.Model;
using Adapter.Aero.Persistences;
using Adapter.Aero.Repositories;
using Adapter.Aero.Services;
using Adapter.Aero.Worker;
using AeroAdapter.Application.Interfaces;
using AeroAdapter.Infrastructure.Writer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adapter.Aero;

public static class AeroDependencyInjection
{
      public static IServiceCollection AddAero(
        this IServiceCollection services,
        IConfiguration configuration)
      {
            //       services.AddHttpClient<AeroHttpClient>(c =>
            //   {
            //       c.BaseAddress = new Uri("https://aero-api/");
            //       c.Timeout = TimeSpan.FromSeconds(30);
            //   });

            services.AddHostedService<ScpReplyWorker>();
            services.AddSingleton<ReplyMessageListener>();
            services.AddScoped<IObjectMapper, ScpReplyMapper>();



            // ==========================
            // Worker
            // ==========================
            services.AddSingleton(
                Channel.CreateBounded<SCPReplyMessageDto>(
                 new BoundedChannelOptions(10_000)
                    {
                        FullMode = BoundedChannelFullMode.DropOldest,
                        SingleReader = true,
                        SingleWriter = false
                    }
                )
             );

            services.AddScoped<IScpCommand, ScpCommand>();
            services.AddScoped<IModuleCommand, ModuleCommand>();
            services.AddScoped<IInputCommand, InputCommand>();
            services.AddScoped<IOutputCommand,OutputCommand>();
            services.AddScoped<IModuleCommand, ModuleCommand>();
            services.AddScoped<IDriverCommand, DriverCommand>();
            services.AddScoped<ITimeCommand,TimeCommand>();
            services.AddScoped<IDoorCommand,DoorCommand>();
            services.AddScoped<IGroupCommand,GroupCommand>();


            services.AddScoped<IScp,ScpService>();
            services.AddScoped<IAeroRepository,AeroRepository>();
                  
            services.AddScoped<IDeviceAdapter, AeroDeviceService>();
            services.AddScoped<IControlAdapter,AeroControlService>();
            services.AddScoped<IMonitorAdapter,AeroMonitorService>();
            services.AddScoped<ITimeAdapter,AeroTimeService>();
            services.AddScoped<IDoorAdapter,AeroDoorService>();
            services.AddScoped<IGroupAdapter,AeroGroupService>();


            services.AddScoped<IAdapter, AeroAdapter>();

            services.AddSingleton<IIdReportService,IdReportService>();

            // ==========================
            // Database
            // ==========================
            services.AddDbContext<AeroDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }
}
