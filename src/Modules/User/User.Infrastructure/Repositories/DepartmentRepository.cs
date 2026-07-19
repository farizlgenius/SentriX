using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using User.Application.Interfaces;
using User.Contract.DTOs;
using User.Domain.Entities;
using User.Infrastructure.Persistences;

namespace User.Infrastructure.Repositories;

public sealed class DepartmentRepository(UserDbContext context) : IDepartmentRepository
{
      public async Task AddAsync(Department domain, CancellationToken ct = default)
      {
            var entity = new Persistences.Entities.Department(domain);
            await context.Departments.AddAsync(entity,ct);
            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
             var entity = await context.Departments.FirstOrDefaultAsync(x => x.guid == guid, ct);
            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

           context.Departments.Remove(entity);
          await context.SaveChangesAsync(ct);

      }

      public async Task<DepartmentDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Departments.AsNoTracking()
            .Where(x => x.guid == guid)
            .Select(x => new DepartmentDto(
                  x.guid,
                  x.name,
                  x.description,
                  x.company_guid,
                  x.location_id,
                  x.is_active,
                  x.is_default
            )).FirstOrDefaultAsync() ?? new DepartmentDto();
      }

      public async Task<IEnumerable<DepartmentDto>> GetDepartmentByCompanyGuidAsync(Guid companyGuid, CancellationToken ct = default)
      {
            var res = await context.Departments.AsNoTracking()
            .Where(x => x.company_guid == companyGuid)
            .Select(x => new DepartmentDto(
                  x.guid,
                  x.name,
                  x.description,
                  x.company_guid,
                  x.location_id,
                  x.is_active
            )).ToArrayAsync();

            return res;
      }

      public async Task<Pagination<DepartmentDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
             var query = context.Departments.AsNoTracking().Where(x => x.location_id == param.locationId).AsQueryable();

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
            .Select(u => new DepartmentDto(
                  u.guid,
                  u.name,
                  u.description,
                  u.company_guid,
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<DepartmentDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }

      public async Task<Pagination<DepartmentDto>> GetPaginationByCompanyGuidAsync(PaginationParams param, Guid guid, CancellationToken ct = default)
      {
             var query = context.Departments.AsNoTracking().Where(x => x.location_id == param.locationId && x.company_guid == guid).AsQueryable();

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
            .Select(u => new DepartmentDto(
                  u.guid,
                  u.name,
                  u.description,
                  u.company_guid,
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<DepartmentDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }



      public async Task<bool> IsAnyByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Departments.AsNoTracking().AnyAsync(x => x.guid == guid);
      }

      public async Task<bool> IsAnyNameAsync(Guid companyGuid,string name, CancellationToken ct = default)
      {
            return await context.Departments.AsNoTracking().AnyAsync(x => x.name.Equals(name) && x.company_guid == companyGuid,ct);
      }

      public async Task<bool> IsAnyRelateAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Departments.AsNoTracking().AnyAsync(x => x.positions.Any() || x.users.Any());
      }

      public async Task UpdateAsync(Department domain, CancellationToken ct = default)
      {
            var entity = await context.Departments.OrderByDescending(x => x.id)
            .Where(x => x.guid == domain.Guid)
            .FirstOrDefaultAsync();

            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(domain);

            context.Departments.Update(entity);
            await context.SaveChangesAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetDepartmentOptionByCompanyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Departments.AsNoTracking()
            .Where(x => x.company_guid == guid)
            .Select(x => new OptionDto(
                  x.name,
                  x.id,
                  string.Empty,
                  x.guid,
                  false
                  )).ToArrayAsync();
      }
}