using Adapter.Amico.Persistences.Entities;
using SharedKernel.Domain;

namespace Adapter.Amico.Interfaces;

public interface IAmicoRepository
{
      Task AddAsync(
             Guid guid,
            string mac,
            string ip,
            string session,
            CancellationToken ct = default
      );

      Task DeleteAsync(
            string Mac,
            string Ip,
            CancellationToken ct = default
      );

      Task<int> GetSlotIdByGuidAsync<TEntity>(Guid Guid, CancellationToken ct = default) where TEntity : BaseSlot;
      Task<IEnumerable<int>> GetSlotIdsByGuidAsync<TEntity>(Guid Guid, CancellationToken ct = default) where TEntity : BaseSlot;

      Task<TEntity> GetSlotByGuidAsync<TEntity>(Guid guid, CancellationToken ct = default) where TEntity : BaseSlot;

      Task<Amicos> GetAmicoByMacAsync(string mac, CancellationToken ct = default);
      Task<Amicos> GetAmicoByIpAsync(string ip, CancellationToken ct = default);
      Task UpdateSessionByMacAsync(string mac, string session, CancellationToken ct = default);
      Task UpdateSessionByIpAsync(string ip, string session, CancellationToken ct = default);

      // Slot Command
      Task AddSlotAsync<TEntity>(
             Guid guid,
            int slot,
             Func<Guid, int, TEntity> factory,
            CancellationToken ct = default
      ) where TEntity : BaseSlot;

      Task AddSlotAsync<TEntity>(
             Guid guid,
             Guid deviceGuid,
            int slot,
             Func<Guid, Guid, int, TEntity> factory,
            CancellationToken ct = default
      ) where TEntity : BaseSlot;


      Task DeleteSlot<TEntity>(
            Guid guid,
            int slot,
            CancellationToken ct = default
      ) where TEntity : BaseSlot;

      Task<Amicos> GetAmicoByGuidAsync(Guid guid, CancellationToken ct = default);
}