using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Time.Infrastructure.Persistences;

namespace Time.Infrastructure;

public static class TimeDependencyInjection
{
       public static IServiceCollection AddTime(
        this IServiceCollection services,
        IConfiguration configuration)
      {


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