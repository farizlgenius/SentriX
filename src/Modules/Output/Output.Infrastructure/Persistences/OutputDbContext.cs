using System;
using Microsoft.EntityFrameworkCore;

namespace Output.Infrastructure.Persistences;

public sealed class OutputDbContext(DbContextOptions<OutputDbContext> options) : DbContext(options)
{

}
