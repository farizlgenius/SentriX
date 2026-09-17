using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;

namespace Core.Infrastructure.Repositories;

public sealed class ComponentMappingRepository(CoreDbContext context) : IComponentMappingRepository
{
  public async Task AddAsync(ComponentMappping entity, CancellationToken ct = default)
  {
    await context.ComponentMappings.AddAsync(
      new Persistences.Entities.ComponentMapping(entity)
    );

    await context.SaveChangesAsync(ct);
  }

      public async Task<int> GetExternalIdByMacAndEntityAsync(string mac, string entity, CancellationToken ct = default)
      {
            return await context.ComponentMappings
              .AsNoTracking()
              .Where(x => x.mac.Equals(mac) && x.entity.Equals(entity))
              .Select(x => x.external_id)
              .FirstOrDefaultAsync();
      }

      public Task GetExternalIdByMacAsync(string mac, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<int>> GetExternalIdsByEntityAndVendorAsync(string entity, Vendor vendor, CancellationToken ct = default)
  {
    return await context.ComponentMappings
      .AsNoTracking()
      .Where(x => x.entity.Equals(entity) && x.vendor == vendor)
      .Select(x => x.external_id)
      .ToArrayAsync();
  }

  public async Task<int> GetFreeIdByMacAndEntityAndVendorAsync(string mac, string entity, Vendor vendor, int max, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task GetFreeIdByMacAsync(string mac, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}