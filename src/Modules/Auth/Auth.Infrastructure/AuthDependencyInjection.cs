using System;
using Auth.Application.Behaviors;
using Auth.Application.Interfaces;
using Auth.Application.Services;
using Auth.Application.ValueObject;
using Auth.Contract.Interfaces;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure;

public static class AuthDependencyInjection
{
      public static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration)
      {
            services.AddOptions<JwtSetting>().Bind(configuration.GetSection("Jwt")).ValidateOnStart();

            services.AddSingleton<IJwtSetting>(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<JwtSetting>>().Value);
                  
            services.AddScoped<IAuth,AuthBehaviors>();
            services.AddScoped<IRefreshTokenAuditRepository,RefreshTokenAuditRepository>();
            services.AddScoped<IApiKeyRepository,ApiKeyRepository>();
            services.AddScoped<IJwt,JwtService>();

            // ==========================
            // Database
            // ==========================
            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            return services;
      }

}
