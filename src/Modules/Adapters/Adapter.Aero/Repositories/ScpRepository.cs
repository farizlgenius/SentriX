using System;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Persistences;
using Adapter.Aero.Persistences.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Aero.Repositories;

public sealed class ScpRepository(AeroDbContext context) : IScpRepository
{
      public async Task<DriverConfiguration> AddDriverConfigurationAsync(DriverConfiguration config,CancellationToken ct=default)
      {
            var data = await context.DriverConfigurations.AddAsync(config);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  return new DriverConfiguration();

            return data.Entity;
            
      }

      public async Task<bool> AddScpAsync(int ScpId, string Mac,CancellationToken ct = default)
      {
            var data = await context.Aeros.AddAsync(
                  new Aeros
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



      public async Task<AccessDatabaseSpecification> GetAccessDatabaseSpecificationByIdAndMacAsync(short ScpId, string Mac,CancellationToken ct=default)
      {
            return await context.AccessDatabaseSpecifications
            .AsNoTracking()
            .Where(x => x.scp_id == ScpId && x.mac.Equals(Mac))
            .FirstOrDefaultAsync(ct) ?? new AccessDatabaseSpecification();
      }

      public async Task<int> GetAeroIdByMacAsync(string Mac,CancellationToken ct = default)
      {
            return await context.Aeros.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.mac.Equals(Mac))
            .Select(x => x.id)
            .FirstOrDefaultAsync();
      }

      public async Task<int> GetAeroIdByScpIdAsync(int ScpId,CancellationToken ct = default)
      {
            return await context.Aeros.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.scp_id == ScpId)
            .Select(x => x.id)
            .FirstOrDefaultAsync();
      }

      public async Task<int> GetAllSioAsync(CancellationToken ct=default)
      {
           return await context.ScpDeviceSpecifications.AsNoTracking().OrderByDescending(x => x.id).Select(x => x.n_sio).FirstOrDefaultAsync(ct);
      }




      public async Task<ElevatorAccessLevelSpecification> GetElevatorAccessLevelSpecificationByIdAndMacAsync(short ScpId, string Mac,CancellationToken ct=default)
      {
            return await context.ElevatorAccessLevelSpecifications
            .AsNoTracking()
            .Where(x => x.mac.Equals(Mac) && x.scp_id == ScpId)
            .FirstOrDefaultAsync(ct) ?? new ElevatorAccessLevelSpecification();
      }

      public async Task<(string Mac, int LocationId)?> GetMacAndLocationIdByScpIdAsync(int scpId, CancellationToken ct = default)
      {
            var result = await context.Aeros
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

      public async Task<int> GetScpIdByMacAsync(string Mac,CancellationToken ct = default)
      {
            return await context.Aeros.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.mac.Equals(Mac))
            .Select(x => x.scp_id)
            .FirstOrDefaultAsync();
      }

      public async Task<string> MacByScpIdAsync(int ScpId,CancellationToken ct=default)
      {
            return await context.Aeros.AsNoTracking().Where(x => x.scp_id == ScpId).Select(x => x.mac).FirstOrDefaultAsync(ct) ?? string.Empty;
      }

      public async Task<int> ScpIdByMacAsync(string Mac,CancellationToken ct=default)
      {
            return await context.Aeros.AsNoTracking().Where(x => x.mac.Equals(Mac)).Select(x => x.scp_id).FirstOrDefaultAsync(ct);
      }

      public async Task<bool> UpdateScpAsync(int ScpId, string Mac,CancellationToken ct=default)
      {
            var entity = await context.Aeros.Where(x => x.mac.Equals(Mac)).OrderByDescending(x => x.id).FirstOrDefaultAsync(ct);

            if(entity == null)
                  return false;

            entity.scp_id = ScpId;
            context.Aeros.Update(entity);
            var save = await context.SaveChangesAsync(ct);

            if(save <= 0)
                  return false;

            return true;
      }
}
