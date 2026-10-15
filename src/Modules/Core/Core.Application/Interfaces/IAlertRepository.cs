using Core.Contract.DTOs.Alert;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Interfaces;

public interface IAlertRepository : IBaseRepository<AlertDto,Alert>
{
      Task<Pagination<AlertEventDto>> GetEventPaginationAsync(PaginationParams param,CancellationToken ct = default);
}