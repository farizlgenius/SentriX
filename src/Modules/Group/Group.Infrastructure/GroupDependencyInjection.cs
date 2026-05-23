using Group.Application.Behaviors;
using Group.Application.Interfaces;
using Group.Contract.Interfaces;
using Group.Infrastructure.Persistences;
using Group.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Group.Infrastructure;

public static class GroupDependencyInjection
{
      public static IServiceCollection AddGroup(
        this IServiceCollection services,
        IConfiguration configuration)
      {

            services.AddScoped<IGroupRepository,GroupRepository>();
            services.AddScoped<IGroup,GroupBehavior>();


            // ==========================
            // Database
            // ==========================
            services.AddDbContext<GroupDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }
}
