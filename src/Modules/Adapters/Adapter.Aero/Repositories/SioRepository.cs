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
            x => x.scp_id == config.scp_id,
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
            .AnyAsync(x => x.scp_id == ScpId && x.sio_number == SioId);
      }

       public async Task<SioPanelConfiguration> GetSioPanelConfigurationByIdAndMacAndAddressAsync(short ScpId, string Mac, short Address,CancellationToken ct=default)
      {
            return await context.SioPanelConfigurations
            .AsNoTracking()
            .Where(x => x.scp_id == ScpId && x.mac.Equals(Mac) && x.address == Address)
            .FirstOrDefaultAsync(ct) ?? new SioPanelConfiguration();
      }

      public async Task<int> GetModuleIdByScpIdAndSioIdAsync(int ScpId, int ModuleId)
      {
            return await context.SioPanelConfigurations.AsNoTracking()
            .Where(x => x.sio_number == ModuleId && x.scp_id == ScpId)
            .OrderByDescending(x => x.id)
            .Select(x => x.module_id)
            .FirstOrDefaultAsync();
      }
}