using System;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Persistences;
using Adapter.Aero.Persistences.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Aero.Repositories;

public sealed class ScpRepository(AeroDbContext context) : IScpRepository
{
      public async Task<bool> AddScpAsync(int ScpId, string Mac,CancellationToken ct = default)
      {
            var data = await context.Scps.AddAsync(
                  new Scp
                  {
                        scp_id = ScpId,
                        mac = Mac
                  }
            );

            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null && save <= 0)
            {
                  return false;
            }

            return true;
      }

      public async Task CreateDriverConfigurationAsync(DriverConfiguration config,CancellationToken ct=default)
      {
            await context.DriverConfigurations.AddAsync(config);
            await context.SaveChangesAsync(ct);
      }

      public async Task<AccessDatabaseSpecification> GetAccessDatabaseSpecificationByIdAndMacAsync(short ScpId, string Mac,CancellationToken ct=default)
      {
            return await context.AccessDatabaseSpecifications
            .AsNoTracking()
            .Where(x => x.scp_id == ScpId && x.mac.Equals(Mac))
            .FirstOrDefaultAsync(ct) ?? new AccessDatabaseSpecification();
      }

      public async Task<int> GetAllSioAsync(CancellationToken ct=default)
      {
           return await context.ScpDeviceSpecifications.AsNoTracking().OrderByDescending(x => x.id).Select(x => x.n_sio).FirstOrDefaultAsync(ct);
      }

      public async Task<DriverConfiguration> GetDriverConfigurationByIdAndMacAndPortNumberAsync(short ScpId, string Mac,int Port,CancellationToken ct=default)
      {
            return await context.DriverConfigurations
            .AsNoTracking()
            .Where(x => x.scp_id == ScpId && x.mac.Equals(Mac) && x.port_number == Port)
            .FirstOrDefaultAsync(ct) ?? new DriverConfiguration();
      }

      public async Task<List<DriverConfiguration>> GetDriverConfigurationsByIdAndMacAsync(short ScpId, string Mac,CancellationToken ct=default)
      {
            return await context.DriverConfigurations
            .AsNoTracking()
            .Where(x => x.scp_id == ScpId && x.mac.Equals(Mac))
            .ToListAsync(ct) ?? new List<DriverConfiguration>();
      }

      public async Task<ElevatorAccessLevelSpecification> GetElevatorAccessLevelSpecificationByIdAndMacAsync(short ScpId, string Mac,CancellationToken ct=default)
      {
            return await context.ElevatorAccessLevelSpecifications
            .AsNoTracking()
            .Where(x => x.mac.Equals(Mac) && x.scp_id == ScpId)
            .FirstOrDefaultAsync(ct) ?? new ElevatorAccessLevelSpecification();
      }

      public async Task<(string Mac, int LocationId)?> GetMacAndLocationIdByScpIdAsync(int scpId,CancellationToken ct = default)
{
    var result = await context.Scps
        .AsNoTracking()
        .Where(x => x.scp_id == scpId)
        .OrderByDescending(x => x.id)
        .Select(x => new ValueTuple<string, int>(x.mac, x.location_id))
        .FirstOrDefaultAsync(ct);

    return result == default ? null : result;
}

      public async Task<ScpDeviceSpecification> GetScpDeviceSpecificationByIdAndMacAsync(short ScpId, string Mac,CancellationToken ct=default)
      {
            return await context.ScpDeviceSpecifications
            .AsNoTracking()
            .Where(x => x.scp_id == ScpId && x.mac.Equals(Mac))
            .FirstOrDefaultAsync(ct) ?? new ScpDeviceSpecification();
      }

     

      public async Task<string> MacByScpIdAsync(int ScpId,CancellationToken ct=default)
      {
            return await context.Scps.AsNoTracking().Where(x => x.scp_id == ScpId).Select(x => x.mac).FirstOrDefaultAsync(ct) ?? string.Empty;
      }

      public async Task<int> ScpIdByMacAsync(string Mac,CancellationToken ct=default)
      {
            return await context.Scps.AsNoTracking().Where(x => x.mac.Equals(Mac)).Select(x => x.scp_id).FirstOrDefaultAsync(ct);
      }

      public async Task<bool> UpdateScpAsync(int ScpId, string Mac,CancellationToken ct=default)
      {
            var entity = await context.Scps.Where(x => x.mac.Equals(Mac)).OrderByDescending(x => x.id).FirstOrDefaultAsync(ct);

            if(entity == null)
                  return false;

            entity.scp_id = ScpId;
            context.Scps.Update(entity);
            var save = await context.SaveChangesAsync(ct);

            if(save <= 0)
                  return false;

            return true;
      }
}
