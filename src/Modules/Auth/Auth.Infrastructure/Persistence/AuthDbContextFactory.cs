using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Auth.Infrastructure.Persistence;


public sealed class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
  public AuthDbContext CreateDbContext(string[] args)
  {
    // VERY IMPORTANT: move up to API project folder
    var basePath = Directory.GetCurrentDirectory();

    var config = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = config.GetConnectionString("PostgresConnection");

    var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    return new AuthDbContext(optionsBuilder.Options);
  }
}
