using Core.Application.Interfaces;
using Core.Contract.DTOs.Operator;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class OperatorRepository(CoreDbContext context) : IOperatorRepository
{
  public async Task AddAsync(Operator entity, CancellationToken ct = default)
  {
    await context.Operators.AddAsync(
      new Persistences.Entities.Operator(entity), ct
    );

    await context.SaveChangesAsync(ct);
  }

  public async Task ChangePasswordAsync(string username, string hashed, CancellationToken ct = default)
  {
    var entity = await context.Operators
      .Where(x => x.username.Equals(username))
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Operator, username);

    entity.password = hashed;
    entity.updated_at = DateTime.UtcNow;

    context.Operators.Update(entity);

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Operators
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct);

    context.Operators.Remove(entity ?? throw new NotFoundException(EntityType.Operator, guid.ToString()));

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var entities = await context.Operators
                  .Where(x => guids.Contains(x.guid) && x.is_default == false)
                  .ToArrayAsync(ct);

    context.Operators.RemoveRange(entities);

    await context.SaveChangesAsync(ct);
  }

  public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Operators
          .Where(x => x.guid == guid)
          .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Operator, guid.ToString());

    en.is_active = false;

    context.Operators.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Operators
           .Where(x => x.guid == guid)
           .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Operator, guid.ToString());

    en.is_active = true;

    context.Operators.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<OperatorDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Operators
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .Select(x => new OperatorDto(
        x.guid,
        x.username,
        x.email,
        x.phone,
        x.active_time,
        x.expire_time,
        x.role_guid,
        x.operator_locations.Select(x => x.location_guid).ToList(),
        x.is_active,
        x.is_default
      )).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Operator, guid.ToString());
  }

  public async Task<OperatorDto> GetByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Operators
      .AsNoTracking()
      .Where(x => x.username.Equals(username))
      .Select(x => new OperatorDto(
        x.guid,
        x.username,
        x.email,
        x.phone,
        x.active_time,
        x.expire_time,
        x.role_guid,
        x.operator_locations.Select(o => o.location_guid).ToList(),
        x.is_active,
        x.is_default
      )).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Operator, username);
  }

  public async Task<Guid> GetDefaultLocationGuidAsync()
  {
    return await context.Operators
      .AsNoTracking()
      .Where(x => x.is_default)
      .Select(x => x.guid)
      .FirstOrDefaultAsync();
  }

  public async Task<string> GetHashByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Operators
      .AsNoTracking()
      .Where(x => x.username.Equals(username))
      .Select(x => x.password)
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Operator, username);
  }

  public async Task<IEnumerable<Guid>> GetLocationGuidByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Operators
      .AsNoTracking()
      .Where(x => x.username.Equals(username))
      .SelectMany(x => x.operator_locations.Select(x => x.location_guid))
      .ToArrayAsync();
  }

  public async Task<Pagination<OperatorDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.Operators
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
              EF.Functions.ILike(x.username, pattern) ||
              EF.Functions.ILike(x.email, pattern) ||
              EF.Functions.ILike(x.phone, pattern)
          );
        }
        else // SQL Server
        {
          query = query.Where(x =>
              x.username.Contains(search) ||
              x.email.Contains(search) ||
              x.phone.Contains(search)
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
          .Select(e => new OperatorDto(
                e.guid,
                e.username,
                e.email,
                e.phone,
                e.active_time,
                e.expire_time,
                e.role_guid,
                e.operator_locations.Select(x => x.location_guid).ToList(),
                e.is_active,
                e.is_default
          )).ToListAsync();

    return new Pagination<OperatorDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
  }

  public async Task<bool> IsAnyByNameAndLocationGuidAsync(string name, Guid locationGuid = default, CancellationToken ct = default)
  {
    return await context.Operators
                  .AsNoTracking()
                  .AnyAsync(x => x.username.Equals(name));
  }

  public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Operators
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid);
  }

  public async Task<bool> IsAnyUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Operators
      .AsNoTracking()
      .AnyAsync(x => x.username.Equals(username));
  }

  public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Operators
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid && x.is_default);
  }

  public async Task UpdateAsync(Operator entity, CancellationToken ct = default)
  {
    var en = await context.Operators
                  .Where(x => x.guid == entity.Guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Operator, entity.Guid.ToString());

    en.email = entity.Email;
    en.phone = entity.Phone;
    en.active_time = entity.JoinedDate;
    en.expire_time = entity.ExpiredDate;
    en.updated_at = DateTime.UtcNow;

    context.Operators.Update(en);

    await context.SaveChangesAsync(ct);
  }
}