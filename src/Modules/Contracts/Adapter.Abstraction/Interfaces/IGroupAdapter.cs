namespace Adapter.Abstraction.Interfaces;


public interface IGroupAdapter
{
      Task CreateGroup(
            Guid Guid,
             string Name,
            List<(Guid DeviceGuid,Guid DoorGuid,Guid TzGuid)> Doors
      );

      Task UpdateGroup(
              Guid Guid,
             string Name,
            List<(Guid DeviceGuid,Guid DoorGuid,Guid TzGuid)> Doors
      );

      Task DeleteGroup(
            Guid DeviceGuid,
            Guid GroupGuid
      );


}