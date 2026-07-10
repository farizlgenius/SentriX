using Adapter.Amico.Interfaces;
using Adapter.Amico.Persistences;
using Adapter.Amico.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Helpers;

namespace Adapter.Amico.Repositories;

public sealed class AmicoRepository(AmicoDbContext context) : IAmicoRepository
{
      public async Task<Amicos> GetAmicoByMacAsync(string mac,CancellationToken ct = default)
      {
            return await context.Amicos
            .Where(x => mac.Equals(mac))
            .FirstOrDefaultAsync() ?? new Amicos();
      }

      public async Task UpdateSessionByMacAsync(string mac, string session, CancellationToken ct = default)
      {
            var entity = await context.Amicos
            .Where(x => x.mac.Equals(mac))
            .FirstOrDefaultAsync();

            if(entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.UpdateSession(session);

            context.Amicos.Update(entity);
            await context.SaveChangesAsync(ct);
      }
}