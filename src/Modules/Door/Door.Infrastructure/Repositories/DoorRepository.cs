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
      public async Task<DoorDto> CreateAsync(Doors domain, CancellationToken ct = default)
      {
            var data = await context.Doors.AddAsync(
                  new Persistences.Entities.Doors(domain)
            );

            var save = await context.SaveChangesAsync(ct);

            if (data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new DoorDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,
                  data.Entity.device_component_id,
                  data.Entity.second_component_id,
                  data.Entity.mac,
                  data.Entity.door_type,
                  data.Entity.metadata,
                  data.Entity.location_id,
                  data.Entity.type,
                  data.Entity.is_active
            );
      }

      public async Task<DoorDto> DeleteAsync(int id, CancellationToken ct = default)
      {
            var entity = await context.Doors.OrderByDescending(x => x.id)
                        .Where(x => x.id == id)
                        .FirstOrDefaultAsync();
            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            var data = context.Doors.Remove(entity);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return new DoorDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,
                  data.Entity.device_component_id,
                  data.Entity.second_component_id,
                  data.Entity.mac,
                  data.Entity.door_type,
                  data.Entity.metadata,
                  data.Entity.location_id,
                  data.Entity.type,
                  data.Entity.is_active
            );
      }

      public async Task<DoorDto> GetByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Doors.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.id == id)
            .Select(x => new DoorDto(
                 x.id,
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
                  new DoorDto(
                        0,
                        0,
                        string.Empty,
                        0,
                        0,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        0,
                        string.Empty,
                        false
                  )
                  ;
      }

      public async Task<Pagination<DoorDto>> GetDoorPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Doors.AsNoTracking().AsQueryable();

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
                  e.id,
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
            return (short)await ComponentHelper.LowestUnassignedNumberAsync<Persistences.Entities.Doors>(
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

      public async Task<bool> IsAnyByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Doors.AsNoTracking().AnyAsync(x => x.id == id);
      }

      public async Task<DoorDto> UpdateAsync(Doors domain, CancellationToken ct = default)
      {
            var entity = await context.Doors.Where(x => x.id == domain.Id).FirstOrDefaultAsync();
            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(domain);

            var data = context.Doors.Update(entity);

            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return new DoorDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,
                  data.Entity.device_component_id,
                  data.Entity.second_component_id,
                  data.Entity.mac,
                  data.Entity.door_type,
                  data.Entity.metadata,
                  data.Entity.location_id,
                  data.Entity.type,
                  data.Entity.is_active
            );


      }
}