using Adapter.Amico.Persistences.Entities;

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
      Task<Amicos> GetAmicoByMacAsync(string mac,CancellationToken ct = default);
      Task UpdateSessionByMacAsync(string mac,string session,CancellationToken ct = default);
}