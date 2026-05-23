using System;

namespace Adapter.Abstraction.Interfaces;

public interface IMonitorAdapter
{
      Task CreateUpdateMonitorPoint(
             string Mac,
            short ComponentId,
            short DeviceComponentId,
            short ModuleComponentId,
            short InputNo,
            string Metadata
      );

      Task DeleteMonitorPoint(
            string Mac,
            short ComponentId,
            short DeviceComponentId,
            short InputNo,
            string Metadata
      );

      Task MaskMonitorPoint(
            string Mac,
            short DeviceComponentId,
            short ComponentId,
            bool IsMask
      );

      Task CreateUpdateMonitorGroup(
             string Mac,
            short ScpId,
            short MpgNumber,
            string Metadata
      );

      Task DeleteMonitorGroup(
             string Mac,
            short ComponentId,
            short MpgNumber
      );
}
