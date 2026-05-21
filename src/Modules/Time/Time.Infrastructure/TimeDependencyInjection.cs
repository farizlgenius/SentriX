using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Time.Application.Behaviors;
using Time.Application.Interfaces;
using Time.Contract.Interfaces;
using Time.Infrastructure.Persistences;
using Time.Infrastructure.Repositories;

namespace Time.Infrastructure;

public static class TimeDependencyInjection
{
       public static IServiceCollection AddTime(
        this IServiceCollection services,
        IConfiguration configuration)
      {

            services.AddScoped<ITime,TimeBehavior>();
            services.AddScoped<IHolidayRepository,HolidayRepository>();
            services.AddScoped<ITimezoneRepository,TimezoneRepository>();

            // ==========================
            // Database
            // ==========================
            services.AddDbContext<TimeDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }
}