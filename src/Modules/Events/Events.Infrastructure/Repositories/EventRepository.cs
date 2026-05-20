using System;
using Events.Application.Interfaces;
using Events.Contract.DTOs;
using Events.Infrastructure.Persistences;
using Events.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Model;

namespace Events.Infrastructure.Repositories;

public sealed class EventRepository(EventDbContext context) : IEventRepository
{
      public async Task AddCommandEvent(CommandResponse response)
      {
            await context.CommandEvents.AddAsync(
                  new CommandEvent(
                        response.Mac,
                        response.ScpId,
                        response.Command,
                        response.Tag,
                        response.SendAt,
                        response.ReceivedAt,
                        response.Body,
                        response.Status,
                        response.Reason
                        )
            );

            await context.SaveChangesAsync();
      }
      public async Task AddAsync(DateTime timeStamp, string actor, string module, string type, string image, string mac, string name, string remarks, int locationId)
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
                  location_id = locationId
            });

            await context.SaveChangesAsync();
      }

      public async Task<Pagination<EventDto>> GetPaginationByLocationIdAsync(PaginationParams param)
      {
            var query = context.Events.AsNoTracking().AsQueryable();

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

            if (param.locationId >= 0)
            {
                  query = query.Where(x => x.location_id == param.locationId || x.location_id == 1);
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
                  e.remarks,
                  e.location_id
            )).ToListAsync();

            return new Pagination<EventDto>(param.pageNumber,param.pageSize,count,(int)Math.Ceiling(count / (double)param.pageSize),res);
      }
}
