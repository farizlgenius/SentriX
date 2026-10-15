using Core.Application.Interfaces;
using Core.Contract.DTOs.Alert;
using Core.Contract.Interfaces;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class AlertService(IAlertRepository repo) : IAlert
{
      public async Task<Guid> CreateAsync(CreateAlertDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<AlertDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<AlertDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<Pagination<AlertEventDto>> GetEventPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await repo.GetEventPaginationAsync(param,ct);
      }

      public async Task<Pagination<AlertDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await repo.GetPaginationAsync(param,ct);
      }

      public async Task<Guid> UpdateAsync(UpdateAlertDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}