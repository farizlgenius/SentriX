using Adapter.Aero.Helpers;
using Adapter.Aero.Persistences;
using Adapter.Aero.Persistences.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Aero.Repositories;

public sealed class SioRepository(AeroDbContext context) : ISioRepository
{
      public async Task<SioPanelConfiguration> AddSioPanelConfigurationAsync(SioPanelConfiguration config, CancellationToken ct = default)
      {
            // Get Mac Sio
            var max = await context.ScpDeviceSpecifications.AsNoTracking().Select(x => x.n_sio).FirstOrDefaultAsync();
            if(max == 0)
                  return new SioPanelConfiguration();

            // Generate Sio
            var com = (short)(await ComponentHelper.LowestUnassignedNumberAsync<SioPanelConfiguration>(context,
            x => x.aero_id == config.aero_id,
            x => x.sio_number,
            max
            ));

            if(com <= 0)
                  return new SioPanelConfiguration();

            config.sio_number = com;

            var data = await context.SioPanelConfigurations.AddAsync(config);
            var save = await context.SaveChangesAsync(ct);
            if(data.Entity == null && save <= 0)
                  return new SioPanelConfiguration();

            return data.Entity ?? new SioPanelConfiguration();
      }

      public async Task<bool> IsAnySioByScpIdAndSioIdAsync(int ScpId, int SioId, CancellationToken ct = default)
      {
            return await context.SioPanelConfigurations.AsNoTracking()
            .AnyAsync(x => x.aero.scp_id == ScpId && x.sio_number == SioId);
      }


      public async Task<int> GetModuleIdByScpIdAndSioIdAsync(int ScpId, int ModuleId,CancellationToken ct = default)
      {
            return await context.SioPanelConfigurations.AsNoTracking()
            .Where(x => x.sio_number == ModuleId && x.aero.scp_id == ScpId)
            .OrderByDescending(x => x.id)
            .Select(x => x.module_id)
            .FirstOrDefaultAsync(ct);
      }

      public async Task<int> GetModuleIdByMacAndSioNumberAsync(string Mac, short SioNo,CancellationToken ct = default)
      {
            return await context.SioPanelConfigurations.AsNoTracking()
            .Where(x => x.aero.mac.Equals(Mac) && x.sio_number == SioNo)
            .OrderByDescending(x => x.id)
            .Select(x => x.module_id)
            .FirstOrDefaultAsync(ct);
      }

      public async Task<short> GetSioNoByModuleIdAsync(int ModuleId, CancellationToken ct = default)
      {
             return await context.SioPanelConfigurations.AsNoTracking()
            .Where(x => x.module_id == ModuleId)
            .OrderByDescending(x => x.id)
            .Select(x => x.sio_number)
            .FirstOrDefaultAsync(ct);
      }

      public async Task<SioPanelConfiguration> GetSioPanelConfigurationByModuleIdAsync(int ModuleId, CancellationToken ct = default)
      {
            return await context.SioPanelConfigurations.AsNoTracking()
            .Include(x => x.aero)
            .Where(x => x.module_id == ModuleId)
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync() ?? new SioPanelConfiguration();
      }

      public async Task<int> GetAeroIdByModuleIdAsync(int ModuleId)
      {
            return await context.SioPanelConfigurations.AsNoTracking()
            .Where(x => x.module_id == ModuleId)
            .OrderByDescending(x => x.id)
            .Select(x => x.aero_id)
            .FirstOrDefaultAsync();
      }
}