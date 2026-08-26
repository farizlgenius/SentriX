using Core.Application.Interfaces;
using Core.Contract.DTOs.Department;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class DepartmentRepository(CoreDbContext context) : IDepartmentRepository
{
  public async Task AddAsync(Department entity, CancellationToken ct = default)
  {
    await context.Departments.AddAsync(
      new Persistences.Entities.Department(entity), ct
    );

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Departments
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct);

    context.Departments.Remove(entity ?? throw new NotFoundException(EntityType.Department, guid.ToString()));

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var entities = await context.Departments
                  .Where(x => guids.Contains(x.guid) && x.is_default == false)
                  .ToArrayAsync(ct);

    context.Departments.RemoveRange(entities);

    await context.SaveChangesAsync(ct);
  }

  public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Departments
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Location, guid.ToString());

    en.is_active = false;

    context.Departments.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Departments
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Location, guid.ToString());

    en.is_active = true;

    context.Departments.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<DepartmentDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Departments
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new DepartmentDto(
                    x.guid,
                    x.name,
                    x.description,
                    x.company.guid,
                    x.is_active,
                    x.is_default
                  )).FirstOrDefaultAsync() ?? throw new NotFoundException(nameof(Location), guid.ToString());
  }

  public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Departments
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .OrderByDescending(x => x.id)
      .Select(x => x.id)
      .FirstOrDefaultAsync();
  }

  public async Task<Pagination<DepartmentDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.Departments
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
              EF.Functions.ILike(x.description, pattern)
          );
        }
        else // SQL Server
        {
          query = query.Where(x =>
              x.name.Contains(search) ||
              x.description.Contains(search)
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
          .Select(e => new DepartmentDto(
                e.guid,
                e.name,
                e.description,
                e.company.guid,
                e.is_active,
                e.is_default
          )).ToListAsync();

    return new Pagination<DepartmentDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
  }

  public async Task<Pagination<DepartmentDto>> GetPaginationByCompanyGuidAsync(PaginationParams param, Guid companyGuid, CancellationToken ct = default)
  {
    var query = context.Departments
                  .AsNoTracking()
                  .Where(x => x.company.guid == companyGuid)
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
              EF.Functions.ILike(x.description, pattern)
          );
        }
        else // SQL Server
        {
          query = query.Where(x =>
              x.name.Contains(search) ||
              x.description.Contains(search)
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
          .Select(e => new DepartmentDto(
                e.guid,
                e.name,
                e.description,
                e.company.guid,
                e.is_active,
                e.is_default
          )).ToListAsync();

    return new Pagination<DepartmentDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
  }

  public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = default, CancellationToken ct = default)
  {
    return await context.Departments
      .AsNoTracking()
      .AnyAsync(x => x.name.Equals(name));
  }

  public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Departments
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid);
  }

  public async Task<bool> IsAnyNameByCompanyGuidAsync(string name, Guid guid, CancellationToken ct = default)
  {
    return await context.Departments
      .AsNoTracking()
      .AnyAsync(x => x.name.Equals(name) && x.company.guid == guid);
  }

  public async Task<bool> IsAnyPositionAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Departments
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid && x.positions.Any());
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

  public async Task UpdateAsync(Department entity, CancellationToken ct = default)
  {
    var en = await context.Departments
                  .Where(x => x.guid == entity.Guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Location, entity.Guid.ToString());

    en.name = entity.Name;
    en.description = entity.Description;

    context.Departments.Update(en);

    await context.SaveChangesAsync(ct);
  }
}