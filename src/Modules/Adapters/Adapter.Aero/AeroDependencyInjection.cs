using System;
using System.Threading.Channels;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Listener;
using Adapter.Aero.Mapper;
using Adapter.Aero.Model;
using Adapter.Aero.Persistences;
using Adapter.Aero.Repositories;
using Adapter.Aero.Services;
using Adapter.Aero.Worker;
using Adapter.Aero.Writer;
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

            services.AddScoped<IScpWriter, ScpWriter>();
            services.AddScoped<ISioWriter, SioWriter>();
            services.AddScoped<IMpWriter, MpWriter>();
            services.AddScoped<ISioWriter, SioWriter>();
            services.AddScoped<IDriverWriter, DriverWriter>();


            services.AddScoped<IWriterRepository, WriterRepository>();
            services.AddScoped<IScpRepository,ScpRepository>();
            services.AddScoped<IMpRepository,MpRepository>();
            services.AddScoped<IWriterRepository,WriterRepository>();

            services.AddScoped<IScp,ScpService>();
                  
            services.AddScoped<IDeviceAdapter, AeroDeviceService>();
            // services.AddScoped<IMonitorAdapter, AeroMonitorService>();

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
