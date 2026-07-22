using Door.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface IDoorAdapter
{
      Task CreateUpdateDoorAsync(
            Guid DeviceGuid,
            Guid DoorGuid,
            string Metadata
      );

      Task DeleteDoorAsync(
            Guid DeviceGuid,
            Guid DoorGuid,
            string Metadata
      );

      Task UpdateDoorAsync(
            Guid DeviceGuid,
            Guid DoorGuid,
            string Metadata
      );
}