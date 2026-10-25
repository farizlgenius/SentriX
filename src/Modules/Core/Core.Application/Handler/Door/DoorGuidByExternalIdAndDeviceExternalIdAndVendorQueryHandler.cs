using Core.Application.Interfaces;
using Core.Contract.Queries.DeviceModule;
using Core.Contract.Queries.Door;
using SharedKernel.Messaging;

namespace Core.Application.Handler.DeviceModule;

public sealed class DoorGuidByExternalIdAndDeviceExternalIdAndVendorQueryHandler(IDoorRepository repo) : IQueryHandler<DoorGuidByExternalIdAndDeviceExternalIdAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(DoorGuidByExternalIdAndDeviceExternalIdAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetGuidByExternalIdAndDeviceExternalIdAndVendorAsync(query.externalId,query.deviceExternalId,query.vendor,ct);
      }
}