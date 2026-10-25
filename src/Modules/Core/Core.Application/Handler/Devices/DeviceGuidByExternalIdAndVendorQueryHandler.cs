using Core.Application.Interfaces;
using Core.Contract.Queries.Device;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Devices;

public sealed class DeviceGuidByExternalIdAndVendorQueryHandler(IDeviceRepository repo) : IQueryHandler<DeviceGuidByExternalIdAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(DeviceGuidByExternalIdAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetGuidByExternalAndVendorAsync(query.externalId,query.vendor,ct);
      }
}