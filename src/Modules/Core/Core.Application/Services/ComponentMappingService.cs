using Core.Application.Interfaces;
using Core.Contract.Interfaces;

namespace Core.Application.Services;

public sealed class ComponentMappingService(IComponentMappingRepository repo) : IComponentMapping
{
      public async Task<int> GetFreeIdByMacAndEntityAndVendorAsync(string mac, string entity, string vendor, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}