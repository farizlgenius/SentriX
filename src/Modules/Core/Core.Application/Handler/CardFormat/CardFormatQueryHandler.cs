using Core.Application.Interfaces;
using Core.Contract.DTOs.CardFormat;
using Core.Contract.Queries.CardFormat;
using SharedKernel.Messaging;

namespace Core.Application.Handler.CardFormat;

public sealed class CardFormatQueryHandler(ICardFormatRepository repo) : IQueryHandler<CardFormatQuery, IEnumerable<CardFormatDto>>
{
      public async Task<IEnumerable<CardFormatDto>> HandleAsync(CardFormatQuery query, CancellationToken ct)
      {
           return await repo.GetAllAsync();
      }
}