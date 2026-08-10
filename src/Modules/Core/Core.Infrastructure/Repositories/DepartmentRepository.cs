using Core.Application.Interfaces;
using Core.Contract.DTOs.Department;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Infrastructure.Repositories;

public sealed class DepartmentRepository : IDepartmentRepository
{
  public async Task AddAsync(Department entity, CancellationToken ct = default)
  {
    var d = new Core.Domain.Entities.Department(
      entity.Name,
      entity.Description,
      entity.CompanyGuid
    );
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

  public Task<DepartmentDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task<Pagination<DepartmentDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> IsAnyByNameAsync(string name, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task UpdateAsync(Department entity, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}