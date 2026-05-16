using System;
using Adapter.Aero.Persistences.Entities;

namespace Adapter.Aero.Interfaces;

public interface IScpRepository
{
      Task<ScpDeviceSpecification> GetScpDeviceSpecificationByIdAndMacAsync(short ScpId,string Mac,CancellationToken ct=default);
      Task<AccessDatabaseSpecification> GetAccessDatabaseSpecificationByIdAndMacAsync(short ScpId,string Mac,CancellationToken ct=default);
      Task<DriverConfiguration> GetDriverConfigurationByIdAndMacAndPortNumberAsync(short ScpId,string Mac,int Port,CancellationToken ct=default);
      Task<List<DriverConfiguration>> GetDriverConfigurationsByIdAndMacAsync(short ScpId,string Mac,CancellationToken ct=default);
      Task<ElevatorAccessLevelSpecification> GetElevatorAccessLevelSpecificationByIdAndMacAsync(short ScpId,string Mac,CancellationToken ct=default);

      // Add 
      Task CreateDriverConfigurationAsync(DriverConfiguration config,CancellationToken ct=default);
      Task<int> GetAllSioAsync(CancellationToken ct=default);
      Task<bool> AddScpAsync(int ScpId,string Mac,CancellationToken ct = default);
      Task<bool> UpdateScpAsync(int ScpId,string Mac,CancellationToken ct = default);
      Task<string> MacByScpIdAsync(int ScpId,CancellationToken ct = default);
      Task<int> ScpIdByMacAsync(string Mac,CancellationToken ct = default);
      Task<(string Mac, int LocationId)?> GetMacAndLocationIdByScpIdAsync(int ScpId,CancellationToken ct = default);
}
