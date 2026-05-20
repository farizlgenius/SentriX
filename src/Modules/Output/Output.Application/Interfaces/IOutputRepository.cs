using Output.Contract.DTOs;
using Output.Domain.Entities;
using SharedKernel.Domain;

namespace Output.Application.Interfaces;

public interface IOutputRepository
{
      Task<Pagination<OutputDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<OutputDto> CreateAsync(Outputs dto,CancellationToken ct = default);
      Task<IEnumerable<short>> GetUnavailableOutputByModuleIdAsync(int moduleId,CancellationToken ct = default);
      Task<bool> IsAnyWithIdAsync(int Id,CancellationToken ct = default);
      Task<OutputDto> GetByIdAsync(int id,CancellationToken ct = default);


}