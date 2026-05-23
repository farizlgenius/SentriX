using SharedKernel.Model;

namespace Adapter.Aero.Interfaces;

public interface IGroupCommand
{
      CommandResponse AccessLevelConfigurationExtended(
           string Mac,short ScpId,short ComponentId,short[] Timezone
      );
}