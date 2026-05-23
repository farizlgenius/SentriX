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
                  data.Entity.metadata,
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

            return new InputGroupDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.mac,
                  data.Entity.device_component_id,
                  data.Entity.metadata,
                  data.Entity.component_id,
                   data.Entity.location_id,
                  data.Entity.type,
                  data.Entity.is_active
                  );
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
                  data.Entity.metadata,
                  data.Entity.type,
                  data.Entity.location_id,
                  data.Entity.is_active
                  );

      }

      public async Task<InputGroupDto> DeleteInputGroupAsync(int id, CancellationToken ct = default)
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

            return new InputGroupDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.mac,
                  data.Entity.device_component_id,
                  data.Entity.metadata,
                  data.Entity.component_id,
                   data.Entity.location_id,
                  data.Entity.type,
                  data.Entity.is_active
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
                  x.metadata,
                  x.type,
                  x.location_id,
                  x.is_active
            ))
            .FirstOrDefaultAsync() ?? 
            new InputDto(
                  0,
                  0,
                  string.Empty,
                  string.Empty,
                  0,
                  0,
                  0,
                  string.Empty,
                  string.Empty,
                  0,
                  false
                  )
            ;
      }

      public async Task<InputGroupDto> GetGroupByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.InputGroups.AsNoTracking().Where(x => x.id == id)
            .Select(x => new InputGroupDto(
                  x.id,
                  x.name,
                  x.mac,
                  x.device_component_id,
                  x.metadata,
                  x.component_id,
                  x.location_id,
                  x.type,
                  x.is_active
                  ))
            .FirstOrDefaultAsync() ?? 
            new InputGroupDto(
                  0,
                  string.Empty,
                  string.Empty,
                  0,
                  string.Empty,
                  0,
                  0,
                  string.Empty,
                  false
                  );
      }

      public async Task<Pagination<InputGroupDto>> GetInputGroupPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.InputGroups.AsNoTracking().AsQueryable();

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
                  x.mac,
                  x.device_component_id,
                  x.metadata,
                  x.component_id,
                  x.location_id,
                  x.type,
                  x.is_active
            )).ToListAsync(ct);

            return new Pagination<InputGroupDto>(param.pageNumber, param.pageSize, count, (int)Math.Ceiling(count / (double)param.pageSize), res);
      }

      public async Task<Pagination<InputDto>> GetInputPaginationAsync(PaginationParams param,CancellationToken ct = default)
      {
            var query = context.Inputs.AsNoTracking().AsQueryable();

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
            .Select(e => new InputDto(
                  e.id,
                  e.component_id,
                  e.name,
                  e.mac,
                  e.device_component_id,
                  e.module_component_id,
                  e.input_no,
                  e.metadata,
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

      public async Task<short> GetLowestInputGroupComponentIdAsync(string Mac, CancellationToken ct = default)
      {
            return (short)await ComponentHelper.LowestUnassignedNumberAsync<InputGroups>(
                  context,
                  x => x.mac.Equals(Mac),
                  x => x.component_id,
                  100
                  );
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
                  data.Entity.metadata,
                  data.Entity.type,
                  data.Entity.location_id,
                  data.Entity.is_active
                  );
      }

      public Task<InputGroupDto> UpdateInputGroupAsync(Domain.Entities.InputGroups domain, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}