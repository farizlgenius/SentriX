using System;
using Output.Contract.DTOs;
using SharedKernel.Domain;

namespace Adapter.Abstraction.Interfaces;

public interface IOutputAdapter
{
      Task CreateAsync(
             Guid Guid,
            Guid DeviceGuid,
            string Metadata,
            Guid ModuleGuid
      );
      Task DeleteAsync(
             Guid Guid,
            Guid DeviceGuid,
            string Metadata
      );
      Task<IEnumerable<OptionDto>> GetRelayModeAsync();
      Task TriggerOutputAsync(
            Guid Guid,
            Guid DeviceGuid,
            short Command
            );
      Task UpdateAsync(
             Guid Guid,
            Guid DeviceGuid,
            string Metadata,
            Guid ModuleGuid
      );

      Task CommandOutputAsync(
            Guid Guid,
            Guid DeviceGuid,
            short Command
      );


}
