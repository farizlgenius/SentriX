using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using User.Application.Interfaces;
using User.Contract.DTOs;
using User.Domain.Entities;
using User.Infrastructure.Persistences;

namespace User.Infrastructure.Repositories;

public sealed class CompanyRepository(UserDbContext context) : ICompanyRepository
{
      public async Task AddAsync(Company d, CancellationToken ct = default)
      {
            var entity = new Persistences.Entities.Company(d);
            await context.Companies.AddAsync(entity, ct);
            await context.SaveChangesAsync(ct);

      }


      public Task<string> CheckRelateRecordAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
           var entity = await context.Companies.FirstOrDefaultAsync(x => x.guid == guid, ct);
            if (entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);
      

           context.Companies.Remove(entity);
            await context.SaveChangesAsync(ct);

      }

      public async Task<CompanyDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Companies.AsNoTracking()
            .Where(x => x.guid == guid)
            .Select(x => new CompanyDto(
                  x.guid,
                  x.name,
                  x.address,
                  x.description,
                  x.location_id,
                  x.is_active,
                  x.is_default
            ))
            .FirstOrDefaultAsync() ?? new CompanyDto();
      }

      public async Task<IEnumerable<CompanyDto>> GetByLocationIdAsync(int LocationId, CancellationToken ct = default)
      {
             return await context.Companies.AsNoTracking()
            .Where(x => x.location_id == LocationId)
            .Select(x => new CompanyDto(
                  x.guid,
                  x.name,
                  x.address,
                  x.description,
                  x.location_id,
                  x.is_active,
                  x.is_default
            ))
            .ToArrayAsync();
      }

      public async Task<Pagination<CompanyDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Companies.AsNoTracking().Where(x => x.location_id == param.locationId).AsQueryable();

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
                                  EF.Functions.ILike(x.address, pattern) ||
                                  EF.Functions.ILike(x.description, pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.address.Contains(search) ||
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
            .Select(u => new CompanyDto(
                  u.guid,
                  u.name,
                  u.address,
                  u.description,
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<CompanyDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
           
      }

      public Task<IEnumerable<OptionDto>> GetOptionByLocationAsync(int locationId, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsAnyByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Companies.AsNoTracking().AnyAsync(x => x.guid == guid);
      }

      public async Task<bool> IsAnyNameAsync(string name, CancellationToken ct = default)
      {
            return await context.Companies.AsNoTracking().AnyAsync(x => x.name.Equals(name));
      }

      public async Task<bool> IsAnyRelateAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Companies.AsNoTracking().AnyAsync(x => x.departments.Any() || x.users.Any());
      }

      public async Task UpdateAsync(Company company, CancellationToken ct = default)
      {
            var entity = await context.Companies.OrderByDescending(x => x.id)
            .Where(x => x.guid == company.Guid)
            .FirstOrDefaultAsync();

            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(company);

            context.Companies.Update(entity);
            await context.SaveChangesAsync();

      }

      public Task UpdateImagePathAsync(string path, Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }



      public async Task<IEnumerable<OptionDto>> GetCompanyOptionByLocationIdAsync(int location, CancellationToken ct = default)
      {
            return await context.Companies.AsNoTracking()
            .Where(x => x.location_id == location)
            .Select(x => new OptionDto(
                  x.name,
                  x.id,
                  x.description,
                  x.guid,
                  false
            )).ToArrayAsync();
      }
}