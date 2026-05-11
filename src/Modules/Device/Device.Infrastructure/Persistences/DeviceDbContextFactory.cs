

using Device.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Device.Infrastructure.Persistences;



public sealed class DeviceDbContextFactory : IDesignTimeDbContextFactory<DeviceDbContext>
{
  public DeviceDbContext CreateDbContext(string[] args)
  {
    // VERY IMPORTANT: move up to API project folder
    var basePath = Directory.GetCurrentDirectory();

    var config = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = config.GetConnectionString("PostgresConnection");

    var optionsBuilder = new DbContextOptionsBuilder<DeviceDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    return new DeviceDbContext(optionsBuilder.Options);
  }
}
