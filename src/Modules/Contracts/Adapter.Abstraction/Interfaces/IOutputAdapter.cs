using System;
using Output.Contract.DTOs;
using SharedKernel.Domain;

namespace Adapter.Abstraction.Interfaces;

public interface IOutputAdapter
{
      Task CreateAsync(
             string Mac,
            short ComponentId,
            short DeviceComponentId,
            short ModuleComponentId,
            short OutputNo,
            short DriverMode,
            short OffineMode,
            short DefaultPulse
      );
      Task DeleteAsync(
            string Mac,
            short ScpId,
            short CpNumber,
            short OpNumber,
            short DefaultPulse
      );
      Task<IEnumerable<OptionDto>> GetRelayModeAsync();
      Task TriggerOutputAsync(string Mac,short ScpId,short CpId,short Command);
      Task UpdateAsync(
             string Mac,
            short ComponentId,
            short DeviceComponentId,
            short ModuleComponentId,
            short OutputNo,
            short DriverMode,
            short OffineMode,
            short DefaultPulse
      );


}
