using Core.Contract.DTOs.Operator;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IOperator : IBase<OperatorDto, CreateOperatorDto, UpdateOperatorDto>
{
      Task<string> GetHashedPasswordByUsernameAsync(string username,CancellationToken ct = default);
      Task<OperatorDto> GetOperatorByUsernameAsync(string username,CancellationToken ct = default);
}