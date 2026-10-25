using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class AreaDoorIn
{
      [Key]
      public int id {get; set; }
      public int area_id {get; set; }
      public Area area {get; set; } = default!;
      public int door_id {get; set; }
      public Door door {get; set; } = default!;
      public AreaDoorIn()
      {
            
      }

      public AreaDoorIn(int doorId)
      {
            door_id = doorId;
      }
}