using Core.Contract.DTOs.Alert;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IAlert : IBase<AlertDto,CreateAlertDto,UpdateAlertDto>
{
      Task<Pagination<AlertEventDto>> GetEventPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<Pagination<AlertLevelDto>> GetLevelPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<bool> DeleteLevelByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<Guid> CreateLevelAsync(CreateAlertLevelDto dto,CancellationToken ct = default);
      Task<Guid> UpdateLevelAsync(UpdateAlertLevelDto dto,CancellationToken ct = default);

}