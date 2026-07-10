using Adapter.Amico.Persistences.Entities;

namespace Adapter.Amico.Interfaces;

public interface IAmicoRepository
{
      Task<Amicos> GetAmicoByMacAsync(string mac,CancellationToken ct = default);
      Task UpdateSessionByMacAsync(string mac,string session,CancellationToken ct = default);
}