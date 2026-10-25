using Core.Application.Interfaces;
using Core.Contract.Queries.CardFormat;
using SharedKernel.Messaging;

namespace Core.Application.Handler.CardFormat;

public sealed class CardFormatExternalIdByGuidQueryHandler(ICardFormatRepository repo) : IQueryHandler<CardFormatExternalIdByGuidQuery, short>
{
      public async Task<short> HandleAsync(CardFormatExternalIdByGuidQuery query, CancellationToken ct)
      {
            return await repo.GetExternalIdByGuidAsync(query.guid,ct);
      }
}