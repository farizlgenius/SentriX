namespace Adapter.Abstraction.Interfaces;


public interface IGroupAdapter
{
      Task CreateGroup(
             string Name,
            short ComponentId,
            List<(string Mac, short DeviceComponentId, short DoorComponentId, short TimeZoneComponentId)> Doors
      );

      Task UpdateGroup(
             string Name,
            short ComponentId,
            List<(string Mac, short DeviceComponentId, short DoorComponentId, short TimeZoneComponentId)> Doors
      );

      Task DeleteGroup(
            string Mac,
            short DeviceComponentId,
            short ComponentId
      );


}