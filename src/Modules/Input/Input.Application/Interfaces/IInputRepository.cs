using Input.Contract.DTOs;
using Input.Domain.Entities;
using SharedKernel.Domain;

namespace Input.Application.Interfaces;

public interface IInputRepository
{
      Task<Pagination<InputDto>> GetInputPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<short> GetLowestInputComponentIdAsync(string Mac,CancellationToken ct = default);
      Task<short> GetLowestInputGroupComponentIdAsync(string Mac,CancellationToken ct = default);
      Task<InputDto> CreateInputAsync(Inputs domain,CancellationToken ct = default);
      Task<InputDto> DeleteInputAsync(int id,CancellationToken ct = default);
      Task<InputDto> UpdateInputAsync(Inputs domain,CancellationToken ct = default);
      Task<InputDto> GetByIdAsync(int id,CancellationToken ct = default);
      Task<InputGroupDto> CreateInputGroupAsync(InputGroups domain,CancellationToken ct = default);
      Task<InputGroupDto> UpdateInputGroupAsync(InputGroups domain,CancellationToken ct = default);
      Task<InputGroupDto> DeleteInputGroupAsync(int id,CancellationToken ct = default);
      Task<InputGroupDto> GetGroupByIdAsync(int id,CancellationToken ct = default);
      Task<Pagination<InputGroupDto>> GetInputGroupPaginationAsync(PaginationParams param,CancellationToken ct = default);
      
}