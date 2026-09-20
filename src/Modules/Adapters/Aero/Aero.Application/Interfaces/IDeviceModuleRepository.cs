using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface IDeviceModuleRepository
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
      CommandResponse SioStatusRequest(string Mac,short ScpId, int First,int Count);
}