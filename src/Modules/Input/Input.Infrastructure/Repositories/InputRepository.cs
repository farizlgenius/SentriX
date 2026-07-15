using Input.Application.Interfaces;
using Input.Contract.DTOs;
using Input.Infrastructure.Persistences;
using Input.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Input.Infrastructure.Repositories;

public sealed class InputRepository(InputDbContext context) : IInputRepository
{
      public async Task<InputDto> CreateInputAsync(Domain.Entities.Inputs domain,CancellationToken ct= default)
      {
            var data = await context.Inputs.AddAsync(
                  new Persistences.Entities.Inputs(domain)
            );
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new InputDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,
                  data.Entity.mac,
                  data.Entity.device_component_id,
                  data.Entity.module_component_id,
                  data.Entity.input_no,
                  data.Entity.sensor_mode,
                  data.Entity.debounce,
                  data.Entity.hold_time,
                  data.Entity.log_function,
                  data.Entity.latch_mode,
                  data.Entity.delay_entry,
                  data.Entity.delay_exit,
                  data.Entity.type,
                  data.Entity.location_id,
                  data.Entity.is_active
                  );
      }

      public async Task<InputGroupDto> CreateInputGroupAsync(Domain.Entities.InputGroups domain, CancellationToken ct = default)
      {
            var data = await context.InputGroups.AddAsync(
                  new Persistences.Entities.InputGroups(domain)
            );

             var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return await context.InputGroups.AsNoTracking().OrderByDescending(x => x.id).Where(x => x.id == data.Entity.id)
            .Select(x => new InputGroupDto(
                  x.id,
                  x.name,
                  x.input_group_detail
                  .SelectMany(d => d.input_list
                        .Select(l => new InputGroupDetailDto(
                              d.mac,
                              d.device_component_id,
                              l.input_type,
                              l.input_component_id
                        )).ToList()
                        ).ToList(),
                  x.component_id,
                  x.location_id,
                  x.type,
                  x.is_active
            ))
            .FirstOrDefaultAsync() ?? new InputGroupDto();
      }

      public async Task<InputDto> DeleteInputAsync(int id, CancellationToken ct = default)
      {
            var entity = await context.Inputs
                              .OrderByDescending(x => x.id)
                              .Where(x => x.id == id)
                              .FirstOrDefaultAsync();

            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            var data = context.Inputs.Remove(entity);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return new InputDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,
                  data.Entity.mac,
                  data.Entity.device_component_id,
                  data.Entity.module_component_id,
                  data.Entity.input_no,
                  data.Entity.sensor_mode,
                  data.Entity.debounce,
                  data.Entity.hold_time,
                  data.Entity.log_function,
                  data.Entity.latch_mode,
                  data.Entity.delay_entry,
                  data.Entity.delay_exit,
                  data.Entity.type,
                  data.Entity.location_id,
                  data.Entity.is_active
                  );

      }

      public async Task<InputGroupDto> DeleteInputGroupAsync(int id, CancellationToken ct = default)
      {
            var entity = await context.InputGroups
                              .OrderByDescending(x => x.id)
                              .Where(x => x.id == id)
                              .Include(x => x.input_group_detail)
                              .ThenInclude(x => x.input_list)
                              .FirstOrDefaultAsync();

            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            var data = context.InputGroups.Remove(entity);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return new InputGroupDto(
                  entity.id,
                  entity.name,
                  entity.input_group_detail
                  .SelectMany(d => d.input_list
                        .Select(l => new InputGroupDetailDto(
                              d.mac,
                              d.device_component_id,
                              l.input_type,
                              l.input_component_id
                        )).ToList()
                        ).ToList(),
                  entity.component_id,
                  entity.location_id,
                  entity.type,
                  entity.is_active
            );


      }

      public async Task<InputDto> GetByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Inputs.AsNoTracking().OrderByDescending(x => x.id).Where(x => x.id == id)
            .Select(x => new InputDto(
                  x.id,
                  x.component_id,
                  x.name,
                  x.mac,
                  x.device_component_id,
                  x.module_component_id,
                  x.input_no,
                  x.sensor_mode,
                  x.debounce,
                  x.hold_time,
                  x.log_function,
                  x.latch_mode,
                  x.delay_entry,
                  x.delay_exit,
                  x.type,
                  x.location_id,
                  x.is_active
            ))
            .FirstOrDefaultAsync() ?? 
            new InputDto()
            ;
      }

      public async Task<InputGroupDto> GetGroupByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.InputGroups.AsNoTracking()
            .Include(x => x.input_group_detail)
            .ThenInclude(x => x.input_list)
            .Where(x => x.id == id)
            .Select(x => new InputGroupDto(
                  x.id,
                  x.name,
                  x.input_group_detail
                  .SelectMany(d => d.input_list
                        .Select(l => new InputGroupDetailDto(
                              d.mac,
                              d.device_component_id,
                              l.input_type,
                              l.input_component_id
                        )).ToList()
                        ).ToList(),
                  x.component_id,
                  x.location_id,
                  x.type,
                  x.is_active
            ))
            .FirstOrDefaultAsync(ct) ?? 
            new InputGroupDto();
      }

      public async Task<IEnumerable<InputDto>> GetInputByMacAsync(string Mac, CancellationToken ct = default)
      {
            return await context.Inputs.AsNoTracking()
            .Where(x => x.mac.Equals(Mac))
            .Select(x => new InputDto(
                  x.id,
                  x.component_id,
                  x.name,
                  x.mac,
                  x.device_component_id,
                  x.module_component_id,
                  x.input_no,
                  x.sensor_mode,
                  x.debounce,
                  x.hold_time,
                  x.log_function,
                  x.latch_mode,
                  x.delay_entry,
                  x.delay_exit,
                  x.type,
                  x.location_id,
                  x.is_active
            ))
            .ToArrayAsync();
      }

      public async Task<IEnumerable<InputGroupDto>> GetInputGroupByMacAsync(string Mac, CancellationToken ct = default)
      {
            return await context.InputGroups.AsNoTracking()
            .Include(x => x.input_group_detail)
            .ThenInclude(x => x.input_list)
            .Where(x => x.input_group_detail.Any(g => g.mac.Equals(Mac)))
            .Select(x => new InputGroupDto(
                  x.id,
                  x.name,
                  x.input_group_detail
                  .SelectMany(d => d.input_list
                        .Select(l => new InputGroupDetailDto(
                              d.mac,
                              d.device_component_id,
                              l.input_type,
                              l.input_component_id
                        )).ToList()
                        ).ToList(),
                  x.component_id,
                  x.location_id,
                  x.type,
                  x.is_active
            ))
            .ToArrayAsync();
      }

      public async Task<Pagination<InputGroupDto>> GetInputGroupPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.InputGroups
            .AsNoTracking()
            .Include(x => x.input_group_detail)
            .ThenInclude(x => x.input_list)
            .AsQueryable();

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
                                  EF.Functions.ILike(x.type,pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) || 
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
            .Select(x => new InputGroupDto(
                  x.id,
                  x.name,
                  x.input_group_detail
                  .SelectMany(d => d.input_list
                        .Select(l => new InputGroupDetailDto(
                              d.mac,
                              d.device_component_id,
                              l.input_type,
                              l.input_component_id
                        )).ToList()
                        ).ToList(),
                  x.component_id,
                  x.location_id,
                  x.type,
                  x.is_active
            )).ToListAsync(ct);

            return new Pagination<InputGroupDto>(param.pageNumber, param.pageSize, count, (int)Math.Ceiling(count / (double)param.pageSize), res);
      }

      public async Task<IEnumerable<OptionDto>> GetInputModeAsync(CancellationToken ct = default)
      {
            return await context.InputModes.AsNoTracking()
            .Select(x => new OptionDto(
                  x.label,
                  x.value,
                  x.description,
                  Guid.Empty,
                  false
            )).ToArrayAsync();

      }

      public async Task<Pagination<InputDto>> GetInputPaginationAsync(PaginationParams param,CancellationToken ct = default)
      {
            var query = context.Inputs.AsNoTracking().Where(x => x.location_id == param.locationId).AsQueryable();

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
                                  EF.Functions.ILike(x.mac, pattern ) ||
                                  EF.Functions.ILike(x.type,pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) || 
                                  x.mac.Contains(search) ||
                                  x.type.Contains(search)
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
            .Select(e => new InputDto(
                  e.id,
                  e.component_id,
                  e.name,
                  e.mac,
                  e.device_component_id,
                  e.module_component_id,
                  e.input_no,
                  e.sensor_mode,
                  e.debounce,
                  e.hold_time,
                  e.log_function,
                  e.latch_mode,
                  e.delay_entry,
                  e.delay_exit,
                  e.type,
                  e.location_id,
                  e.is_active
            )).ToListAsync(ct);

            return new Pagination<InputDto>(param.pageNumber, param.pageSize, count, (int)Math.Ceiling(count / (double)param.pageSize), res);
      }

      public async Task<short> GetLowestInputComponentIdAsync(string Mac,CancellationToken ct = default)
      {
            return (short)await ComponentHelper.LowestUnassignedNumberAsync<Inputs>(
                  context,
                  x => x.mac.Equals(Mac),
                  x => x.component_id,
                  100
                  );
      }

      public async Task<short> GetLowestInputGroupComponentIdAsync(CancellationToken ct = default)
      {
            return (short)await ComponentHelper.LowestUnassignedNumberAsync<InputGroups>(
                  context,
                  x => x.component_id,
                  100
                  );
      }

      public async Task<IEnumerable<short>> GetUnavailableInputByModuleIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Inputs.AsNoTracking().Where(x => x.module_component_id == id).Select(x => x.input_no).ToArrayAsync();
      }

      public async Task<bool> IsAnyInputGroupNotSyncAsync(string Mac, int LocationId, DateTime SyncAt, CancellationToken ct = default)
      {
            return await context.InputGroups.AsNoTracking().AnyAsync(x => x.input_group_detail.Any(x => x.mac.Equals(Mac)) && x.location_id == LocationId && x.updated_at > SyncAt);
      }

      public async Task<bool> IsAnyInputNotSyncAsync(string Mac, int LocationId, DateTime SyncAt, CancellationToken ct = default)
      {
            return await context.Inputs.AsNoTracking().AnyAsync(x => x.mac.Equals(Mac) && x.location_id == LocationId && x.updated_at > SyncAt);
      }

      public async Task<InputDto> UpdateInputAsync(Domain.Entities.Inputs domain, CancellationToken ct = default)
      {
            var entity = await context.Inputs
                              .OrderByDescending(x => x.id)
                              .Where(x => x.id == domain.Id)
                              .FirstOrDefaultAsync();

            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(domain);

            var data = context.Inputs.Update(entity);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return new InputDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,
                  data.Entity.mac,
                  data.Entity.device_component_id,
                  data.Entity.module_component_id,
                  data.Entity.input_no,
                  data.Entity.sensor_mode,
                  data.Entity.debounce,
                  data.Entity.hold_time,
                  data.Entity.log_function,
                  data.Entity.latch_mode,
                  data.Entity.delay_entry,
                  data.Entity.delay_exit,
                  data.Entity.type,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }

      public async Task<InputGroupDto> UpdateInputGroupAsync(Domain.Entities.InputGroups domain, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}