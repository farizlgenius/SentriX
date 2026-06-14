using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.Application.Behaviors;
using User.Application.Interfaces;
using User.Contract.Interfaces;
using User.Infrastructure.Persistences;
using User.Infrastructure.Repositories;

namespace User.Infrastructure;

public static class UserDependencyInjection
{
       public static IServiceCollection AddUser(
        this IServiceCollection services,
        IConfiguration configuration)
      {

            services.AddScoped<IUser,UserBehavior>();
            services.AddScoped<IUserRepository,UserRepository>();

            // ==========================
            // Database
            // ==========================
            services.AddDbContext<UserDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }
}