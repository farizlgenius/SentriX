using Core.Application.Interfaces;
using Core.Contract.DTOs.Operator;
using Core.Contract.Interfaces;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class OperatorService(IOperatorRepository repo) : IOperator
{
      public async Task<Guid> CreateAsync(CreateOperatorDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<OperatorDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<OperatorDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<string> GetHashedPasswordByUsernameAsync(string username, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<OperatorDto> GetOperatorByUsernameAsync(string username, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Pagination<OperatorDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Guid> UpdateAsync(UpdateOperatorDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}