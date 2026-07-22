using System;

namespace Adapter.Abstraction.Interfaces;

public interface IInputAdapter
{
      Task CreateUpdateMonitorPoint(
             Guid Guid,
            Guid DeviceGuid,
            string metadata,
            Guid ModuleGuid
      );

      Task DeleteMonitorPoint(
           Guid Guid,
            Guid DeviceGuid,
            string Metadata
      );

      Task MaskMonitorPoint(
            Guid Guid,
            Guid DeviceGuid,
            bool IsMask
      );

      Task CreateUpdateMonitorGroup(
            Guid Guid,
            Guid DeviceGuid,
            List<(short Type,Guid InputGuid)> Inputs
      );

      Task DeleteMonitorGroup(
            Guid Guid,
            Guid DeviceGuid
      );
}
