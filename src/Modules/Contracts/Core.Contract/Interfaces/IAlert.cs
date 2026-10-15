using Core.Contract.DTOs.Alert;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IAlert : IBase<AlertDto,CreateAlertDto,UpdateAlertDto>
{
      Task<Pagination<AlertEventDto>> GetEventPaginationAsync(PaginationParams param,CancellationToken ct = default);
}