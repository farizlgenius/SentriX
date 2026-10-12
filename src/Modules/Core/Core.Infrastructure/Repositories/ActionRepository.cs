using Core.Application.Interfaces;
using Core.Contract.DTOs.Action;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Infrastructure.Repositories;

public sealed class ActionRepository : IActionRepository
{
      public Task AddAsync(Actions entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<ActionDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<ActionDto>> GetByLocationAsync(Guid locationGuid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Guid> GetGuidByIdAsync(int id, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<string> GetNameByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Pagination<ActionDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task UpdateAsync(Actions entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}