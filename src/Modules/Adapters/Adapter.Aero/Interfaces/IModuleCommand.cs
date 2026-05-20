using System;
using Adapter.Aero.Persistences.Entities;
using SharedKernel.Model;

namespace AeroAdapter.Application.Interfaces;

public interface IModuleCommand
{
      CommandResponse SioPanelConfiguration(
           string Mac,
           short ScpId,
           short SioNumber,
           short nInput,
           short nOutput,
           short nReader,
           short Model,
           short Enable,
           short Port,
           short Address,
           short Emax,
           short Flags,
           short nSioNextIn,
           short nSioNextOut,
           short nSioNextRdr 
      );
      CommandResponse SioStatusRequest(string Mac,short ScpId,int First,int Count);
}
