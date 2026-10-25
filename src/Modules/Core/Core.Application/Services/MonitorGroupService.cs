using Core.Contract.DTOs.MonitorGroup;
using Core.Contract.Interfaces;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class MonitorGroupService : IMonitorGroup
{
      public Task<Guid> CreateAsync(CreateMonitorGroupDto dto, CancellationToken ct = default)
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

      public Task<MonitorGroupDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<MonitorGroupDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Pagination<MonitorGroupDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Guid> UpdateAsync(UpdateMonitorGroupDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}