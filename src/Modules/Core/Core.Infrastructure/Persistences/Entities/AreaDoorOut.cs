using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class AreaDoorOut
{
      [Key]
      public int id {get; set; }
      public int area_id {get; set; }
      public Area area {get; set; } = default!;
      public int door_id {get; set; }
      public Door door {get; set; } = default!;
      public AreaDoorOut()
      {
            
      }

      public AreaDoorOut(int doorId)
      {
            door_id = doorId;
      }
}