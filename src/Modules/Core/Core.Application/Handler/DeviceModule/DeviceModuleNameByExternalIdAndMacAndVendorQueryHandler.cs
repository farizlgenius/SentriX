using Core.Application.Interfaces;
using Core.Contract.Queries.DeviceModule;
using SharedKernel.Messaging;

namespace Core.Application.Handler.DeviceModule;

public sealed class DeviceModuleNameByExternalIdAndMacAndVendorQueryHandler(IDeviceModuleRepository repo) : IQueryHandler<DeviceModuleNameByExternalIdAndMacAndVendorQuery, string>
{
      public async Task<string> HandleAsync(DeviceModuleNameByExternalIdAndMacAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetNameByExternalIdAndMacAndVendorAsync(query.externalId,query.mac,query.vendor,ct);
      }
}