using Setting.Contract.DTOs;
using SharedKernel.Domain;

namespace Setting.Contract.Interfaces;

public interface ICardFormat
{
      Task<CardFormatDto> CreateAsync(CreateCardFormatDto dto,CancellationToken cancellationToken = default);
      Task<Pagination<CardFormatDto>> GetCardFormatPaginationAsync(PaginationParams param,CancellationToken cancellationToken = default);
      Task<CardFormatDto> DeleteByIdAsync(int id,CancellationToken ct = default);
      Task<CardFormatDto> UpdateAsync(CardFormatDto dto,CancellationToken ct = default);
      Task<CardFormatDto> GetByIdAsync(int id,CancellationToken ct = default);
      
}