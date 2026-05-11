using System;
using Microsoft.EntityFrameworkCore;
using Operator.Application.Interfaces;
using Operator.Contract.DTOs;
using Operator.Infrastructure.Persistences;
using Operator.Infrastructure.Persistences.Entities;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Operator.Infrastructure.Repositories;

public sealed class OperatorRepository(OperatorDbContext context) : IOperatorRepository
{
      public Task<OperatorDto> AddAsync(Domain.Entities.Operators domain)
      {
            throw new NotImplementedException();
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

      public Task<OperatorDto> DeleteByIdAsync(int id)
      {
            throw new NotImplementedException();
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
                  x.role_id
            ))
            .FirstOrDefaultAsync()
            ??
            new OperatorDto(0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0);
      }

      public Task<Pagination<OperatorDto>> GetPaginationWithLocationIdAsync(int LocationId, int Page, int PageSize, string Search)
      {
            throw new NotImplementedException();
      }

      public async Task<string> GetPassowrdByUsernameAsync(string Username)
      {
            return await context.operators.AsNoTracking()
            .Where(x => x.username.Equals(Username))
            .OrderByDescending(x => x.id)
            .Select(x => x.password)
            .FirstOrDefaultAsync() ?? string.Empty;
      }

      public Task<bool> IsAnyByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyUsernameAsync(string Username)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyWithLocationIdAsync(int LocationId)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsExceptLocationIdsAsync(List<int> LocationIds)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsOperatorExistsByUsernameAsync(string Username)
      {
            return await context.operators.AsNoTracking()
            .AnyAsync(x => x.username.Equals(Username));
      }

      public Task<bool> IsValidCompanyAsync(int CompanyId)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsValidDepartmentAsync(int DepartmentId)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsValidPositionAsync(int PositionId)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsValidRoleIdAsync(int RoleId)
      {
            throw new NotImplementedException();
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

            if(entities is null || entities.Length == 0)
                  return;

            context.operator_locations.RemoveRange(entities);

            await context.SaveChangesAsync(ct);
      }

      public Task<OperatorDto> UpdateAsync(Domain.Entities.Operators domain)
      {
            throw new NotImplementedException();
      }

      // public async Task<OperatorDto> AddAsync(Operators domain)
      // {
      //       var data = await context.operators.AddAsync(
      //        new Persistences.Entities.Operators(domain)
      //      );

      //       var save = await context.SaveChangesAsync();

      //       if (data == null || save <= 0)
      //             throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

      //       data.Entity.AddPassword(domain.password);
      //       data.Entity.AddLocation(data.Entity.id, domain.LocationId);
      //       save = await context.SaveChangesAsync();

      //       if (data == null || save <= 0)
      //             throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

      //       return new OperatorDto(
      //         data.Entity.id,
      //         data.Entity.operator_id,
      //         data.Entity.username,
      //         data.Entity.title,
      //         data.Entity.firstname,
      //         data.Entity.middlename,
      //         data.Entity.lastname,
      //         data.Entity.gender,
      //         data.Entity.email,
      //         data.Entity.mobile,
      //         await context.Roles.AsNoTracking().OrderByDescending(x => x.id).Where(c => c.users.Select(x => x.id).Contains(data.Entity.id)).Select(c => c.name).FirstOrDefaultAsync() ?? ""
      //         );
      // }

      // public async Task<OperatorDto> DeleteByIdAsync(int id)
      // {
      //       var entity = await context.operators.OrderByDescending(u => u.id).Where(u => u.id == id).FirstOrDefaultAsync();
      //       if (entity == null)
      //             throw new Exception(MessageHelper.DB.RecordNotFound);

      //       var data = context.Operators.Remove(entity);
      //       var save = await context.SaveChangesAsync();

      //       if (data == null || save <= 0)
      //             throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

      //       return new OperatorDto(
      //         data.Entity.id,
      //         data.Entity.operator_id,
      //         data.Entity.username,
      //         data.Entity.title,
      //         data.Entity.firstname,
      //         data.Entity.middlename,
      //         data.Entity.lastname,
      //         data.Entity.gender,
      //         data.Entity.email,
      //         data.Entity.mobile,
      //         await context.Roles.AsNoTracking().OrderByDescending(x => x.id).Where(c => c.users.Select(x => x.id).Contains(data.Entity.id)).Select(c => c.name).FirstOrDefaultAsync() ?? ""
      //         );
      // }

      // public async Task<Pagination<OperatorDto>> GetPaginationWithLocationIdAsync(int LocationId, int Page, int PageSize,string Search)
      // {
      //       var query = context.operators.AsNoTracking().AsQueryable();
      //       var totalItems = await query.CountAsync();
      //       var items = await query.OrderByDescending(u => u.id)
      //       .Where(u => u.operator_locations.Select(ul => ul.location_id).Contains(LocationId))
      //       .Skip((Page - 1) * PageSize)
      //       .Take(PageSize)
      //       .Select(u =>
      //       new OperatorDto(
      //         u.id,
      //         u.username,
      //         u.title,
      //         u.firstname,
      //         u.middlename,
      //         u.lastname,
      //         u.gender,
      //         u.email,
      //         u.mobile,
      //          u.role.name
      //         ))
      //       .ToListAsync();

      //       return new Pagination<OperatorDto>(Page, PageSize, totalItems, (int)Math.Ceiling(totalItems / (double)PageSize), items);
      // }

      // public async Task<bool> IsAnyByIdAsync(int id)
      // {
      //       return await context.operators.AsNoTracking().AnyAsync(u => u.id == id);
      // }

      // public async Task<bool> IsAnyUsernameAsync(string Username)
      // {
      //       return await context.operators.AsNoTracking().AnyAsync(u => u.username.Equals(Username));
      // }

      // public async Task<bool> IsAnyWithLocationIdAsync(int LocationId)
      // {
      //       return await context.Locations.AsNoTracking().AnyAsync(l => l.id == LocationId);
      // }

      // public async Task<bool> IsValidCompanyAsync(int CompanyId)
      // {
      //       return await context.Companies.AsNoTracking().AnyAsync(c => c.id == CompanyId);
      // }

      // public async Task<bool> IsValidDepartmentAsync(int DepartmentId)
      // {
      //       return await context.Departments.AsNoTracking().AnyAsync(d => d.id == DepartmentId);
      // }

      // public async Task<bool> IsExceptLocationIdsAsync(List<int> LocationIds)
      // {
      //       var existingIds = await context.Locations
      //           .Where(x => LocationIds.Contains(x.id))
      //           .Select(x => x.id)
      //           .ToListAsync();

      //       return LocationIds.Except(existingIds).Any();
      // }

      // public async Task<bool> IsValidPositionAsync(int PositionId)
      // {
      //       return await context.Positions.AsNoTracking().AnyAsync(p => p.id == PositionId);
      // }

      // public async Task<OperatorDto> UpdateAsync(Operators domain)
      // {
      //       var entity = await context.operators.OrderByDescending(u => u.id).Where(u => u.id == domain.Id).FirstOrDefaultAsync();
      //       if (entity == null)
      //             throw new Exception(MessageHelper.Common.RecordNotFound);

      //       entity.Update(domain);
      //       var data = context.operators.Update(entity);
      //       var save = await context.SaveChangesAsync();

      //       if (data == null || save <= 0)
      //             throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

      //       return new OperatorDto(
      //         data.Entity.id,
      //         data.Entity.username,
      //         data.Entity.title,
      //         data.Entity.firstname,
      //         data.Entity.middlename,
      //         data.Entity.lastname,
      //         data.Entity.gender,
      //         data.Entity.email,
      //         data.Entity.mobile,
      //         await context.Roles.AsNoTracking().OrderByDescending(x => x.id).Where(c => c.users.Select(x => x.id).Contains(data.Entity.id)).Select(c => c.name).FirstOrDefaultAsync() ?? ""
      //         );
      // }

      // public async Task<bool> IsValidRoleIdAsync(int RoleId)
      // {
      //       return await context.Roles.AsNoTracking().AnyAsync(x => x.id == RoleId);
      // }
}
