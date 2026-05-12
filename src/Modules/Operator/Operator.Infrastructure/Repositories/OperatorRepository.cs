using System;
using Location.Contract.Queries;
using Microsoft.EntityFrameworkCore;
using Operator.Application.Interfaces;
using Operator.Contract.DTOs;
using Operator.Domain.Entities;
using Operator.Infrastructure.Persistences;
using Operator.Infrastructure.Persistences.Entities;
using Role.Contract.Queries;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Operator.Infrastructure.Repositories;

public sealed class OperatorRepository(OperatorDbContext context, IMessageBus bus) : IOperatorRepository
{
      public async Task<OperatorDto> AddAsync(Domain.Entities.Operators domain)
      {
            var data = await context.operators.AddAsync(
            new Persistences.Entities.Operators(domain)
          );

            var save = await context.SaveChangesAsync();

            if (data == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            data.Entity.AddPassword(domain.Password);
            save = await context.SaveChangesAsync();

            if (data == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            foreach (var locationId in domain.LocationId)
            {
                  if (await context.operator_locations.AsNoTracking().AnyAsync(ol => ol.operator_id == data.Entity.id && ol.location_id == locationId))
                        continue;
                  await context.operator_locations.AddAsync(
                        new OperatorLocation
                        {
                              location_id = locationId,
                              operator_id = data.Entity.id
                        }
                  );
            }

            save = await context.SaveChangesAsync();

            if (data == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new OperatorDto(
              data.Entity.id,
              data.Entity.username,
              data.Entity.title,
              data.Entity.firstname,
              data.Entity.middlename,
              data.Entity.lastname,
              data.Entity.gender,
              data.Entity.email,
              data.Entity.mobile,
            data.Entity.role_id,
            data.Entity.operator_locations.Select(ol => ol.location_id).ToList()
              );
      }

      public async Task AddOperatorLocationsAsync(int operatorId, int locationId, CancellationToken ct)
      {
            await context.operator_locations.AddAsync(
                  new OperatorLocation
                  {
                        location_id = locationId,
                        operator_id = 1
                  },
                  ct
            );

            await context.SaveChangesAsync(ct);
      }

      public async Task<OperatorDto> DeleteByIdAsync(int id)
      {
            var entity = await context.operators.OrderByDescending(u => u.id).Where(u => u.id == id).FirstOrDefaultAsync();
            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            var data = context.operators.Remove(entity);
            var save = await context.SaveChangesAsync();

            if (data == null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

            var operatorLocations = await context.operator_locations.Where(ol => ol.operator_id == id).ToArrayAsync();
            if (operatorLocations != null && operatorLocations.Length > 0)
            {
                  context.operator_locations.RemoveRange(operatorLocations);
                  await context.SaveChangesAsync();
            }

            return new OperatorDto(
              data.Entity.id,
              data.Entity.username,
              data.Entity.title,
              data.Entity.firstname,
              data.Entity.middlename,
              data.Entity.lastname,
              data.Entity.gender,
              data.Entity.email,
              data.Entity.mobile,
              data.Entity.role_id,
                  data.Entity.operator_locations.Select(ol => ol.location_id).ToList()
              );
      }

      public async Task<int> GetLocationIdByUsernameAsync(string username)
      {
            return await context.operator_locations
                  .Where(ol => context.operators.Any(o => o.username == username))
                  .Select(ol => ol.location_id)
                  .FirstOrDefaultAsync();
      }

      public async Task<List<int>> GetLocationIdsByUsernameAsync(string username, CancellationToken ct)
      {
            return await context.operator_locations
                  .Where(ol => context.operators.Any(o => o.id == ol.operator_id && o.username == username))
                  .Select(ol => ol.location_id)
                  .ToListAsync(ct);
      }

      public async Task<OperatorDto> GetOperatorByUsernameAsync(string Username)
      {
            return await context.operators.AsNoTracking()
            .Where(x => x.username.Equals(Username))
            .OrderByDescending(x => x.id)
            .Select(x => new OperatorDto(
                  x.id,
                  x.username,
                  x.title,
                  x.firstname,
                  x.middlename,
                  x.lastname,
                  x.gender,
                  x.email,
                  x.mobile,
                  x.role_id,
                  x.operator_locations.Select(ol => ol.location_id).ToList()
            ))
            .FirstOrDefaultAsync()
            ??
            new OperatorDto(0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, new List<int>());
      }

      public async Task<Pagination<OperatorDto>> GetPagination(PaginationParams param,CancellationToken ct = default)
      {

            var operatorIds = await context.operator_locations.AsNoTracking().Where(ol => ol.location_id == param.locationId).Select(ol => ol.operator_id).ToListAsync();
            var query = context.operators.AsNoTracking().Where(x => operatorIds.Contains(x.id)).AsQueryable();

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
                                  EF.Functions.ILike(x.firstname, pattern) ||
                                  EF.Functions.ILike(x.lastname, pattern) ||
                                  EF.Functions.ILike(x.middlename, pattern) ||
                                  EF.Functions.ILike(x.email, pattern) ||
                                  EF.Functions.ILike(x.mobile, pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.username.Contains(search) ||
                                  x.firstname.Contains(search) ||
                                  x.lastname.Contains(search) ||
                                  x.middlename.Contains(search) ||
                                  x.email.Contains(search) ||
                                  x.mobile.Contains(search)
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
            .Select(r => new OperatorDto(r.id, r.username, r.title, r.firstname, r.middlename, r.lastname, r.gender, r.email, r.mobile, r.role_id,r.operator_locations.Select(ol => ol.location_id).ToList()))
            .ToListAsync(ct);

            return new Pagination<OperatorDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }

      public async Task<string> GetPassowrdByUsernameAsync(string Username)
      {
            return await context.operators.AsNoTracking()
            .Where(x => x.username.Equals(Username))
            .OrderByDescending(x => x.id)
            .Select(x => x.password)
            .FirstOrDefaultAsync() ?? string.Empty;
      }

      public async Task<bool> IsAnyByIdAsync(int id)
      {
            return await context.operators.AsNoTracking().AnyAsync(u => u.id == id);
      }

      public async Task<bool> IsAnyUsernameAsync(string Username)
      {
            return await context.operators.AsNoTracking().AnyAsync(u => u.username.Equals(Username));
      }

      public async Task<bool> IsAnyWithLocationIdAsync(int LocationId)
      {
            return await bus.QueryAsync(new IsAnyLocationByIdQuery(LocationId));
      }

      public async Task<bool> IsLocationIdsValidAsync(List<int> LocationIds)
      {
            return await bus.QueryAsync(new IsLocationIdsValidQuery(LocationIds));
      }

      public async Task<bool> IsOperatorExistsByUsernameAsync(string Username)
      {
            return await context.operators.AsNoTracking()
            .AnyAsync(x => x.username.Equals(Username));
      }


      public async Task<bool> IsValidRoleIdAsync(int RoleId)
      {
            return await bus.QueryAsync(new IsValidRoleIdQuery(RoleId));
      }

      public async Task RemoveOperatorLocationByLocationIdAsync(int locationID)
      {
            var entity = await context.operator_locations.Where(x => x.location_id == locationID).ToArrayAsync();

            if (entity is null)
                  return;

            context.operator_locations.RemoveRange(entity);

            await context.SaveChangesAsync();
      }

      public async Task RemoveOperatorLocationsAsync(int locationId, CancellationToken ct)
      {
            var entities = await context.operator_locations.Where(x => x.location_id == locationId).ToArrayAsync(ct);

            if (entities is null || entities.Length == 0)
                  return;

            context.operator_locations.RemoveRange(entities);

            await context.SaveChangesAsync(ct);
      }

      public async Task<OperatorDto> UpdateAsync(Domain.Entities.Operators domain)
      {
            var entity = await context.operators.OrderByDescending(u => u.id).Where(u => u.id == domain.Id).FirstOrDefaultAsync();
            if (entity == null)
                  throw new Exception(MessageHelper.Common.RecordNotFound);

            entity.Update(domain);
            var data = context.operators.Update(entity);
            var save = await context.SaveChangesAsync();

            if (data == null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

            return new OperatorDto(
              data.Entity.id,
              data.Entity.username,
              data.Entity.title,
              data.Entity.firstname,
              data.Entity.middlename,
              data.Entity.lastname,
              data.Entity.gender,
              data.Entity.email,
              data.Entity.mobile,
              data.Entity.role_id,
                  data.Entity.operator_locations.Select(ol => ol.location_id).ToList()
              );
      }

      public async Task<PasswordRuleDto> CreatePasswordRuleAsync(Domain.Entities.PasswordRule dto)
      {
            var entity = await context.password_rules.FirstOrDefaultAsync();
            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(dto);
            var data = context.password_rules.Update(entity);

            var save = await context.SaveChangesAsync();

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new PasswordRuleDto(data.Entity.id,data.Entity.len,data.Entity.is_lower,data.Entity.is_upper,data.Entity.is_symbol,data.Entity.is_digit,data.Entity.weaks.Select(x => x.pattern).ToList());
      }

      public async Task<PasswordRuleDto> GetPassowrdRuleAsync()
      {
            return await context.password_rules.AsNoTracking()
            .Select(x => new PasswordRuleDto(x.id,x.len,x.is_lower,x.is_upper,x.is_symbol,x.is_digit,x.weaks.Select(x => x.pattern).ToList()))
            .FirstOrDefaultAsync() ?? new PasswordRuleDto(0,0,false,false,false,false,new List<string>());
      }


}
