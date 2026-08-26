using Core.Application.Interfaces;
using Core.Contract.DTOs.Feature;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories;

public sealed class FeatureRepository(CoreDbContext context) : IFeatureRepository
{
      public async Task<IEnumerable<FeatureDto>> GetAsync(CancellationToken ct = default)
      {
            return await context.Features
            .AsNoTracking()
            .OrderByDescending(x => x.id)
            .Select(x => new FeatureDto(
                  x.guid,
                  x.name,
                  x.module.name
            )).ToArrayAsync(ct);
      }

      public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Features
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .OrderByDescending(x => x.id)
                  .Select(x => x.id)
                  .FirstOrDefaultAsync();
      }

      public async Task<Dictionary<Guid, (int, string)>> GetMapIdGuidByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            var res = await context.Features
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .OrderByDescending(x => x.id)
                  .Select(x => new { x.guid, x.id, x.name })
                  .ToArrayAsync();

            return res.ToDictionary(x => x.guid, x => (x.id, x.name));
      }
}