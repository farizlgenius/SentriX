using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Constants;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class DeviceModuleIdsMapGuidsByGuidsQueryHandler(IDeviceModuleRepository repo) : IQueryHandler<DeviceModuleIdsMapGuidsByGuidsQuery, Dictionary<Guid, int>>
{
  public async Task<Dictionary<Guid, int>> HandleAsync(DeviceModuleIdsMapGuidsByGuidsQuery query, CancellationToken ct)
  {
    foreach (var guid in query.guids)
    {
      if (!await repo.IsAnyGuidAsync(guid, ct))
        throw new NotFoundException(EntityType.DeviceModule, guid.ToString());
    }

    return await repo.GetDeviceModuleIdsMapGuidsByGuidsAsync(query.guids, ct);
  }
}