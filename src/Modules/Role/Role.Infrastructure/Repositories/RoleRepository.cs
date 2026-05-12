using System;
using Location.Contract.Queries;
using Microsoft.EntityFrameworkCore;
using Role.Application.Interfaces;
using Role.Contract.DTOs;
using Role.Domain.Entities;
using Role.Infrastructure.Persistences;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Role.Infrastructure.Repositories;

public sealed class RoleRepository(RoleDbContext context,IMessageBus bus) : IRoleRepository
{
      public async Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(int RoleId, CancellationToken ct = default)
      {
            return await context.permissions.AsNoTracking()
                  .Where(p => p.role_id == RoleId)
                  .Select(p => new PermissionDto(
                        p.feature_id,
                        p.feature.name,
                        p.is_enabled,
                        p.is_created,
                        p.is_updated,
                        p.is_deleted
                  ))
                  .ToListAsync(ct);
      }

      public async Task<RoleDto> AddAsync(Roles domain)
      {
            var data = await context.roles.AddAsync(
              new Persistences.Entities.Roles(domain)
            );
            data.Entity.AddPermissions(domain.Permissions);
            var save = await context.SaveChangesAsync();

            if (data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new RoleDto(
              data.Entity.id,
              data.Entity.name,
              data.Entity.permissions
              .Select(r => new PermissionDto(r.feature_id,
                context.features.AsNoTracking().OrderByDescending(x => x.id).Where(x => x.id == r.feature_id).Select(x => x.name).FirstOrDefault() ?? "",
              r.is_enabled,
              r.is_created,
              r.is_updated,
              r.is_deleted))
              .ToList()
              );
      }

      public async Task<RoleDto> DeleteByIdAsync(int id)
      {
            var entity = await context.roles.OrderByDescending(r => r.id).Where(r => r.id == id).FirstOrDefaultAsync();
            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            var data = context.roles.Remove(entity);
            var save = await context.SaveChangesAsync();

            if (data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return new RoleDto(
              data.Entity.id,
              data.Entity.name,
              data.Entity.permissions
              .Select(r => new PermissionDto(r.feature_id,
                context.features.AsNoTracking().OrderByDescending(x => x.id).Where(x => x.id == r.feature_id).Select(x => x.name).FirstOrDefault() ?? "",
              r.is_enabled,
              r.is_created,
              r.is_updated,
              r.is_deleted))
              .ToList()
              );

      }

      public async Task<List<RoleDto>> DeleteRangeAsync(List<int> ids)
      {
            var records = await context.roles.Where(r => ids.Contains(r.id)).ToListAsync();
            if (records is null || records.Count == 0)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            context.roles.RemoveRange(records);
            var save = await context.SaveChangesAsync();
            if (save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);


            return records.Select(data => new RoleDto(
              data.id,
              data.name,
              data.permissions
              .Select(r => new PermissionDto(r.feature_id,
                context.features.AsNoTracking().OrderByDescending(x => x.id).Where(x => x.id == r.feature_id).Select(x => x.name).FirstOrDefault() ?? "",
              r.is_enabled,
              r.is_created,
              r.is_updated,
              r.is_deleted))
              .ToList()
              )).ToList();
      }

      public async Task<List<RoleDto>> GetByLocationIdAsync(int id)
      {

            return await context.roles.AsNoTracking().Where(r => r.location_id == id)
            .Select(data => new RoleDto(
              data.id,
              data.name,
              data.permissions
              .Select(r => new PermissionDto(r.feature_id,
                context.features.AsNoTracking().OrderByDescending(x => x.id).Where(x => x.id == r.feature_id).Select(x => x.name).FirstOrDefault() ?? "",
              r.is_enabled,
              r.is_created,
              r.is_updated,
              r.is_deleted))
              .ToList()
              )).ToListAsync();
      }

      public async Task<List<FeatureDto>> GetFeaturesAsync()
      {
            return await context.features.AsNoTracking().Select(x => new FeatureDto(x.id, x.name)).ToListAsync();
      }

      public async Task<Pagination<RoleDto>> GetPagination(PaginationParams param,CancellationToken ct = default)
      {
            var query = context.roles.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.name, pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) 
                              );
                        }
                  }
            }

            if (param.locationId >= 0)
            {
                  query = query.Where(x => x.location_id == param.locationId || x.location_id == 1);
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
            .Select(r => new RoleDto(r.id, r.name, r.permissions.Select(p => new PermissionDto(p.feature_id, p.feature.name, p.is_enabled, p.is_created, p.is_updated, p.is_deleted)).ToList()))
            .ToListAsync(ct);

            return new Pagination<RoleDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }

      public async Task<bool> IsAllExistByIdsAsync(List<int> ids)
      {
            var count = await context.roles
       .AsNoTracking()
       .Where(r => ids.Contains(r.id))
       .CountAsync();

            return count == ids.Count;
      }

      public async Task<bool> IsAnyLocationIdAsync(int LocationId)
      {
            return await bus.QueryAsync(new IsAnyLocationByIdQuery(LocationId));
      }

      public async Task<bool> IsAnyNameWithLocationIdAsync(int LocationId, string Name)
      {
            return await context.roles.AsNoTracking().AnyAsync(r => r.location_id == LocationId && r.name.Equals(Name));
      }

      public async Task<bool> IsAnyWithIdAsync(int id)
      {
            return await context.roles.AsNoTracking().AnyAsync(r => r.id == id);
      }

      public async Task<RoleDto> UpdateAsync(Roles domain)
      {
            var entity = await context.roles.OrderByDescending(r => r.id).Where(r => r.id == domain.Id).FirstOrDefaultAsync();
            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            // Delete Relate Permissions
            var old = await context.permissions.Where(p => p.role_id == domain.Id).ToArrayAsync();
            context.permissions.RemoveRange(old);
            var save = await context.SaveChangesAsync();

            if (save < 0)
                  throw new Exception(MessageHelper.DB.DeleteRelateRecordUnsuccessful);

            entity.Update(domain);

            var data = context.roles.Update(entity);
            save = await context.SaveChangesAsync();

            if (data == null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

            return new RoleDto(
              data.Entity.id,
              data.Entity.name,
              data.Entity.permissions
              .Select(r => new PermissionDto(r.feature_id,
                context.features.AsNoTracking().OrderByDescending(x => x.id).Where(x => x.id == r.feature_id).Select(x => x.name).FirstOrDefault() ?? "",
              r.is_enabled,
              r.is_created,
              r.is_updated,
              r.is_deleted))
              .ToList()
              );
      }

      public async  Task<bool> IsValidRoleIdAsync(int RoleId, CancellationToken ct = default)
      {
            return await context.roles.AsNoTracking().AnyAsync(r => r.id == RoleId, ct);
      }
}
