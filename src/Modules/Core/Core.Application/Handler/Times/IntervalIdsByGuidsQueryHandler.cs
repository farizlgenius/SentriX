using Core.Application.Interfaces;
using Core.Contract.Queries;
using Core.Contract.Queries.Time;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Times;

public sealed class IntervalIdsByGuidsQueryHandler(IIntervalRepository repo) : IQueryHandler<IntervalIdsByGuidsQuery, IEnumerable<int>>
{
  public async Task<IEnumerable<int>> HandleAsync(IntervalIdsByGuidsQuery query, CancellationToken ct)
  {
    return await repo.GetIdsByGuidsAsync(query.guids, ct);
  }
}