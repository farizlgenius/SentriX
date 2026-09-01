using Core.Application.Interfaces;
using Core.Contract.DTOs.Company;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class CompanyRepository(CoreDbContext context) : ICompanyRepository
{
  public async Task AddAsync(Company entity, CancellationToken ct = default)
  {
    await context.Companies.AddAsync(
      new Persistences.Entities.Company(entity), ct
    );

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Companies
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct);

    context.Companies.Remove(entity ?? throw new NotFoundException(EntityType.Company, guid.ToString()));

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var entities = await context.Companies
                  .Where(x => guids.Contains(x.guid) && x.is_default == false)
                  .ToArrayAsync(ct);

    context.Companies.RemoveRange(entities);

    await context.SaveChangesAsync(ct);
  }

  public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Companies
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Location, guid.ToString());

    en.is_active = false;

    context.Companies.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Companies
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Location, guid.ToString());

    en.is_active = true;

    context.Companies.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<CompanyDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Companies
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new CompanyDto(
                    x.guid,
                    x.name,
                    x.address,
                    x.description,
                    x.is_active,
                    x.is_default
                  )).FirstOrDefaultAsync() ?? throw new NotFoundException(nameof(Location), guid.ToString());
  }

  public async Task<IEnumerable<CompanyDto>> GetAsync()
  {
    return await context.Companies
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


  public async Task<IEnumerable<CompanyDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
  {
    return await context.Companies
      .AsNoTracking()
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

  public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    var res = await context.Companies.AsNoTracking()
      .Where(x => x.guid == guid)
      .Select(x => x.id)
      .FirstOrDefaultAsync();

    if (res == 0)
      throw new NotFoundException(EntityType.Company, guid.ToString());

    return res;
  }

  public async Task<Pagination<CompanyDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.Companies
                  .AsNoTracking()
                  .AsQueryable();

    if (!string.IsNullOrWhiteSpace(param.search))
    {
      if (!string.IsNullOrWhiteSpace(param.search))
      {
        var search = param.search.Trim();

        if (context.Database.IsNpgsql())
        {
          var pattern = $"%{search}%";

          query = query.Where(x =>
              EF.Functions.ILike(x.name, pattern) ||
              EF.Functions.ILike(x.description, pattern) ||
              EF.Functions.ILike(x.address, pattern)
          );
        }
        else // SQL Server
        {
          query = query.Where(x =>
              x.name.Contains(search) ||
              x.description.Contains(search) ||
              x.address.Contains(search)
          );
        }

      }
    }


    if (param.startDate != null)
    {
      var startUtc = DateTime.SpecifyKind(param.startDate.Value, DateTimeKind.Utc);
      query = query.Where(x => x.created_at >= startUtc);
    }

    if (param.endDate != null)
    {
      var endUtc = DateTime.SpecifyKind(param.endDate.Value, DateTimeKind.Utc);
      query = query.Where(x => x.created_at <= endUtc);
    }

    var count = await query.CountAsync();

    var res = await query
          .AsNoTracking()
          .OrderByDescending(e => e.created_at)
          .Skip((param.pageNumber - 1) * param.pageSize)
          .Take(param.pageSize)
          .Select(e => new CompanyDto(
                e.guid,
                e.name,
                e.address,
                e.description,
                e.is_active,
                e.is_default
          )).ToListAsync();

    return new Pagination<CompanyDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
  }

  public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId, CancellationToken ct = default)
  {
    return await context.Companies
      .AsNoTracking()
      .AnyAsync(x => x.name.Equals(name));

  }

  public async Task<bool> IsAnyDepartmentAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Companies
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid && x.departments.Any());
  }

  public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Companies
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid);
  }

  public async Task<bool> IsAnyUserAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Companies
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid && x.users.Any());
  }

  public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Companies
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid && x.is_default);
  }

  public async Task UpdateAsync(Company entity, CancellationToken ct = default)
  {
    var en = await context.Companies
                  .Where(x => x.guid == entity.Guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Location, entity.Guid.ToString());

    en.name = entity.Name;
    en.description = entity.Description;
    en.address = entity.Address;

    context.Companies.Update(en);

    await context.SaveChangesAsync(ct);
  }
}