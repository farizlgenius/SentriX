using System;

namespace Adapter.Abstraction.Interfaces;

public interface IInputAdapter
{
      Task CreateUpdateMonitorPoint(
             string Mac,
            short ComponentId,
            short DeviceComponentId,
            short ModuleComponentId,
            short InputNo,
            short SensorMode,
            short Debounce,
            short HoldTime,
            short LogFunction,
            short LatchMode,
            short DelayEntry,
            short DelayExit
      );

      Task DeleteMonitorPoint(
            string Mac,
            short ComponentId,
            short DeviceComponentId,
            short InputNo,
            short SensorMode,
            short Debounce,
            short HoldTime,
            short LogFunction,
            short LatchMode,
            short DelayEntry,
            short DelayExit
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
            List<(short Type,short Number)> Inputs
      );

      Task DeleteMonitorGroup(
             string Mac,
            short ComponentId,
            short MpgNumber
      );
}
