using System;
using Adapter.Aero.Persistences.Entities;

namespace AeroAdapter.Application.Interfaces;

public interface ISioWriter
{
      Task<bool> SioPanelConfiguration(short ScpId,string Mac,SioPanelConfiguration config);
      Task<bool> SioStatusRequest(short ScpId,string Mac,int First,int Count);
}
