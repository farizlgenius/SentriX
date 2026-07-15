using Adapter.Amico.Interface;

namespace Adapter.Amico.Adapters;

public sealed class AmicoGroupAdapter(
      IGroupCommand command
) : IAmicoGroupAdapter
{
      public async Task CreateGroup(string Mac, short DeviceComponentId, short ComponentId, List<(short DoorComponentId, short TimeZoneComponentId)> Doors)
      {
            throw new NotImplementedException();
      }

      public async Task DeleteGroup(string Mac, short DeviceComponentId, short ComponentId)
      {
            throw new NotImplementedException();
      }
}