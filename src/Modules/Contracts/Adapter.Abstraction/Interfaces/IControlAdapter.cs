using System;
using Output.Contract.DTOs;
using SharedKernel.Domain;

namespace Adapter.Abstraction.Interfaces;

public interface IControlAdapter
{
      Task CreateAsync(CreateOutputDto dto);
      Task<IEnumerable<OptionDto>> GetRelayModeAsync();
      Task TriggerOutputAsync(int moduleId,int Command);
}
