using Microsoft.EntityFrameworkCore;
using Setting.Application.Interfaces;
using Setting.Contract.DTOs;
using Setting.Domain.Entities;
using Setting.Infrastructure.Persistences;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Setting.Infrastructure.Repositories;

public sealed class CfmtRepository(SettingDbContext context) : ICfmtRepository
{
      public async Task<CardFormatDto> CreateCardFormatAsync(CardFormat domain, CancellationToken cancellationToken = default)
      {
            var data = await context.CardFormats.AddAsync(
                  new Persistences.Entities.CardFormat(domain)
            );

            var save = await context.SaveChangesAsync(cancellationToken);
            if(data.Entity is null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new CardFormatDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.fac,
                  data.Entity.offset,
                  data.Entity.function_id,
                  data.Entity.flag,
                  data.Entity.bits,
                  data.Entity.pe_ln,
                  data.Entity.pe_loc,
                  data.Entity.po_ln,
                  data.Entity.po_loc,
                  data.Entity.fc_ln,
                  data.Entity.fc_loc,
                  data.Entity.ch_ln,
                  data.Entity.ch_loc,
                  data.Entity.ic_ln,
                  data.Entity.ic_loc,
                  data.Entity.component_id,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }

      public async Task<CardFormatDto> DeleteByIdAsync(int id, CancellationToken ct = default)
      {
            var entity = await context.CardFormats
            .Where(x => x.id == id)
            .FirstOrDefaultAsync();

            if(entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            var data = context.CardFormats.Remove(entity);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity is null || save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return new CardFormatDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.fac,
                  data.Entity.offset,
                  data.Entity.function_id,
                  data.Entity.flag,
                  data.Entity.bits,
                  data.Entity.pe_ln,
                  data.Entity.pe_loc,
                  data.Entity.po_ln,
                  data.Entity.po_loc,
                  data.Entity.fc_ln,
                  data.Entity.fc_loc,
                  data.Entity.ch_ln,
                  data.Entity.ch_loc,
                  data.Entity.ic_ln,
                  data.Entity.ic_loc,
                  data.Entity.component_id,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }

      public async Task<CardFormatDto> GetByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.CardFormats
            .Where(x => x.id == id)
            .Select(x => new CardFormatDto(
                  x.id,
                  x.name,
                  x.fac,
                  x.offset,
                  x.function_id,
                  x.flag,
                  x.bits,
                  x.pe_ln,
                  x.pe_loc,
                  x.po_ln,
                  x.po_loc,
                  x.fc_ln,
                  x.fc_loc,
                  x.ch_ln,
                  x.ch_loc,
                  x.ic_ln,
                  x.ic_loc,
                  x.component_id,
                  x.location_id,
                  x.is_active
            ))
            .FirstOrDefaultAsync() ?? new CardFormatDto();
      }

      public async Task<IEnumerable<CardFormatDto>> GetByLocationIdAsync(int LocationId, CancellationToken ct = default)
      {
            return await context.CardFormats.AsNoTracking()
            .Where(x => x.location_id == LocationId || x.location_id == 0)
            .Select(x => new CardFormatDto(
                  x.id,
                  x.name,
                  x.fac,
                  x.offset,
                  x.function_id,
                  x.flag,
                  x.bits,
                  x.pe_ln,
                  x.pe_loc,
                  x.po_ln,
                  x.po_loc,
                  x.fc_ln,
                  x.fc_loc,
                  x.ch_ln,
                  x.ch_loc,
                  x.ic_ln,
                  x.ic_loc,
                  x.component_id,
                  x.location_id,
                  x.is_active
            ))
            .ToArrayAsync();
      }

      public async Task<Pagination<CardFormatDto>> GetCardFormatPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.CardFormats.AsNoTracking().Where(x => x.location_id == param.locationId || x.location_id == 0).AsQueryable();

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
                                  EF.Functions.ILike(x.bits.ToString(), pattern ) ||
                                  EF.Functions.ILike(x.fac.ToString(),pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) || 
                                  x.bits.ToString().Contains(search) ||
                                  x.fac.ToString().Contains(search)
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
            .Select(e => new CardFormatDto(
                  e.id,
                  e.name,
                  e.fac,
                  e.offset,
                  e.function_id,
                  e.flag,
                  e.bits,
                  e.pe_ln,
                  e.pe_loc,
                  e.po_ln,
                  e.po_loc,
                  e.fc_ln,
                  e.fc_loc,
                  e.ch_ln,
                  e.ch_loc,
                  e.ic_ln,
                  e.ic_loc,
                  e.component_id,
                  e.location_id,
                  e.is_active
            )).ToListAsync(ct);

            return new Pagination<CardFormatDto>(param.pageNumber, param.pageSize, count, (int)Math.Ceiling(count / (double)param.pageSize), res);
      }

      public async Task<short> GetLowestComponentIdAsync(int LocationId,CancellationToken ct = default)
      {
            return (short)await ComponentHelper.LowestUnassignedNumberAsync<Persistences.Entities.CardFormat>(
                  context,
                  x => x.location_id == LocationId || x.location_id == 0,
                  x => x.component_id,
                  7
                  );
      }

      public async Task<bool> IsAnyCardFormatNotSyncAsync(int LocationId, DateTime SyncAt, CancellationToken ct = default)
      {
            return await context.CardFormats.AsNoTracking()
            .AnyAsync(x => (x.location_id == LocationId || LocationId == 0) && x.updated_at > SyncAt);
      }

      public async Task<CardFormatDto> UpdateAsync(Domain.Entities.CardFormat domain, CancellationToken ct = default)
      {
            var entity = await context.CardFormats
            .Where(x => x.id == domain.Id)
            .FirstOrDefaultAsync();

            if(entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(domain);

            var data = context.CardFormats.Update(entity);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity is null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

            return new CardFormatDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.fac,
                  data.Entity.offset,
                  data.Entity.function_id,
                  data.Entity.flag,
                  data.Entity.bits,
                  data.Entity.pe_ln,
                  data.Entity.pe_loc,
                  data.Entity.po_ln,
                  data.Entity.po_loc,
                  data.Entity.fc_ln,
                  data.Entity.fc_loc,
                  data.Entity.ch_ln,
                  data.Entity.ch_loc,
                  data.Entity.ic_ln,
                  data.Entity.ic_loc,
                  data.Entity.component_id,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }
}