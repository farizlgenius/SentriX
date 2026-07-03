using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Setting.Infrastructure.Persistences;


public sealed class SettingDbContextFactory : IDesignTimeDbContextFactory<SettingDbContext>
{
  public SettingDbContext CreateDbContext(string[] args)
  {
    // VERY IMPORTANT: move up to API project folder
    var basePath = Directory.GetCurrentDirectory();

    var config = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = config.GetConnectionString("PostgresConnection");

    var optionsBuilder = new DbContextOptionsBuilder<SettingDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    return new SettingDbContext(optionsBuilder.Options);
  }
}