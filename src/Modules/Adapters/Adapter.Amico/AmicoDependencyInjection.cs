using System;
using Adapter.Amico.Persistences;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adapter.Amico;

public static class AmicoDependencyInjection
{
      public static IServiceCollection AddAmico(
        this IServiceCollection services,
        IConfiguration configuration)
      {
            //       services.AddHttpClient<AeroHttpClient>(c =>
            //   {
            //       c.BaseAddress = new Uri("https://aero-api/");
            //       c.Timeout = TimeSpan.FromSeconds(30);
            //   });


            // ==========================
            // Database
            // ==========================
            services.AddDbContext<AmicoDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }
}
