using Adapter.Aero.Persistences.Entities;

namespace Adapter.Aero.Interfaces;

public interface ICpWriter
{
      Task<bool> OutputPointSpecification(OutputPointSpecification config);
      Task<bool> ControlPointConfiguration(ControlPointConfiguration config);
}