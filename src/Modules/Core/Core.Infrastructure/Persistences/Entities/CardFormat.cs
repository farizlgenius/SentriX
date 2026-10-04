using SharedKernel.Domain;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class CardFormat : BaseEntity
{

      public string name { get; set; } = string.Empty;
      public short fac { get; set; } = -1;
      public short bits { get; set; }
      public short even_parity_len { get; set; }
      public short even_parity_loc { get; set; }
      public short odd_parity_len { get; set; }
      public short odd_parity_loc { get; set; }
      public short fac_len { get; set; }
      public short fac_loc { get; set; }
      public short card_no_len { get; set; }
      public short card_no_loc { get; set; }
      public short issue_code_len { get; set; }
      public short issue_code_loc { get; set; }
      public string metadata {get; set; } = string.Empty;

      public CardFormat()
      {
      }

      public CardFormat(Domain.Entities.CardFormat domain) : base(domain.Guid)
      {
            this.name = domain.Name;
            this.fac = domain.Fac;
            this.bits = domain.Bits;
            this.even_parity_len = domain.EvenParityLen;
            this.even_parity_loc = domain.EvenParityLoc;
            this.odd_parity_len = domain.OddParityLen;
            this.odd_parity_loc = domain.OddParityLoc;
            this.fac_len = domain.FacLen;
            this.fac_loc = domain.FacLoc;
            this.card_no_len = domain.CardNoLen;
            this.card_no_loc = domain.CardNoLoc;
            this.issue_code_len = domain.IssueCodeLen;
            this.issue_code_loc = domain.IssueCodeLoc;
            metadata = domain.Metadata;

      }


}