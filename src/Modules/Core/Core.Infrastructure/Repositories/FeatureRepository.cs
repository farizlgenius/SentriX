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
            .Select(x => new FeatureDto(
                  x.guid,
                  x.name
            )).ToArrayAsync(ct);
      }
}