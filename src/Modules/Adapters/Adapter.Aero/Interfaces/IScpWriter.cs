using System;
using Adapter.Aero.Enums;
using Adapter.Aero.Persistences.Entities;



namespace AeroAdapter.Application.Interfaces;

public interface IScpWriter
{
      // Below Command need to reset controller if change
      Task<bool> ScpDeviceSpecification(short ScpId,string Mac,ScpDeviceSpecification Spec);
      Task<bool> AccessDatabaseSpecification(short ScpId,string Mac,AccessDatabaseSpecification Spec);

      // End

      Task<bool> TimeSet(short ScpId,string Mac);
      
      bool CreateChannel();
      bool SendASCIICommandAsync(string Command);
      Task<bool> WebConfigRead(short ScpId,string Mac,WebConfigReadType Type);  
      Task<bool> DriverConfiguration(short ScpId,string Mac,DriverConfiguration config);
      Task<bool> ReadsConfiguration(short ScpId,string Mac,WebConfigReadType Type);
      Task<bool> SCPStructureStatusRead(short ScpId,string Mac,List<short> StructureList);
      Task<bool> ElevatorAccessLevelSpecification(short ScpId,string Mac,ElevatorAccessLevelSpecification spec);
      Task<bool> SCPReset(short ScpId,string Mac);

}
