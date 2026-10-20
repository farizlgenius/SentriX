using Core.Contract.DTOs.Alert;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Interfaces;

public interface IAlertRepository : IRepository<AlertDto,Alert>
{
      Task<Pagination<AlertEventDto>> GetEventPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<Pagination<AlertLevelDto>> GetLevelPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task AddLevelAsync(AlertLevel domain,CancellationToken ct = default);
      Task DeleteLevelAsync(Guid guid,CancellationToken ct = default);
      Task<bool> IsAnyLevelRelatedEntitiesAsync(Guid guid,CancellationToken ct = default);
      Task<bool> IsAnyLevelByGuidAsync(Guid guid,CancellationToken ct = default);
      Task UpdateLevelAsync(AlertLevel domain,CancellationToken ct = default);
      Task<int> GetLevelIdByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetLevelOptionAsync(CancellationToken ct = default);
}