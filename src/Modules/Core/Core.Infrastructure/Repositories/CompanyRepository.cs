using Core.Application.Interfaces;
using Core.Contract.DTOs.Company;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class CompanyRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Company, Domain.Entities.Company, CompanyDto>(
  context,
  d => new Persistences.Entities.Company(d),
  (en, dom) =>
  {
    en.name = dom.Name;
    en.address = dom.Address;
    en.description = dom.Description;
    en.updated_at = DateTime.UtcNow;
    en.location_id = dom.LocationId;
  },
  CompanyProjections.ToDto,
  CompanyProjections.ToCommand
), ICompanyRepository
{


  public async Task<IEnumerable<CompanyDto>> GetAsync()
  {
    return await Context.Companies
      .AsNoTracking()
      .OrderByDescending(x => x.id)
      .Select(x => new CompanyDto(
        x.guid,
        x.name,
        x.address,
        x.description,
        x.is_active,
        x.is_default
      ))
      .ToArrayAsync();
  }



  public async Task<Pagination<CompanyDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await base.GetPaginationAsync(param,
    param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location == null || x.location.guid == param.locationGuid : null,
    [x => x.name, x => x.description, x => x.address],
    ct);
  }



  public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    return await Context.Companies
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid && x.departments.Any(), cancellationToken: ct);
  }

  public async Task<bool> IsAnyUserAsync(Guid guid, CancellationToken ct = default)
  {
    return await Context.Companies
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid && x.users.Any(), cancellationToken: ct);
  }




}