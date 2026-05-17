using System;
using Adapter.Aero.Persistences;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Aero.Repositories;

public sealed class MpRepository(AeroDbContext context) : IMpRepository
{
      public async Task<InputPointSpecification> AddInputPointSpecificationAsync(InputPointSpecification config,CancellationToken ct = default)
      {
            var data = await context.InputPointSpecifications.AddAsync(config);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  return new InputPointSpecification();

            return data.Entity;
      }


}
