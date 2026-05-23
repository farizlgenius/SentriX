using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Door.Infrastructure.Persistences;

public sealed class DoorDbContextFactory : IDesignTimeDbContextFactory<DoorDbContext>
{
      public DoorDbContext CreateDbContext(string[] args)
      {
            // VERY IMPORTANT: move up to API project folder
            var basePath = Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("PostgresConnection");

            var optionsBuilder = new DbContextOptionsBuilder<DoorDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new DoorDbContext(optionsBuilder.Options);
      }
}

