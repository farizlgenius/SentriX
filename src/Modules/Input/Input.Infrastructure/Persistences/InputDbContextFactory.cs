using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Input.Infrastructure.Persistences;


public sealed class OutputDbContextFactory : IDesignTimeDbContextFactory<InputDbContext>
{
  public InputDbContext CreateDbContext(string[] args)
  {
    // VERY IMPORTANT: move up to API project folder
    var basePath = Directory.GetCurrentDirectory();

    var config = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = config.GetConnectionString("PostgresConnection");

    var optionsBuilder = new DbContextOptionsBuilder<InputDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    return new InputDbContext(optionsBuilder.Options);
  }
}
