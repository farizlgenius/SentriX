using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

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
    var res = await context.ComponentMappings
        .AsNoTracking()
        .Where(x => x.mac == mac && x.entity == entity) // Use == for better SQL translation
        .OrderByDescending(x => x.id)
        .Select(x => (int?)x.external_id)               // Cast to int? so it returns null if not found
        .FirstOrDefaultAsync(ct);                       // Don't forget to pass your cancellation token!

    if (res == null)
        throw new NotFoundException(EntityType.ComponentMapping, $"Mac:{mac}, Entity:{entity}");

    return res.Value; // Extract the underlying int value
}

  public Task GetExternalIdByMacAsync(string mac, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<int>> GetExternalIdsByEntityAndVendorAsync(string entity, Vendor vendor, CancellationToken ct = default)
  {
    return await context.ComponentMappings
      .AsNoTracking()
      .Where(x => x.entity == entity && x.vendor == vendor)
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

      public async Task<int> GetInternalIdByExternalIdAndEntityAndVendorAsync(short externalId, string entity, Vendor vendor, CancellationToken ct = default)
      {
           var res = await context.ComponentMappings
            .AsNoTracking()
            .Where(x => x.external_id == externalId && x.entity.Equals(entity) && x.vendor == vendor)
            .Select(x => (int?)x.internal_id)
            .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.ComponentMapping,$"external id : {externalId}, entity: {entity}, vendor: {vendor}");
              
            return res;
      }

      public async Task<string> GetMacByExternalIdAndEntityAndVendorAsync(
        int externalId,
        string entity,
        Vendor vendor, 
        CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}