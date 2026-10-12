using Core.Contract.DTOs.Action;
using Core.Contract.Interfaces;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class ActionService : IAction
{
      public Task<Guid> CreateAsync(CreateActionDto dto, CancellationToken ct = default)
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

      public Task<ActionDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<ActionDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Pagination<ActionDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Guid> UpdateAsync(UpdateActionDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}