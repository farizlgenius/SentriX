using Core.Application.Interfaces;
using Core.Contract.DTOs.Time;
using Core.Contract.Queries.Time;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Times;

public sealed class IntervalByGuidsQueryHandler(IIntervalRepository repo) : IQueryHandler<IntervalByGuidsQuery, IEnumerable<IntervalDto>>
{
      public async Task<IEnumerable<IntervalDto>> HandleAsync(IntervalByGuidsQuery query, CancellationToken ct)
      {
            return await repo.GetByGuidsAsync(query.guids,ct);
      }
}