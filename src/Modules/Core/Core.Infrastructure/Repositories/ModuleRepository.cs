using Core.Application.Interfaces;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories;

public sealed class ModuleRepository(CoreDbContext context) : IModuleRepository
{
  public async Task<Dictionary<Guid, (int, string)>> GetMapGuidAndMapByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var res = await context.Modules
      .AsNoTracking()
      .Where(x => guids.Contains(x.guid))
      .OrderByDescending(x => x.id)
      .Select(x => new { x.guid, x.id, x.name })
      .ToArrayAsync(ct);

    return res.ToDictionary(x => x.guid, x => (x.id, x.name));
  }
}