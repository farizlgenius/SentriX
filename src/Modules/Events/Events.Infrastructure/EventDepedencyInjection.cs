using System;
using Events.Application.Interfaces;
using Events.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Events.Infrastructure.Repositories;
using Events.Contract.Interfaces;
using Events.Application.Behaviors;

namespace Events.Infrastructure;


public static class EventDepedencyInjection
{
      public static IServiceCollection AddEvents(
        this IServiceCollection services,
        IConfiguration configuration)
      {

            services.AddScoped<IEventRepository,EventRepository>();
            services.AddScoped<IEvent,EventBehavior>();


            // ==========================
            // Database
            // ==========================
            services.AddDbContext<EventDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }

}