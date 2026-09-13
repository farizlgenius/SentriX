using Core.Application.Interfaces;
using Core.Contract.Queries;
using Core.Domain.Entities;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class InsertInCommingDeviceQueryHandler(IDeviceRepository repo) : IQueryHandler<InsertInCommingDeviceQuery, bool>
{
  public async Task<bool> HandleAsync(InsertInCommingDeviceQuery query, CancellationToken ct)
  {
    var d = new Device(
      query.device.Name,
      query.device.SerialNumber,
      query.device.Mac,
      query.device.Ip,
      query.device.Port,
      query.device.Firmware,
      query.device.Vendor,
      query.device.Metadata
    );
    await repo.AddAsync(d, ct);

    return true;
  }
}