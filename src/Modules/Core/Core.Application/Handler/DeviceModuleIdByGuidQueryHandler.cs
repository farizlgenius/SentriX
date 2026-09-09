using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class DeviceModuleIdByGuidQueryHandler(IDeviceModuleRepository repo) : IQueryHandler<DeviceModuleIdByGuidQuery, int>
{
      public async Task<int> HandleAsync(DeviceModuleIdByGuidQuery query, CancellationToken ct)
      {
            return await repo.GetDeviceModuleIdByGuidAsync(query.guid,ct);
      }
}