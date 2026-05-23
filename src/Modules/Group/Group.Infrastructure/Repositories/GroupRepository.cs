using Group.Application.Interfaces;
using Group.Contract.DTOs;
using Group.Domain.Entities;
using Group.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Helpers;

namespace Group.Infrastructure.Repositories;

public sealed class GroupRepository(GroupDbContext context) : IGroupRepository
{
      public async Task<GroupDto> CreateAsync(Groups domain, CancellationToken ct = default)
      {
            var data = await context.Groups.AddAsync(
                  new Persistences.Entities.Groups(domain)
            );

            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new GroupDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,
                  data.Entity.metadata,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }

      public async Task<GroupDto> DeleteAsync(int id, CancellationToken ct = default)
      {
            var entity = await context.Groups.OrderByDescending(x => x.id == id)
            .Where(x => x.id == id)
            .FirstOrDefaultAsync();

            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            var data = context.Groups.Remove(entity);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return new GroupDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,
                  data.Entity.metadata,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }

      public async Task<GroupDto> GetByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Groups.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Select(x => new GroupDto(
                  x.id,
                  x.component_id,
                  x.name,
                  x.metadata,
                  x.location_id,
                  x.is_active
            ))
            .FirstOrDefaultAsync() ?? new GroupDto(
                  0,
                  0,
                  string.Empty,
                  string.Empty,
                  0,
                  false
                  );
      }

      public async Task<short> GetLowestGroupComponentIdAsync(CancellationToken ct = default)
      {
            return (short)await ComponentHelper.LowestUnassignedNumberAsync<Persistences.Entities.Groups>(
                  context,
                  x => x.component_id,
                  100,
                  ct
            );
      }

      public async Task<bool> IsAnyByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Groups.AsNoTracking().AnyAsync(x => x.id == id);
      }

      public async Task<GroupDto> UpdateAsync(Groups dto, CancellationToken ct = default)
      {
            var entity = await context.Groups.OrderByDescending(x => x.id == dto.Id)
            .Where(x => x.id == dto.Id)
            .FirstOrDefaultAsync();

            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(dto);

            var data = context.Groups.Update(entity);
            var save = await context.SaveChangesAsync(ct);

             if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

            return new GroupDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,
                  data.Entity.metadata,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }
}