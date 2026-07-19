using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class Card
{

      [Key]
      public int id { get; set; }
      public Guid guid { get; set; }
      public short bits { get; set; }
      public int card_number { get; set; }
      public DateTime created_at { get; set; }
      public DateTime updated_at { get; set; }
      public Guid? user_guid { get; set; }
      public Users user { get; set; } = default!;

      public Card() { }

      public Card(Domain.Entities.Card d)
      {
            this.guid = d.Guid;
            this.bits = d.Bits;
            this.card_number = d.CardNumber;
            this.created_at = DateTime.UtcNow;
            this.updated_at = DateTime.UtcNow;
            this.user_guid = d.UserGuid;
      }

      public void Update(Domain.Entities.Card d)
      {
            this.bits = d.Bits;
            this.card_number = d.CardNumber;
            this.updated_at = DateTime.UtcNow;
      }

}