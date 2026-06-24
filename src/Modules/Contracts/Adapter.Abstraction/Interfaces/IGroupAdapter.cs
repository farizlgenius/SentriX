namespace Adapter.Abstraction.Interfaces;


public interface IGroupAdapter
{
      Task CreateUpdateLevel(
            string Mac,
            short DeviceComponentId,
            short ComponentId,
            List<(short DoorComponentId,short TimeZoneComponentId)> Doors
      );

      Task DeleteLevel(
            string Mac,
            short DeviceComponentId,
            short ComponentId
      );


}