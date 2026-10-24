using Core.Contract.DTOs.Output;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IOutputRepository : IRepository<OutputDto, Output>
{
      Task<short> GetFreeExeternalIdByMacAsync(string mac,int max, IEnumerable<short>? exception = null, CancellationToken ct = default);
}