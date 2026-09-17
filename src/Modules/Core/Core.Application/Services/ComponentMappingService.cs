using Core.Application.Interfaces;
using Core.Contract.Interfaces;
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
        CancellationToken ct = default)
      {
            var existingIds = await repo.GetExternalIdsByEntityAndVendorAsync(
                entity,
                vendor);

            var usedIds = existingIds.ToHashSet();

            for (var id = 0; id < max; id++)
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

      
}