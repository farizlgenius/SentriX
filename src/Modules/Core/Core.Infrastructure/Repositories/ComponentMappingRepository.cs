using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;

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

  public async Task GetFreeIdByMacAndEntityAndVendorAsync(string mac, string entity, string vendor, CancellationToken ct = default)
  {
    return await conte
  }

}