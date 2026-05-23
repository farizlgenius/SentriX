namespace Adapter.Abstraction.Interfaces;

public interface IDoorAdapter
{
      Task CreateUpdateDoorAsync(
            string Mac,
            short DeviceComponentId,
            string Metadata,
            short FirstComponentId,
            short SecondComponentId = -1
      );

      Task DeleteDoorAsync(
            string Mac,
            short DeviceComponentId,
            string Metadata,
            short FirstComponentId,
            short SecondComponentId = -1
      );

      Task UpdateDoorAsync(
            string Mac,
            short DeviceComponentId,
            string Metadata,
            short FirstComponentId,
            short SecondComponentId = -1
      );
}