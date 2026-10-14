using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class AlertEventDetail
{
      [Key]
      public int id {get; set; }
      public int alert_id {get; set ;}
      public Alert alert {get; set; } = default!;
      public int event_detail_id {get; set ;}
      public EventDetail event_detail {get; set ;} = default!;
      public AlertEventDetail()
      {
            
      }

      public AlertEventDetail(int eventDetail)
      {
            event_detail_id = eventDetail;
      }
}