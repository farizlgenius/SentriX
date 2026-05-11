using System;
using Adapter.Aero.Persistences.Entities;

namespace AeroAdapter.Application.Interfaces;

public interface IMpRepository
{
      Task<InputPointSpecification> GetInputPointSpecificationByIdAndMacAndSioNumber(short ScpId,string Mac,short SioNumber);
}
