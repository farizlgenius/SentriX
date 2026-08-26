using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Card : BaseEntity
{
      public short bits { get; set; }
      public int fac { get; set; }
      public int card_number { get; set; }
      public int? user_id { get; set; }
      public User user { get; set; } = default!;

      public Card() { }

      public Card(Domain.Entities.Card d) : base(d.Guid)
      {
            this.bits = d.Bits;
            this.fac = d.Fac;
            this.card_number = d.CardNumber;
            this.user_id = d.UserId;
      }


}