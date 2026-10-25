using Core.Application.Interfaces;
using Core.Contract.Queries.DeviceModule;
using SharedKernel.Messaging;

namespace Core.Application.Handler.DeviceModule;

public sealed class DeviceModuleGuidByExternalIdAndDeviceExternalIdAndVendorQueryHandler(IDeviceModuleRepository repo) : IQueryHandler<DeviceModuleGuidByExternalIdAndDeviceExternalIdAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(DeviceModuleGuidByExternalIdAndDeviceExternalIdAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetGuidByExternalIdAndDeviceExternalIdAndVendorAsync(query.externalId,query.deviceExternalId,query.vendor,ct);
      }
}