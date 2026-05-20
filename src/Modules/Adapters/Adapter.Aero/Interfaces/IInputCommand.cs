using System;
using Adapter.Aero.Persistences.Entities;
using SharedKernel.Model;


namespace AeroAdapter.Application.Interfaces;

public interface IInputCommand
{
      CommandResponse InputPointSpecification(
            string Mac,
            short ScpId,
            short SioNumber,
            short InputNumber,
            short IcvtNumber,
            short Debounce,
            short HoldTime
      );
}
