using System;
using Adapter.Aero.Persistences.Entities;

namespace AeroAdapter.Application.Interfaces;

public interface IMpRepository
{
      Task<InputPointSpecification> AddInputPointSpecificationAsync(InputPointSpecification config,CancellationToken ct = default);
}
