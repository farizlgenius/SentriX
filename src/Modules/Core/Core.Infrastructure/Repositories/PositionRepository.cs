using Core.Application.Interfaces;
using Core.Contract.DTOs.Position;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class PositionRepository(CoreDbContext context) : IPositionRepository
{
      public async Task AddAsync(Position entity, CancellationToken ct = default)
      {
            await context.Positions.AddAsync(
              new Persistences.Entities.Position(entity), ct
            );

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Positions
              .Where(x => x.guid == guid)
              .FirstOrDefaultAsync(ct);

            context.Positions.Remove(entity ?? throw new NotFoundException(EntityType.Position, guid.ToString()));

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            var entities = await context.Positions
                          .Where(x => guids.Contains(x.guid) && x.is_default == false)
                          .ToArrayAsync(ct);

            context.Positions.RemoveRange(entities);

            await context.SaveChangesAsync(ct);
      }

      public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.Positions
              .Where(x => x.guid == guid)
              .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Location, guid.ToString());

            en.is_active = false;

            context.Positions.Update(en);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.Positions
              .Where(x => x.guid == guid)
              .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Location, guid.ToString());

            en.is_active = true;

            context.Positions.Update(en);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<PositionDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Positions
                          .AsNoTracking()
                          .Where(x => x.guid == guid)
                          .Select(x => new PositionDto(
                            x.guid,
                            x.name,
                            x.description,
                            x.department_guid,
                            x.is_active,
                            x.is_default
                          )).FirstOrDefaultAsync() ?? throw new NotFoundException(nameof(Location), guid.ToString());
      }

      public async Task<Pagination<PositionDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Positions
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
                  .Select(e => new PositionDto(
                        e.guid,
                        e.name,
                        e.description,
                        e.department_guid,
                        e.is_active,
                        e.is_default
                  )).ToListAsync();

            return new Pagination<PositionDto>(
                  param.pageNumber,
                  param.pageSize,
                  count,
                  (int)Math.Ceiling(count / (double)param.pageSize),
                  res
                  );
      }

      public async Task<Pagination<PositionDto>> GetPaginationByDepartmentGuidAsync(PaginationParams param, Guid guid, CancellationToken ct = default)
      {
            var query = context.Positions
                          .AsNoTracking()
                          .Where(x => x.department_guid == guid)
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
                  .Select(e => new PositionDto(
                        e.guid,
                        e.name,
                        e.description,
                        e.department_guid,
                        e.is_active,
                        e.is_default
                  )).ToListAsync();

            return new Pagination<PositionDto>(
                  param.pageNumber,
                  param.pageSize,
                  count,
                  (int)Math.Ceiling(count / (double)param.pageSize),
                  res
                  );
      }

      public async Task<bool> IsAnyByNameAsync(string name, CancellationToken ct = default)
      {
            return await context.Positions
              .AsNoTracking()
              .AnyAsync(x => x.name.Equals(name));
      }

      public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Positions
                          .AsNoTracking()
                          .AnyAsync(x => x.guid == guid);
      }

      public async Task<bool> IsAnyNameByDepartmentGuidAsync(string name, Guid guid, CancellationToken ct = default)
      {
            return await context.Positions
        .AsNoTracking()
        .AnyAsync(x => x.name.Equals(name) && x.department_guid == guid);
      }


      public async Task<bool> IsAnyUserAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Positions
              .AsNoTracking()
              .AnyAsync(x => x.guid == guid && x.users.Any());
      }

      public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Positions
                          .AsNoTracking()
                          .AnyAsync(x => x.guid == guid && x.is_default);
      }

      public async Task UpdateAsync(Position entity, CancellationToken ct = default)
      {
            var en = await context.Positions
                          .Where(x => x.guid == entity.Guid)
                          .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Location, entity.Guid.ToString());

            en.name = entity.Name;
            en.description = entity.Description;

            context.Positions.Update(en);

            await context.SaveChangesAsync(ct);
      }
}