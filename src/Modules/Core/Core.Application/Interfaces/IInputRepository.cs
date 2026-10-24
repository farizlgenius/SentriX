using Core.Contract.DTOs.Input;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IInputRepository : IRepository<InputDto, Input>
{
      Task<short> GetFreeExeternalIdByMacAsync(string mac,int max, IEnumerable<short>? exception = null, CancellationToken ct = default);
      
}