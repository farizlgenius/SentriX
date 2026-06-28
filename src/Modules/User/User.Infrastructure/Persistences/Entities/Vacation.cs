
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class Vacation : BaseEntity
{
      public DateTime vacation_date { get; set; }
      public short vacation_day {get; set;}
      public Users? users { get; set; }
      public Vacation()
      {
            
      }
}