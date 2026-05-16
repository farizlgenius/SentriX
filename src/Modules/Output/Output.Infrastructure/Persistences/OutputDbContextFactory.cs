using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Output.Infrastructure.Persistences;


public sealed class OutputDbContextFactory : IDesignTimeDbContextFactory<OutputDbContext>
{
  public OutputDbContext CreateDbContext(string[] args)
  {
    // VERY IMPORTANT: move up to API project folder
    var basePath = Directory.GetCurrentDirectory();

    var config = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = config.GetConnectionString("PostgresConnection");

    var optionsBuilder = new DbContextOptionsBuilder<OutputDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    return new OutputDbContext(optionsBuilder.Options);
  }
}
