

// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Setting.Application.Behaviors;
// using Setting.Application.Interfaces;
// using Setting.Contract.Interfaces;
// using Setting.Infrastructure.Persistences;
// using Setting.Infrastructure.Repositories;

// namespace Setting.Infrastructure;

// public static class SettingDependencyInjection
// {
//        public static IServiceCollection AddSetting(
//         this IServiceCollection services,
//         IConfiguration configuration)
//       {

//             services.AddScoped<ICardFormat,CardFormatService>();
//             services.AddScoped<ICfmtRepository,CfmtRepository>();


//             // ==========================
//             // Database
//             // ==========================
//             services.AddDbContext<SettingDbContext>(options =>
//                 options.UseNpgsql(
//                 configuration.GetConnectionString("PostgresConnection"),
//                 npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
//                 ));

//             return services;
//       }
// }
