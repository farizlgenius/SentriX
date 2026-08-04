using Door.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface IDoorAdapter
{
      Task CreateAsync(
            Guid DeviceGuid,
            Guid DoorGuid,
            string Metadata
      );

      Task DeleteAsync(
            Guid DeviceGuid,
            Guid DoorGuid,
            string Metadata
      );

      Task UpdateAsync(
            Guid DeviceGuid,
            Guid DoorGuid,
            string Metadata
      );
}