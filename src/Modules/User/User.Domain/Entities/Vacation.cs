using SharedKernel.Domain;

namespace User.Domain.Entities;

public sealed class Vacation : BaseDomain
{
      public DateTime VacationDate { get; set; }
      public short VacationDay {get; set;}
      public int UserId {get; set;}
      public Vacation(
            int id, 
            short componentId,
            DateTime vacationDate,
            short vacationDay,
            int userId, 
            int locationId, 
            bool IsActive
            ) : base(id, componentId, locationId, IsActive)
      {
            this.VacationDate = VacationDate;
            this.VacationDay = VacationDay;
            this.UserId = userId;
      }
}