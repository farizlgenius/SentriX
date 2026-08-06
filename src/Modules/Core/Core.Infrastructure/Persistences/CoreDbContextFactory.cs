using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Core.Infrastructure.Persistences;

public sealed class CoreDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
{
  public CoreDbContext CreateDbContext(string[] args)
  {
    // VERY IMPORTANT: move up to API project folder
    var basePath = Directory.GetCurrentDirectory();

    var config = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = config.GetConnectionString("PostgresConnection");

    var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    return new CoreDbContext(optionsBuilder.Options);
  }
}

