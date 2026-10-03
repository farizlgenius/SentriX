using Core.Application.Interfaces;
using Core.Contract.DTOs.CardFormat;
using Core.Contract.Interfaces;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class CardFormatService(ICardFormatRepository card) : ICardFormat
{
      public Task<Guid> CreateAsync(CreateCardFormatDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<CardFormatDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<CardFormatDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<Pagination<CardFormatDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await card.GetPaginationAsync(param,ct);
      }

      public Task<Guid> UpdateAsync(UpdateCardFormatDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}