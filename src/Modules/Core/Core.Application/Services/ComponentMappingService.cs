using Core.Application.Interfaces;
using Core.Contract.Interfaces;
using SharedKernel.Enums;

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
}