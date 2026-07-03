using Setting.Application.Interfaces;
using Setting.Contract.DTOs;
using Setting.Contract.Queries;
using SharedKernel.Messaging;

namespace Setting.Application.Handler;

public sealed class CardFormatByLocationIdQueryHandler(ICfmtRepository repo) : IQueryHandler<CardFormatByLocationIdQuery, IEnumerable<CardFormatDto>>
{
      public async Task<IEnumerable<CardFormatDto>> HandleAsync(CardFormatByLocationIdQuery query, CancellationToken ct)
      {
            return await repo.GetByLocationIdAsync(query.LocationId);
      }
}