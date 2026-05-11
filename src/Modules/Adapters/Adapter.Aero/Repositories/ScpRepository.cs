using System;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Persistences;
using Adapter.Aero.Persistences.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Aero.Repositories;

public sealed class ScpRepository(AeroDbContext context) : IScpRepository
{


      public async Task<AccessDatabaseSpecification> GetAccessDatabaseSpecificationByIdAndMacAsync(short ScpId, string Mac)
      {
            return await context.AccessDatabaseSpecifications
            .AsNoTracking()
            .Where(x => x.scp_id == ScpId && x.mac.Equals(Mac))
            .FirstOrDefaultAsync() ?? new AccessDatabaseSpecification();
      }


      public async Task<DriverConfiguration> GetDriverConfigurationByIdAndMacAndPortNumberAsync(short ScpId, string Mac, short Port)
      {
            return await context.DriverConfigurations
            .AsNoTracking()
            .Where(x => x.scp_id == ScpId && x.mac.Equals(Mac) && x.port_number == Port)
            .FirstOrDefaultAsync() ?? new DriverConfiguration();
      }

      public async Task<ElevatorAccessLevelSpecification> GetElevatorAccessLevelSpecificationByIdAndMacAsync(short ScpId, string Mac)
      {
            return await context.ElevatorAccessLevelSpecifications
            .AsNoTracking()
            .Where(x => x.mac.Equals(Mac) && x.scp_id == ScpId)
            .FirstOrDefaultAsync() ?? new ElevatorAccessLevelSpecification();
      }




      public async Task<ScpDeviceSpecification> GetScpDeviceSpecificationByIdAndMacAsync(short ScpId, string Mac)
      {
            return await context.ScpDeviceSpecifications
            .AsNoTracking()
            .Where(x => x.scp_id == ScpId && x.mac.Equals(Mac))
            .FirstOrDefaultAsync() ?? new ScpDeviceSpecification();
      }

      public async Task<SioPanelConfiguration> GetSioPanelConfigurationByIdAndMacAndAddressAsync(short ScpId, string Mac, short Address)
      {
            return await context.SioPanelConfigurations
            .AsNoTracking()
            .Where(x => x.scp_id == ScpId && x.mac.Equals(Mac))
            .FirstOrDefaultAsync() ?? new SioPanelConfiguration();
      }


}
