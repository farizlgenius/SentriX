using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Location.Infrastructure.Persistences;

public sealed class LocationDbContextFactory : IDesignTimeDbContextFactory<LocationDbContext>
{
  public LocationDbContext CreateDbContext(string[] args)
  {
    // VERY IMPORTANT: move up to API project folder
    var basePath = Directory.GetCurrentDirectory();

    var config = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = config.GetConnectionString("PostgresConnection");

    var optionsBuilder = new DbContextOptionsBuilder<LocationDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    return new LocationDbContext(optionsBuilder.Options);
  }
}
