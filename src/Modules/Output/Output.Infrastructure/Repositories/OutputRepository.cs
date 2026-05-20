using System.Data.Common;
using Device.Contract.DTOs;
using Microsoft.EntityFrameworkCore;
using Output.Application.Interfaces;
using Output.Contract.DTOs;
using Output.Domain.Entities;
using Output.Infrastructure.Persistences;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Output.Infrastructure.Repositories;

public sealed class OutputRepository(OutputDbContext context) : IOutputRepository
{
      public async Task<OutputDto> CreateAsync(Outputs dto,CancellationToken ct =default)
      {
            var data = await context.Outputs.AddAsync(
                  new Persistences.Entities.Outputs(dto)
            );

            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new OutputDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.module_id,
                  data.Entity.output_no,
                  data.Entity.model,
                  data.Entity.location_id,
                  data.Entity.default_pulse

            );
      }

      public async Task<OutputDto> GetByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Outputs.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.id == id)
            .Select(e => new OutputDto(
                  e.id,
                  e.name,
                  e.module_id,
                  e.output_no,
                  e.model,
                  e.location_id,
                  e.default_pulse
            ))
            .FirstOrDefaultAsync(ct) ?? new OutputDto(0,string.Empty,0,0,string.Empty,0,0);
      }

      public async Task<Pagination<OutputDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct = default)
      {
            var query = context.Outputs.AsNoTracking().AsQueryable();

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
            .Select(e => new OutputDto(
                  e.id,
                  e.name,
                  e.module_id,
                  e.output_no,
                  e.model,
                  e.location_id,
                  e.default_pulse
            )).ToListAsync(ct);

            return new Pagination<OutputDto>(param.pageNumber,param.pageSize,count,(int)Math.Ceiling(count / (double)param.pageSize),res);
      }



      public async Task<IEnumerable<short>> GetUnavailableOutputByModuleIdAsync(int moduleId, CancellationToken ct = default)
      {
            return await context.Outputs.AsNoTracking().Where(x => x.module_id == moduleId).Select(x => x.output_no).ToArrayAsync();
      }

      public async Task<bool> IsAnyWithIdAsync(int Id, CancellationToken ct = default)
      {
            return await context.Outputs.AsNoTracking().AnyAsync(x => x.id == Id,ct);
      }
}