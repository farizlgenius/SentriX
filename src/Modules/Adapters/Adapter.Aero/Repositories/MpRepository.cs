using System;
using Adapter.Aero.Persistences;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Aero.Repositories;

public sealed class MpRepository(AeroDbContext context) : IMpRepository
{
      public async Task<InputPointSpecification> GetInputPointSpecificationByIdAndMacAndSioNumber(short ScpId, string Mac, short SioNumber)
      {
            return await context.InputPointSpecifications.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.scp_id == ScpId && x.mac.Equals(Mac) && x.sio_number == SioNumber)
            .FirstOrDefaultAsync() ?? new InputPointSpecification();
      }
}
