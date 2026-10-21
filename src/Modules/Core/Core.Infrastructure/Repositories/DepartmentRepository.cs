using Core.Application.Interfaces;
using Core.Contract.DTOs.Department;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class DepartmentRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Department,Domain.Entities.Department,DepartmentDto>(
  context,
  d => new Persistences.Entities.Department(d),
  (en, dom) =>
  {
    en.name = dom.Name;
    en.description = dom.Description;
    en.company_id = dom.CompanyId;
    en.updated_at = DateTime.UtcNow;
  },
  DepartmentProjections.ToDto
),IDepartmentRepository
{





  public async Task<IEnumerable<DepartmentDto>> GetByCompanyAsync(Guid guid, CancellationToken ct = default)
  {
    return await Context.Departments
      .AsNoTracking()
      .Where(x => x.company.guid == guid)
      .OrderByDescending(x => x.id)
      .Select(x => new DepartmentDto(
        x.guid,
        x.name,
        x.description,
        x.company.guid,
        x.company.name,
        x.is_active,
        x.is_default
      )).ToArrayAsync(cancellationToken: ct);
  }



      public async Task<Pagination<DepartmentDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Pagination<DepartmentDto>> GetPaginationByCompanyGuidAsync(PaginationParams param, Guid companyGuid, CancellationToken ct = default)
  {
    return await base.GetPaginationAsync(
      param,
      x => x.company.guid == companyGuid,
      [x => x.name,x => x.description],
      ct
    );
  }


      public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = default, CancellationToken ct = default)
  {
    return await Context.Departments
      .AsNoTracking()
      .AnyAsync(x => x.name.Equals(name), cancellationToken: ct);
  }


  public async Task<bool> IsAnyNameByCompanyGuidAsync(string name, Guid guid, CancellationToken ct = default)
  {
    return await Context.Departments
      .AsNoTracking()
      .AnyAsync(x => x.name.Equals(name) && x.company.guid == guid, cancellationToken: ct);
  }


  public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    return await Context.Departments
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid && x.positions.Any(), cancellationToken: ct);
  }

  public async Task<bool> IsAnyUserAsync(Guid guid, CancellationToken ct = default)
  {
    return await Context.Companies
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid && x.users.Any(), cancellationToken: ct);
  }


}