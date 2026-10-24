using Core.Application.Interfaces;
using Core.Contract.DTOs.Device;
using Core.Contract.Queries.Device;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Devices;

public sealed class DeviceByExternalIdAndVendorQueryHandler(IDeviceRepository repo) : IQueryHandler<DeviceByExternalIdAndVendorQuery, DeviceDto>
{
      public async Task<DeviceDto> HandleAsync(DeviceByExternalIdAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetByExternalIdAndVendorAsync(query.externalId,query.Vendor,ct);
      }
}