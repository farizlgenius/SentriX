using Door.Application.Interfaces;
using Door.Contract.DTOs;
using Door.Domain.Entities;
using Door.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Door.Infrastructure.Repositories;

public sealed class DoorRepository(DoorDbContext context) : IDoorRepository
{
      public async Task AddAsync(Doors domain, CancellationToken ct = default)
      {
           await context.Doors.AddAsync(
                  new Persistences.Entities.Doors(domain)
            );

            await context.SaveChangesAsync(ct);

      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Doors.OrderByDescending(x => x.id)
                        .Where(x => x.guid == guid)
                        .FirstOrDefaultAsync();
            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            context.Doors.Remove(entity);
            await context.SaveChangesAsync(ct);

      }

      public async Task<IEnumerable<OptionDto>> GetAccessControlFlagAsync(CancellationToken ct = default)
      {
            return await context.AccessControlFlags.AsNoTracking()
           .Select(x => new OptionDto(
            x.label,
            x.value,
            x.description
            )).ToArrayAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetSpareFlagAsync(CancellationToken ct = default)
      {
            return await context.SpareFlags.AsNoTracking()
           .Select(x => new OptionDto(
            x.label,
            x.value,
            x.description
            )).ToArrayAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetApbModeAsync(CancellationToken ct = default)
      {
             return await context.ApbModes.AsNoTracking()
           .Select(x => new OptionDto(
            x.label,
            x.value,
            x.description
            )).ToArrayAsync();
      }

      public async Task<DoorDto> GetByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Doors.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.id == id)
            .Select(x => new DoorDto(
                 x.guid,
                 x.component_id,
                 x.name,
                 x.device_component_id,
                 x.second_component_id,
                 x.mac,
                 x.door_type,
                 x.metadata,
                 x.location_id,
                 x.type,
                 x.is_active
                  )).FirstOrDefaultAsync(ct) ?? 
                  new DoorDto();
      }

      public async Task<IEnumerable<OptionDto>> GetDoorModeAsync(CancellationToken ct = default)
      {
             return await context.DoorModes.AsNoTracking()
           .Select(x => new OptionDto(
            x.label,
            x.value,
            x.description
            )).ToArrayAsync();
      }

      public async Task<Pagination<DoorDto>> GetDoorPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Doors.AsNoTracking().Where(x => x.location_id == param.locationId).AsQueryable();

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
                                  EF.Functions.ILike(x.mac,pattern) ||
                                  EF.Functions.ILike(x.door_type,pattern) ||
                                  EF.Functions.ILike(x.type,pattern)  
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) || 
                                  x.mac.Contains(search) ||
                                  x.door_type.Contains(search) || 
                                  x.type.Contains(search) 
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

            var count = await query.CountAsync();

            var res = await query.AsNoTracking()
            .OrderByDescending(e => e.created_at)
            .Skip((param.pageNumber - 1) * param.pageSize)
            .Take(param.pageSize)
            .Select(e => new DoorDto(
                  e.guid,
                  e.component_id,
                  e.name,
                  e.device_component_id,
                  e.second_component_id,
                  e.mac,
                  e.door_type,
                  e.metadata,
                  e.location_id,
                  e.type,
                  e.is_active
            )).ToListAsync(ct);

            return new Pagination<DoorDto>(param.pageNumber,param.pageSize,count,(int)Math.Ceiling(count / (double)param.pageSize),res);
      }

      public async Task<short> GetLowestDoorComponentIdAsync(string Mac, CancellationToken ct = default)
      {
            return (short)await ComponentHelper.LowestUnassignedNumberAsync<Persistences.Entities.Doors>(
                  context,
                  x => x.mac.Equals(Mac),
                  x => x.component_id,
                  10,
                  ct
                  );
      }

      public async Task<short> GetLowestDoorComponentIdWithExceptionAsync(string Mac, List<int> Excepts, CancellationToken ct = default)
      {
            return await ComponentHelper.LowestUnassignedNumberAsync<Persistences.Entities.Doors>(
                  context,
                  Excepts,
                  x => x.mac.Equals(Mac),
                  x => new
                  {
                        x.component_id,
                        x.second_component_id
                  },
                  10,
                  ct
                  );
      }

      public async Task<IEnumerable<OptionDto>> GetReaderModeAsync(CancellationToken ct = default)
      {
           return await context.ReaderModes.AsNoTracking()
           .Select(x => new OptionDto(
            x.label,
            x.value,
            x.description
            )).ToArrayAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetStrikeModeAsync(CancellationToken ct = default)
      {
            return await context.StrikeModes.AsNoTracking()
           .Select(x => new OptionDto(
            x.label,
            x.value,
            x.description
            )).ToArrayAsync();
      }

      public async Task<bool> IsAnyByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Doors.AsNoTracking().AnyAsync(x => x.id == id);
      }

      public async Task UpdateAsync(Doors domain, CancellationToken ct = default)
      {
            var entity = await context.Doors.Where(x => x.guid == domain.Guid).FirstOrDefaultAsync();
            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(domain);

            context.Doors.Update(entity);

           await context.SaveChangesAsync(ct);



      }

      public async Task<IEnumerable<OptionDto>> GetOsdpBaudrateAsync(CancellationToken ct = default)
      {
             return await context.OsdpBaudrates.AsNoTracking()
           .Select(x => new OptionDto(
            x.label,
            x.value,
            x.description
            )).ToArrayAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetDoorOptionByLocationIdAsync(int LocationId, CancellationToken ct = default)
      {
            return await context.Doors.AsNoTracking()
            .Where(x => x.location_id == LocationId)
            .Select(x => new OptionDto(
                  x.name,
                  x.component_id,
                  x.mac+","+x.type,
                  Guid.Empty
            )).ToListAsync();
      }

      public async Task<string> GetNameByMacAndComponentIdAsync(string Mac, short ComponentId, CancellationToken ct = default)
      {
            return await context.Doors.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.mac.Equals(Mac) && (x.component_id == ComponentId || x.second_component_id == ComponentId))
            .Select(x => x.name)
            .FirstOrDefaultAsync() ?? string.Empty;
            
      }

      public async Task<IEnumerable<DoorDto>> GetDoorByMacAsync(string Mac, CancellationToken ct = default)
      {
            return await context.Doors.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.mac.Equals(Mac))
            .Select(x => new DoorDto(
                  x.guid,
                  x.component_id,
                  x.name,
                  x.device_component_id,
                  x.second_component_id,
                  x.mac,
                  x.door_type,
                  x.metadata,
                  x.location_id,
                  x.type,
                  x.is_active
            )).ToArrayAsync();
      }

      public async Task<bool> IsAnyDoorNotSyncAsync(string Mac,int LocationId,DateTime SyncAt, CancellationToken ct = default)
      {
            return await context.Doors.AsNoTracking().AnyAsync(x => x.location_id == LocationId && x.mac.Equals(Mac) && x.updated_at > SyncAt);
      }

      public async Task<DoorDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Doors.AsNoTracking()
            .Where(x => x.guid == guid)
            .Select(x => new DoorDto(
                  x.guid,
                  x.component_id,
                  x.name,
                  x.device_component_id,
                  x.second_component_id,
                  x.mac,
                  x.door_type,
                  x.metadata,
                  x.location_id,
                  x.type,
                  x.is_active,
                  x.is_default
            )).FirstOrDefaultAsync() ?? new DoorDto();
      }

      public async Task<bool> IsAnyByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Doors.AsNoTracking().AnyAsync(x => x.guid == guid);
      }
}