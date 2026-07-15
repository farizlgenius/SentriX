namespace Adapter.Abstraction.Interfaces;


public interface IGroupAdapter
{
      Task CreateGroup(
            string Mac,
            short DeviceComponentId,
            short ComponentId,
            List<(short DoorComponentId,short TimeZoneComponentId)> Doors
      );

      Task DeleteGroup(
            string Mac,
            short DeviceComponentId,
            short ComponentId
      );


}