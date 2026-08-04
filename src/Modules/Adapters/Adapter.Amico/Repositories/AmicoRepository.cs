using Adapter.Amico.Interfaces;
using Adapter.Amico.Persistences;
using Adapter.Amico.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Adapter.Amico.Repositories;

public sealed class AmicoRepository(AmicoDbContext context) : IAmicoRepository
{
      public async Task AddAsync(
            Guid guid,
            string mac,
            string ip,
            string session,
            CancellationToken ct = default)
      {
            await context.Amicos.AddAsync(
                  new Persistences.Entities.Amicos(
                        guid,
                        mac,
                        ip,
                        session
                        ),
                        ct
            );

            await context.SaveChangesAsync(ct);
      }

      public async Task AddSlotAsync<TEntity>(Guid guid, int slot, Func<Guid, int, TEntity> factory, CancellationToken ct = default) where TEntity : BaseSlot
      {
            var entity = factory(guid, slot);

            await context.Set<TEntity>().AddAsync(entity, ct);
            await context.SaveChangesAsync(ct);
      }

      public async Task AddSlotAsync<TEntity>(Guid guid, Guid deviceGuid, int slot, Func<Guid, Guid, int, TEntity> factory, CancellationToken ct = default) where TEntity : BaseSlot
      {
            var entity = factory(guid, deviceGuid, slot);

            await context.Set<TEntity>().AddAsync(entity, ct);
            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(string Mac, string Ip, CancellationToken ct = default)
      {
            var entity = await context.Amicos
            .Where(x => x.mac.Equals(Mac))
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync();

            if (entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            context.Amicos.Remove(entity);

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteSlot<TEntity>(Guid guid, int slot, CancellationToken ct = default) where TEntity : BaseSlot
      {
            var e = await context.Set<TEntity>()
            .Where(x => x.guid == guid && x.slot_id == slot)
            .FirstOrDefaultAsync(ct);

            if (e is null)
                  throw new Exception(MessageHelper.Common.NotFound("Slot Id", slot));

            context.Set<TEntity>().Remove(e);
            await context.SaveChangesAsync(ct);
      }

      public async Task<Amicos> GetAmicoByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Amicos.AsNoTracking()
            .Where(x => x.guid == guid)
            .FirstOrDefaultAsync() ?? throw new Exception(MessageHelper.DB.RecordNotFounds(guid.ToString()));
      }

      public async Task<Amicos> GetAmicoByMacAsync(string mac, CancellationToken ct = default)
      {
            return await context.Amicos
            .Where(x => mac.Equals(mac))
            .FirstOrDefaultAsync() ?? throw new Exception(MessageHelper.DB.RecordNotFounds(mac));
      }

      public async Task<Amicos> GetAmicoByIpAsync(string ip, CancellationToken ct = default)
      {
            return await context.Amicos
            .Where(x => ip.Equals(x.ip))
            .FirstOrDefaultAsync() ?? throw new Exception(MessageHelper.DB.RecordNotFounds(ip));
      }

      public async Task<TEntity> GetSlotByGuidAsync<TEntity>(Guid guid, CancellationToken ct = default) where TEntity : BaseSlot
      {
            return await context.Set<TEntity>()
            .Where(x => x.guid == guid)
            .FirstOrDefaultAsync() ?? throw new Exception(MessageHelper.DB.RecordNotFounds(guid.ToString()));
      }

      public async Task<int> GetSlotIdByGuidAsync<TEntity>(Guid Guid, CancellationToken ct = default) where TEntity : BaseSlot
      {
           var res= await context.Set<TEntity>()
            .Where(x => x.guid == Guid)
            .Select(x => x.slot_id)
            .DefaultIfEmpty(-1)
            .FirstAsync();

            if(res == -1)
                  throw new Exception(MessageHelper.DB.RecordNotFounds(Guid.ToString()));

            return res;
      }

      public async Task UpdateSessionByMacAsync(string mac, string session, CancellationToken ct = default)
      {
            var entity = await context.Amicos
            .Where(x => x.mac.Equals(mac))
            .FirstOrDefaultAsync();

            if (entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.UpdateSession(session);

            context.Amicos.Update(entity);
            await context.SaveChangesAsync(ct);
      }

      public async Task UpdateSessionByIpAsync(string ip, string session, CancellationToken ct = default)
      {
             var entity = await context.Amicos
            .Where(x => x.ip.Equals(ip))
            .FirstOrDefaultAsync();

            if (entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.UpdateSession(session);

            context.Amicos.Update(entity);
            await context.SaveChangesAsync(ct);
      }

      public async Task<IEnumerable<int>> GetSlotIdsByGuidAsync<TEntity>(Guid Guid, CancellationToken ct = default) where TEntity : BaseSlot
      {
            var res= await context.Set<TEntity>()
            .Where(x => x.guid == Guid)
            .Select(x => x.slot_id)
            .ToArrayAsync();

            return res;
      }
}