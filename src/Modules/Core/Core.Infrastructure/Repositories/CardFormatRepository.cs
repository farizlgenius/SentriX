using Core.Application.Interfaces;
using Core.Contract.DTOs.CardFormat;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Core.Infrastructure.Repositories;

public sealed class CardFormatRepository(CoreDbContext context) : ICardFormatRepository
{
      public Task AddAsync(CardFormat entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<CardFormatDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<CardFormatDto>> GetByLocationAsync(Guid locationGuid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Guid> GetGuidByIdAsync(int id, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<Pagination<CardFormatDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.CardFormats
                  .AsNoTracking()
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
                                  EF.Functions.ILike(x.bits.ToString(), pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.bits.ToString().Contains(search) 
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

            var res = await query
                  .AsNoTracking()
                  .OrderByDescending(e => e.created_at)
                  .Skip((param.pageNumber - 1) * param.pageSize)
                  .Take(param.pageSize)
                  .Select(x => new CardFormatDto(
                        x.guid,
                        x.name,
                        x.fac,
                        x.bits,
                        x.even_parity_len,
                        x.even_parity_loc,
                        x.odd_parity_len,
                        x.odd_parity_loc,
                        x.fac_len,
                        x.fac_loc,
                        x.card_no_len,
                        x.card_no_loc,
                        x.issue_code_len,
                        x.issue_code_loc,
                        x.location == null ? Guid.Empty : x.location.guid,
                        x.is_active,
                        x.is_default
                  )).ToListAsync();

            return new Pagination<CardFormatDto>(
                  param.pageNumber,
                  param.pageSize,
                  count,
                  (int)Math.Ceiling(count / (double)param.pageSize),
                  res
                  );
      }

      public Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task UpdateAsync(CardFormat entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}