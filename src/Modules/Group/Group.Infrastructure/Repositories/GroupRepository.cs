using Door.Contract.DTOs;
using Group.Application.Interfaces;
using Group.Contract.DTOs;
using Group.Domain.Entities;
using Group.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
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
                  new List<GroupDoorDto>(),
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
                  new List<GroupDoorDto>(),
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
                  new List<GroupDoorDto>(),
                  x.location_id,
                  x.is_active
            ))
            .FirstOrDefaultAsync() ?? new GroupDto(
                  0,
                  0,
                  string.Empty,
                  new List<GroupDoorDto>(),
                  0,
                  false
                  );
      }

      public async Task<IEnumerable<GroupDto>> GetByLocationIdAsync(int location, CancellationToken ct = default)
      {
            return await context.Groups.AsNoTracking()
            .Where(x => x.location_id == location)
            .Select(x => new GroupDto(
                  x.id,
                  x.component_id,
                  x.name,
                  new List<GroupDoorDto>(),
                  x.location_id,
                  x.is_active
                  )).ToArrayAsync();
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

      public async Task<Pagination<GroupDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Groups.AsNoTracking()
            .Include(x => x.group_doors)
            .ThenInclude(s => s.group_door_detail)
            .Where(x => x.location_id == param.locationId || x.location_id == 0).AsQueryable();

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

            var res = await query.AsNoTracking()
            .OrderByDescending(e => e.created_at)
            .Skip((param.pageNumber - 1) * param.pageSize)
            .Take(param.pageSize)
            .Select(e => new GroupDto(
                  e.id,
                  e.component_id,
                  e.name,
                  new List<GroupDoorDto>(),
                  e.location_id,
                  e.is_active
            )).ToListAsync(ct);

            return new Pagination<GroupDto>(param.pageNumber,param.pageSize,count,(int)Math.Ceiling(count / (double)param.pageSize),res);
      }

      public async Task<bool> IsAnyByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Groups.AsNoTracking().AnyAsync(x => x.id == id);
      }

      public async Task<GroupDto> UpdateAsync(Groups dto, CancellationToken ct = default)
      {
            var entity = await context.Groups
            .Include(x => x.group_doors)
            .ThenInclude(x => x.group_door_detail)
            .OrderByDescending(x => x.id == dto.Id)
            .Where(x => x.id == dto.Id)
            .FirstOrDefaultAsync();

            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(dto);

            context.GroupDoors.RemoveRange(entity.group_doors);

            entity.group_doors = dto.GroupDoors.Select(x => new Persistences.Entities.GroupDoor(x)).ToList();

            var data = context.Groups.Update(entity);
            var save = await context.SaveChangesAsync(ct);

             if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

            return new GroupDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,
                  new List<GroupDoorDto>(),
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }
}