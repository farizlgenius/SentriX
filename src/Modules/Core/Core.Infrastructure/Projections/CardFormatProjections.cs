using System.Linq.Expressions;
using Core.Contract.DTOs.CardFormat;

namespace Core.Infrastructure.Projections;

public static class CardFormatProjections
{
      // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.CardFormat, CardFormatDto>> ToDto = 
        x => new CardFormatDto(
            x.guid,
            x.name,
            x.fac,
            x.bits,
            x.even_parity_len,
            x.even_parity_loc,
            x.odd_parity_len,
            x.odd_parity_len,
            x.fac_len,
            x.fac_loc,
            x.card_no_len,
            x.card_no_loc,
            x.issue_code_len,
            x.issue_code_loc,
            x.is_active,
            x.is_default
        );
}