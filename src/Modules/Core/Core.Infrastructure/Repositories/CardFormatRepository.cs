using Core.Application.Interfaces;
using Core.Contract.DTOs.CardFormat;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Core.Infrastructure.Repositories;

public sealed class CardFormatRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.CardFormat,Domain.Entities.CardFormat,CardFormatDto>(
context,
d => new Persistences.Entities.CardFormat(d),
(en,dom) =>
{
    en.name = dom.Name;
    en.bits = dom.Bits;
    en.fac = dom.Fac;
    en.even_parity_len = dom.EvenParityLen;
    en.even_parity_loc = dom.EvenParityLoc;
    en.odd_parity_len = dom.OddParityLen;
    en.odd_parity_loc = dom.OddParityLoc;
    en.fac_len = dom.FacLen;
    en.fac_loc = dom.FacLoc;
    en.card_no_len = dom.CardNoLen;
    en.card_no_loc = dom.CardNoLoc;
    en.issue_code_len = dom.IssueCodeLen;
    en.issue_code_loc = dom.IssueCodeLoc;
    en.updated_at = DateTime.UtcNow;  
},
CardFormatProjections.ToDto
),ICardFormatRepository
{

      public async Task<IEnumerable<CardFormatDto>> GetAllAsync(CancellationToken ct = default)
      {
            return await Context.CardFormats
                  .AsNoTracking()
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
                        x.is_active,
                        x.is_default
                  )).ToArrayAsync();
      }





      public async Task<Pagination<CardFormatDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
                  param,
                  param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
                  searchFields:[x => x.name,x => x.bits.ToString()],
                  ct:ct);
      }



      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

     
}