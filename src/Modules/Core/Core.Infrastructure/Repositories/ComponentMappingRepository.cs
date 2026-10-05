using Core.Application.Interfaces;
using Core.Contract.DTOs.Device;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class ComponentMappingRepository(CoreDbContext context) : IComponentMappingRepository
{
  public async Task InsertAsync(ComponentMappping entity, CancellationToken ct = default)
  {
    await context.ComponentMappings.AddAsync(
      new Persistences.Entities.ComponentMapping(entity)
    );

    await context.SaveChangesAsync(ct);
  }

  public async Task<int> GetExternalIdByMacAndEntityAsync(string mac, EntityType entity, CancellationToken ct = default)
  {
    var res = await context.ComponentMappings
        .AsNoTracking()
        .Where(x => x.mac == mac && x.entity == entity) // Use == for better SQL translation
        .OrderByDescending(x => x.id)
        .Select(x => (int?)x.external_id)               // Cast to int? so it returns null if not found
        .FirstOrDefaultAsync(ct);                       // Don't forget to pass your cancellation token!

    if (res == null)
      throw new NotFoundException(EntityType.ComponentMapping.ToString(), $"Mac:{mac}, Entity:{entity}");

    return res.Value; // Extract the underlying int value
  }



  public async Task<IEnumerable<int?>> GetExternalIdsByEntityAndVendorAsync(EntityType entity, Vendor vendor, CancellationToken ct = default)
  {
    return await context.ComponentMappings
      .AsNoTracking()
      .Where(x => x.entity == entity && x.vendor == vendor)
      .Select(x => x.external_id)
      .ToArrayAsync();
  }

  public async Task<int> GetFreeIdByEntityAsync(EntityType entity, int max, IEnumerable<int>? exception = default, CancellationToken ct = default)
  {
    // 1. Fetch only non-null IDs directly from the database to save memory
    var existingIds = await context.ComponentMappings
        .AsNoTracking()
        .Where(x => x.entity == entity && x.external_id != null)
        .Select(x => x.external_id!.Value) // '!' removes the warning because database filter guarantees non-null
        .ToListAsync(ct); // Pass the CancellationToken here for production safety

    // 2. Convert to a HashSet for O(1) lightning-fast lookups
    var existIds = existingIds.ToHashSet();

    // 3. FIX: Concat returns a new sequence, it does not modify the original set. 
    // We must use UnionWith to actually add the exceptions to the HashSet.
    if (exception != null)
    {
      existIds.UnionWith(exception);
    }

    for (var id = 1; id < max; id++)
    {
      if (!existIds.Contains(id))
        return id;
    }

    throw new ExceedException(entity.ToString());
  }

  public async Task<int> GetFreeIdByMacAndEntityAndVendorAsync(string mac, EntityType entity, Vendor vendor, int max, IEnumerable<int>? exception = default, CancellationToken ct = default)
  {
    // 1. Fetch only non-null IDs directly from the database to save memory
    var existingIds = await context.ComponentMappings
        .AsNoTracking()
        .Where(x => x.entity == entity && x.vendor == vendor && x.mac == mac && x.external_id != null)
        .Select(x => x.external_id!.Value) // '!' removes the warning because database filter guarantees non-null
        .ToListAsync(ct); // Pass the CancellationToken here for production safety

    // 2. Convert to a HashSet for O(1) lightning-fast lookups
    var existIds = existingIds.ToHashSet();

    // 3. FIX: Concat returns a new sequence, it does not modify the original set. 
    // We must use UnionWith to actually add the exceptions to the HashSet.
    if (exception != null)
    {
      existIds.UnionWith(exception);
    }

    for (var id = 1; id < max; id++)
    {
      if (!existIds.Contains(id))
        return id;
    }

    throw new ExceedException(entity.ToString());
  }

  public async Task<int> GetFreeIdByEntityAndVendorAsync(EntityType entity, Vendor vendor, int max, IEnumerable<int>? exception = default, CancellationToken ct = default)
  {
    // 1. Fetch only non-null IDs directly from the database to save memory
    var existingIds = await context.ComponentMappings
        .AsNoTracking()
        .Where(x => x.entity == entity && x.vendor == vendor && x.external_id != null)
        .Select(x => x.external_id!.Value) // '!' removes the warning because database filter guarantees non-null
        .ToListAsync(ct); // Pass the CancellationToken here for production safety

    // 2. Convert to a HashSet for O(1) lightning-fast lookups
    var existIds = existingIds.ToHashSet();

    // 3. FIX: Concat returns a new sequence, it does not modify the original set. 
    // We must use UnionWith to actually add the exceptions to the HashSet.
    if (exception != null)
    {
      existIds.UnionWith(exception);
    }

    for (var id = 1; id < max; id++)
    {
      if (!existIds.Contains(id))
        return id;
    }

    throw new ExceedException(entity.ToString());
  }

  public async Task<string> GetMacByExternalIdAndEntityAndVendorAsync(
    int externalId,
    EntityType entity,
    Vendor vendor,
    CancellationToken ct = default)
  {
    var res = await context.ComponentMappings
      .AsNoTracking()
      .Where(x => x.external_id == externalId && x.entity == entity && x.vendor == vendor)
      .OrderByDescending(x => x.id)
      .Select(x => x.mac)
      .FirstOrDefaultAsync();

    return res ?? string.Empty;
  }

  public async Task<int> GetExternalIdByGuidAndEntityAsync(Guid guid, EntityType entity, CancellationToken ct = default)
  {
    var res = await context.ComponentMappings
      .AsNoTracking()
      .Where(x => x.entity == entity && x.guid == guid)
      .OrderByDescending(x => x.id)
      .Select(x => x.external_id)
      .FirstOrDefaultAsync();

    if (res == null)
      throw new NotFoundException(EntityType.ComponentMapping.ToString(), guid.ToString());

    return (int)res;

  }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.ComponentMappings
              .Where(x => x.guid == guid)
              .OrderByDescending(x => x.id)
              .ToArrayAsync(ct);

            if(entity.Count() == 0)
              throw new NotFoundException(EntityType.ComponentMapping.ToString(),guid.ToString());

            context.ComponentMappings.RemoveRange(entity);

            await context.SaveChangesAsync(ct);
      }

      public async Task<Guid> GetGuidByExternalIdAndEntityAndVendorAsync(short externalId, EntityType entity, Vendor vendor, CancellationToken ct = default)
      {
          var res =  await context.ComponentMappings
            .AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.external_id == externalId && x.entity == entity && x.vendor == vendor)
            .Select(x => x.guid)
            .FirstOrDefaultAsync();

          if(res == Guid.Empty)
            throw new NotFoundException(EntityType.ComponentMapping.ToString());

          return res;
      }

      public async Task UpdateExternalIdByMacAsync(string mac, short externalId, CancellationToken ct = default)
      {
          var entity = await context.ComponentMappings
            .Where(x => x.mac != null && x.mac.Equals(mac) && x.entity == EntityType.Device)
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync();

          if(entity == null)
            throw new NotFoundException(EntityType.ComponentMapping.ToString(),mac);

          entity.external_id = externalId;

          context.ComponentMappings.Update(entity);

          await context.SaveChangesAsync(ct);
      }

      public async Task<DeviceComponentDto> GetComponentByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }


}