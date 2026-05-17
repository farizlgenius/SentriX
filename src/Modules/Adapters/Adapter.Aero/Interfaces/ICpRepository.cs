using Adapter.Aero.Persistences.Entities;
using Output.Contract.DTOs;

namespace Adapter.Aero.Interfaces;

public interface ICpRepository
{
      Task<ControlPointConfiguration> AddControlPointConfigurationAsync(ControlPointConfiguration config,CancellationToken ct = default);
      Task<OutputPointSpecification> AddOutputPointSpeicificationAsync(OutputPointSpecification config,CancellationToken ct = default);
}