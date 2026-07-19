using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using User.Application.Interfaces;
using User.Contract.DTOs;
using User.Domain.Entities;
using User.Infrastructure.Persistences;

namespace User.Infrastructure.Repositories;

public sealed class PositionRepository(UserDbContext context) : IPositionRepository
{
      public async Task AddAsync(Position domain, CancellationToken ct = default)
      {
            var d = new Persistences.Entities.Position(domain);
            await context.Positions.AddAsync(d,ct);
            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Positions.FirstOrDefaultAsync(x => x.guid == guid, ct);
            if (entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);
      

           context.Positions.Remove(entity);
            await context.SaveChangesAsync(ct);
      }

      public async Task<PositionDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Positions.AsNoTracking()
            .Where(x => x.guid == guid)
            .Select(x => new PositionDto(
                  x.guid,
                  x.name,
                  x.description,
                  x.department_guid,
                  x.location_id,
                  x.is_active,
                  x.is_default
            )).FirstOrDefaultAsync() ?? new PositionDto(); 
      }

      public async Task<Pagination<PositionDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Positions.AsNoTracking().Where(x => x.location_id == param.locationId).AsQueryable();

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

            var totalItems = await query.CountAsync();
            var items = await query.OrderByDescending(r => r.id)
            .Skip((param.pageNumber - 1) * param.pageSize)
            .Take(param.pageSize)
            .Select(u => new PositionDto(
                  u.guid,
                  u.name,
                  u.description,
                  u.department_guid,
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<PositionDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }

      public async Task<IEnumerable<OptionDto>> GetPositionOptionByDepartmentGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Positions.AsNoTracking()
            .Where(x => x.department_guid == guid)
            .Select(x => new OptionDto(
                  x.name,
                  x.id,
                  x.description,
                  x.guid,
                  false
            )).ToArrayAsync();
      }

      public async Task<Pagination<PositionDto>> GetPositionPaginationByDepartmentGuidAsync(PaginationParams param, Guid guid, CancellationToken ct = default)
      {
            var query = context.Positions.AsNoTracking().Where(x => x.location_id == param.locationId && x.department_guid == guid).AsQueryable();

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

            var totalItems = await query.CountAsync();
            var items = await query.OrderByDescending(r => r.id)
            .Skip((param.pageNumber - 1) * param.pageSize)
            .Take(param.pageSize)
            .Select(u => new PositionDto(
                  u.guid,
                  u.name,
                  u.description,
                  u.department_guid,
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<PositionDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }

      public async Task<bool> IsAnyByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Positions.AsNoTracking().AnyAsync(x => x.guid == guid);
      }

      public async Task<bool> IsAnyNameAsync(Guid departmentGuid, string name, CancellationToken ct = default)
      {
            return await context.Positions.AsNoTracking().AnyAsync(x => x.department_guid == departmentGuid && x.name.Equals(name));
      }

      public async Task<bool> IsAnyRelateAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Positions.AsNoTracking().AnyAsync(x => x.users.Any());
      }

      public async Task UpdateAsync(Position domain, CancellationToken ct = default)
      {
            var entity = await context.Positions.OrderByDescending(x => x.id)
            .Where(x => x.guid == domain.Guid)
            .FirstOrDefaultAsync();

            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(domain);

            context.Positions.Update(entity);
           await context.SaveChangesAsync();
      }
}