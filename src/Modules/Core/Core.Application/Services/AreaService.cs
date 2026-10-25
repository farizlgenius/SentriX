using Core.Contract.DTOs.Area;
using Core.Contract.Interfaces;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class AreaService : IArea
{
      public Task<Guid> CreateAsync(CreateAreaDto dto, CancellationToken ct = default)
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

      public Task<AreaDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<AreaDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Pagination<AreaDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Guid> UpdateAsync(UpdateAreaDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}