using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Persistences;
using Adapter.Aero.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using Output.Contract.DTOs;

namespace Adapter.Aero.Repositories;

public sealed class CpRepository(AeroDbContext context) : ICpRepository
{
      public async Task<ControlPointConfiguration> AddControlPointConfigurationAsync(ControlPointConfiguration config,CancellationToken ct = default)
      {
            var cp_number = await ComponentHelper.LowestUnassignedNumberAsync<ControlPointConfiguration>(
                  context,
                  x => x.aero_id == config.aero_id,
                  x => x.cp_number,
                  ct
            );
            config.cp_number = (short)cp_number;
            var data = await context.ControlPointConfigurations.AddAsync(config);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  return data.Entity ?? new ControlPointConfiguration();

            

            return await context.ControlPointConfigurations
                              .Include(x => x.aero)   // load related table
                              .FirstAsync(x => x.id == data.Entity.id, ct);
      }

      public async Task<OutputPointSpecification> AddOutputPointSpeicificationAsync(OutputPointSpecification config,CancellationToken ct = default)
      {
            
            var data = await context.OutputPointSpecifications.AddAsync(config);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  return data.Entity ?? new OutputPointSpecification();

           return await context.OutputPointSpecifications
                              .Include(x => x.aero)   // load related table
                              .FirstAsync(x => x.id == data.Entity.id, ct);
      }
}