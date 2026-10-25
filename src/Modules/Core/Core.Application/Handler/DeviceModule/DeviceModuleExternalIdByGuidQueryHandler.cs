using Core.Application.Interfaces;
using Core.Contract.Queries.DeviceModule;
using SharedKernel.Messaging;

namespace Core.Application.Handler.DeviceModule;

public sealed class DeviceModuleExternalIdByGuidQueryHandler(IDeviceModuleRepository repo) : IQueryHandler<DeviceModuleExternalIdByGuidQuery, short>
{
      public async Task<short> HandleAsync(DeviceModuleExternalIdByGuidQuery query, CancellationToken ct)
      {
            return await repo.GetExternalIdByGuidAsync(query.guid,ct);
      }
}