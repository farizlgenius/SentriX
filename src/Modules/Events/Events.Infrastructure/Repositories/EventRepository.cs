using System;
using Events.Application.Interfaces;
using Events.Contract.DTOs;
using Events.Infrastructure.Persistences;
using Events.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Events.Infrastructure.Repositories;

public sealed class EventRepository(EventDbContext context) : IEventRepository
{
      public async Task AddCommandEvent(string Name,int LocationId,CommandResponse response,CancellationToken ct = default)
      {
            await context.CommandEvents.AddAsync(
                  new CommandEvent(
                        Name,
                        response.Mac,
                        response.ScpId,
                        response.Command,
                        response.Tag,
                        response.SendAt,
                        response.ReceivedAt,
                        response.Body ?? "",
                        response.Status,
                        response.Reason,
                        string.Empty,
                        DeviceType.AERO.ToString(),
                        LocationId
                        )
            );

            await context.SaveChangesAsync(ct);
      }
      public async Task AddAsync(DateTime timeStamp, string actor, string module, string type, string image, string mac, string name,string code,string remarks, int locationId,CancellationToken ct = default)
      {
            await context.Events.AddAsync(new Event
            {
                  timestamp = timeStamp,
                  actor = actor,
                  module = module,
                  type = type,
                  image = image,
                  mac = mac,
                  name = name,
                  remarks = remarks,
                  code = code,
                  location_id = locationId
            });

            await context.SaveChangesAsync(ct);
      }

      public async Task<Pagination<EventDto>> GetPaginationByLocationIdAsync(PaginationParams param,CancellationToken ct = default)
      {
            var query = context.Events.AsNoTracking().Where(x => x.location_id == param.locationId || x.location_id == 0).AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.actor, pattern) ||
                                  EF.Functions.ILike(x.module, pattern) ||
                                  EF.Functions.ILike(x.type, pattern) ||
                                  EF.Functions.ILike(x.mac, pattern) ||
                                  EF.Functions.ILike(x.name, pattern) ||
                                  EF.Functions.ILike(x.remarks, pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.actor.Contains(search) ||
                                  x.module.Contains(search) ||
                                  x.type.Contains(search) ||
                                  x.mac.Contains(search) ||
                                  x.name.Contains(search) ||
                                  x.remarks.Contains(search)
                              );
                        }
                  }
            }

            if (param.startDate != null)
            {
                  var startUtc = DateTime.SpecifyKind(param.startDate.Value, DateTimeKind.Utc);
                  query = query.Where(x => x.timestamp >= startUtc);
            }

            if (param.endDate != null)
            {
                  var endUtc = DateTime.SpecifyKind(param.endDate.Value, DateTimeKind.Utc);
                  query = query.Where(x => x.timestamp <= endUtc);
            }

            var count = await query.CountAsync();

            var res = await query.AsNoTracking()
            .OrderByDescending(e => e.timestamp)
            .Skip((param.pageNumber - 1) * param.pageSize)
            .Take(param.pageSize)
            .Select(e => new EventDto(
                  e.timestamp,
                  e.actor,
                  e.module,
                  e.type,
                  e.image,
                  e.mac,
                  e.name,
                  e.code,
                  e.remarks,
                  e.location_id
            )).ToListAsync(ct);

            return new Pagination<EventDto>(param.pageNumber,param.pageSize,count,(int)Math.Ceiling(count / (double)param.pageSize),res);
      }

      public async Task UpdateCommandEvent(string Mac, int Tag, short CommandStatus, string Reason,CancellationToken ct = default)
      {
            var entities = await context.CommandEvents
            .OrderByDescending(x => x.id)
            .Where(x => x.tag == Tag && x.mac.Equals(Mac) && x.status.Equals(SharedKernel.Enums.CommandStatus.PENDING.ToString()))
            .ToArrayAsync();

            if(entities.Count() == 0)
                  return;

            foreach(var entity in entities)
            {
                  entity.status = CommandStatus == 1 ? SharedKernel.Enums.CommandStatus.SUCCESSED.ToString() : SharedKernel.Enums.CommandStatus.FAILED.ToString();
                  if(!string.IsNullOrWhiteSpace(Reason) && CommandStatus != 1)
                        entity.reason = Reason;
            }

      

            context.CommandEvents.UpdateRange(entities);

            await context.SaveChangesAsync(ct);


      }

      public async Task<Pagination<CommandEventDto>> GetCommandPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.CommandEvents.AsNoTracking().Where(x => x.location_id == param.locationId || x.location_id == 0).AsQueryable();

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
                                  EF.Functions.ILike(x.mac, pattern) ||
                                  EF.Functions.ILike(x.command, pattern) ||
                                  EF.Functions.ILike(x.body, pattern) ||
                                  EF.Functions.ILike(x.status, pattern) ||
                                  EF.Functions.ILike(x.reason, pattern) ||
                                  EF.Functions.ILike(x.response, pattern) ||
                                  EF.Functions.ILike(x.type, pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.mac.Contains(search) ||
                                  x.command.Contains(search) ||
                                  x.body.Contains(search) ||
                                  x.status.Contains(search) ||
                                  x.reason.Contains(search) ||
                                  x.response.Contains(search) ||
                                  x.type.Contains(search)
                              );
                        }
                  }
            }

            if (param.startDate != null)
            {
                  var startUtc = DateTime.SpecifyKind(param.startDate.Value, DateTimeKind.Utc);
                  query = query.Where(x => x.send_at >= startUtc);
            }

            if (param.endDate != null)
            {
                  var endUtc = DateTime.SpecifyKind(param.endDate.Value, DateTimeKind.Utc);
                  query = query.Where(x => x.received_at <= endUtc);
            }

            var count = await query.CountAsync();

            var res = await query.AsNoTracking()
            .OrderByDescending(e => e.send_at)
            .Skip((param.pageNumber - 1) * param.pageSize)
            .Take(param.pageSize)
            .Select(e => new CommandEventDto(
                  e.id,
                  e.name,
                  e.mac,
                  e.command,
                  e.tag,
                  e.send_at,
                  e.received_at,
                  e.body,
                  e.status,
                  e.reason,
                  e.reason,
                  e.location_id,
                  e.type,
                  e.is_active
            )).ToListAsync(ct);

            return new Pagination<CommandEventDto>(param.pageNumber,param.pageSize,count,(int)Math.Ceiling(count / (double)param.pageSize),res);
      }
}
