using Device.Application.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class ModuleIdByMacAndAddressQueryHandler(IDeviceRepository repo) : IQueryHandler<ModuleIdByMacAndAddressQuery, int>
{
      public async Task<int> HandleAsync(ModuleIdByMacAndAddressQuery query, CancellationToken ct)
      {
            return await repo.GetModuleIdByMacAndAddressAsync(query.Mac,query.Address);
      }
}