using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace User.Infrastructure.Persistences;


public sealed class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
  public UserDbContext CreateDbContext(string[] args)
  {
    // VERY IMPORTANT: move up to API project folder
    var basePath = Directory.GetCurrentDirectory();

    var config = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = config.GetConnectionString("PostgresConnection");

    var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    return new UserDbContext(optionsBuilder.Options);
  }
}
