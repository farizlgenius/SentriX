using Core.Application.Interfaces;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories;

public sealed class GroupRepository(CoreDbContext context) : IGroupRepository
{
      public async Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await context.Groups
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .OrderByDescending(x => x.id)
                  .Select(x => x.id)
                  .ToListAsync();
      }
}