using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Group.Infrastructure.Persistences;

public sealed class GroupDbContextFactory : IDesignTimeDbContextFactory<GroupDbContext>
{
      public GroupDbContext CreateDbContext(string[] args)
      {
            // VERY IMPORTANT: move up to API project folder
            var basePath = Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("PostgresConnection");

            var optionsBuilder = new DbContextOptionsBuilder<GroupDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new GroupDbContext(optionsBuilder.Options);
      }
}