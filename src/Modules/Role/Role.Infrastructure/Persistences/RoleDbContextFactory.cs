using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Role.Infrastructure.Persistences;


public sealed class RoleDbContextFactory : IDesignTimeDbContextFactory<RoleDbContext>
{
  public RoleDbContext CreateDbContext(string[] args)
  {
    // VERY IMPORTANT: move up to API project folder
    var basePath = Directory.GetCurrentDirectory();

    var config = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = config.GetConnectionString("PostgresConnection");

    var optionsBuilder = new DbContextOptionsBuilder<RoleDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    return new RoleDbContext(optionsBuilder.Options);
  }
}