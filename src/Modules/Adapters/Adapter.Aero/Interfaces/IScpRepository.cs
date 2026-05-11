using System;
using Adapter.Aero.Persistences.Entities;

namespace Adapter.Aero.Interfaces;

public interface IScpRepository
{
      Task<ScpDeviceSpecification> GetScpDeviceSpecificationByIdAndMacAsync(short ScpId,string Mac);
      Task<AccessDatabaseSpecification> GetAccessDatabaseSpecificationByIdAndMacAsync(short ScpId,string Mac);
      Task<DriverConfiguration> GetDriverConfigurationByIdAndMacAndPortNumberAsync(short ScpId,string Mac,short Port);
      Task<SioPanelConfiguration> GetSioPanelConfigurationByIdAndMacAndAddressAsync(short ScpId,string Mac,short Address);
      Task<ElevatorAccessLevelSpecification> GetElevatorAccessLevelSpecificationByIdAndMacAsync(short ScpId,string Mac);
}
