using System;
using Adapter.Aero.Persistences.Entities;


namespace AeroAdapter.Application.Interfaces;

public interface IMpWriter
{
      Task<bool> InputPointSpecification(short ScpId,string Mac,InputPointSpecification spec);
}
