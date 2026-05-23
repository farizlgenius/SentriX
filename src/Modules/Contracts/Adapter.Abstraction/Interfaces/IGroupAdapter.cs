namespace Adapter.Abstraction.Interfaces;


public interface IGroupAdapter
{
      Task CreateUpdateLevel(
            string Mac,
            short DeviceComponentId,
            short ComponentId,
            string Metadata
      );

      Task DeleteLevel(
            string Mac,
            short DeviceComponentId,
            short ComponentId
      );


}