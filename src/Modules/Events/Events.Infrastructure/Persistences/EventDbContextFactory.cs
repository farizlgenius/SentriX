using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Events.Infrastructure.Persistences;

public sealed class EventDbContextFactory : IDesignTimeDbContextFactory<EventDbContext>
{
  public EventDbContext CreateDbContext(string[] args)
  {
    // VERY IMPORTANT: move up to API project folder
    var basePath = Directory.GetCurrentDirectory();

    var config = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = config.GetConnectionString("PostgresConnection");

    var optionsBuilder = new DbContextOptionsBuilder<EventDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    return new EventDbContext(optionsBuilder.Options);
  }
}
