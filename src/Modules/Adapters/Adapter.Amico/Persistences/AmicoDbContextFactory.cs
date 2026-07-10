

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Adapter.Amico.Persistences;



public sealed class AmicoDbContextFactory : IDesignTimeDbContextFactory<AmicoDbContext>
{
  public AmicoDbContext CreateDbContext(string[] args)
  {
    // VERY IMPORTANT: move up to API project folder
    var basePath = Directory.GetCurrentDirectory();

    var config = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = config.GetConnectionString("PostgresConnection");

    var optionsBuilder = new DbContextOptionsBuilder<AmicoDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    return new AmicoDbContext(optionsBuilder.Options);
  }
}
