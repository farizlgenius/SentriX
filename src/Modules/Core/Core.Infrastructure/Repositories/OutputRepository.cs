using Core.Application.Interfaces;
using Core.Contract.DTOs.Output;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using SharedKernel.Domain;

namespace Core.Infrastructure.Repositories;

public sealed class OutputRepository(CoreDbContext context) : IBaseRepository<OutputDto, Output>
{
  public async Task AddAsync(Output entity, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<OutputDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<OutputDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Pagination<OutputDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task UpdateAsync(Output entity, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}