using Core.Application.Interfaces;
using Core.Contract.DTOs.AdapterEvent;
using Core.Contract.Queries;
using Core.Domain.Entities;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class InsertAdapterEventQueryHandler(
  IAdapterEventRepository repo,
  IDeviceRepository dev
  ) : IQueryHandler<InsertAdapterEventQuery, bool>
{
  public async Task<bool> HandleAsync(InsertAdapterEventQuery query, CancellationToken ct)
  {

    var data = await dev.GetNameAndLocationIdByMacAsync(query.res.Mac, ct);
    var d = new AdapterEvent(
      data.Item1,
      query.res.Mac,
      query.res.ScpId,
      query.res.Command,
      query.res.Tag,
      query.res.SendAt,
      query.res.ReceivedAt,
      query.res.Body ?? string.Empty,
      query.res.Status,
      query.res.Reason,
      string.Empty,
      query.res.Vendor,
      data.Item2
    );

    await repo.AddAsync(d, ct);

    return true;
  }
}