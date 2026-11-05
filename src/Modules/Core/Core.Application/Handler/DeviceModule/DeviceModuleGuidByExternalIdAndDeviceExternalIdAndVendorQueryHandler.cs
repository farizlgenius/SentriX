using Core.Application.Interfaces;
using Core.Contract.Queries.DeviceModule;
using SharedKernel.Messaging;

namespace Core.Application.Handler.DeviceModule;

public sealed class DeviceModuleGuidByExternalIdAndMacAndVendorQueryHandler(IDeviceModuleRepository repo) : IQueryHandler<DeviceModuleGuidByExternalIdAndMacAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(DeviceModuleGuidByExternalIdAndMacAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetGuidByExternalIdAndMacAndVendorAsync(query.externalId,query.mac,query.vendor,ct);
      }
}