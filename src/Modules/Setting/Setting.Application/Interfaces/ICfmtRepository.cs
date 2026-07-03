using Setting.Contract.DTOs;
using SharedKernel.Domain;

namespace Setting.Application.Interfaces;

public interface ICfmtRepository
{
      Task<CardFormatDto> CreateCardFormatAsync(Domain.Entities.CardFormat domain,CancellationToken ct = default);
      Task<Pagination<CardFormatDto>> GetCardFormatPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<short> GetLowestComponentIdAsync(int LocationId,CancellationToken ct = default);
      Task<CardFormatDto> DeleteByIdAsync(int id,CancellationToken ct = default);
      Task<CardFormatDto> UpdateAsync(Domain.Entities.CardFormat domain,CancellationToken ct = default);
      Task<CardFormatDto> GetByIdAsync(int id,CancellationToken ct = default);
      Task<bool> IsAnyCardFormatNotSyncAsync(int LocationId,DateTime SyncAt,CancellationToken ct = default);
      Task<IEnumerable<CardFormatDto>> GetByLocationIdAsync(int LocationId,CancellationToken ct = default);
}