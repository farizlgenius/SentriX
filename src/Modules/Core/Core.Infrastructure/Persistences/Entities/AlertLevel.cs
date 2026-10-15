namespace Core.Infrastructure.Persistences.Entities;

public sealed class AlertLevel : BaseEntity
{
      public string name {get; set; } = string.Empty;
      public string color_code {get; set; } = string.Empty; // Hex string
      // Relation
      public ICollection<Alert> alerts {get; set;} = default!;
      public AlertLevel()
      {
            
      }

      public AlertLevel(Domain.Entities.AlertLevel d) : base(d.Guid)
      {
            name = d.Name;
            color_code = d.ColorCode;
      }
}