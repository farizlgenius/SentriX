using Core.Application.Interfaces;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class ComponentMappingService(IComponentMappingRepository repo) : IComponentMapping
{

      public async Task<int?> GetFreeIdByMacAndEntityAndVendorAsync(
        string entity,
        Vendor vendor,
        int max,
        IEnumerable<int>? exception = default,
        CancellationToken ct = default)
      {
            var ids = 0;

            var existingIds = await repo.GetExternalIdsByEntityAndVendorAsync(
                entity,
                vendor);

            if(exception != null && exception.Any())
                  existingIds.Concat(exception);

            var usedIds = existingIds.ToHashSet();

            if (entity.Equals(EntityType.Device))
            {
                  ids = 1;
            }

            for (var id = ids; id < max; id++)
            {
                  if (!usedIds.Contains(id))
                        return id;
            }

            return null;
      }

      public async Task<int>  GetExternalIdByMacAndEntityAsync(string mac, string entityType, CancellationToken ct = default)
      {
            var res = await repo.GetExternalIdByMacAndEntityAsync(mac,entityType);

            if(res == 0)
                  throw new NotFoundException(EntityType.ComponentMapping,$"Mac:{mac},Entity:{entityType}");

            return res;
      }

      public async Task InsertComponentMappingAsync(
            string entity,
            int internalId,
            int externalId,
            string mac,
            Vendor vendor,
            int locationId,
            CancellationToken ct = default
      )
      {
            var d = new ComponentMappping(
                  entity,
                  internalId,
                  externalId,
                  mac,
                  vendor,
                  locationId
            );

            await repo.AddAsync(d,ct);
      }

      public async Task<string> GetMacByExternalIdAndEntityAndVendorAsync(int externalId,string entity,Vendor vendor,CancellationToken ct = default)
      {
            return await repo.GetMacByExternalIdAndEntityAndVendorAsync(externalId,entity,vendor,ct);
      }
}