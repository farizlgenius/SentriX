using Microsoft.EntityFrameworkCore;

namespace Time.Infrastructure.Persistences;

public sealed class TimeDbContext(DbContextOptions<TimeDbContext> options) : DbContext(options)
{
      
}