using Core.Contract.DTOs.Operator;

namespace Core.Contract.Interfaces;

public interface IOperator : IBase<OperatorDto, CreateOperatorDto, UpdateOperatorDto>
{
  Task<bool> ChangePasswordAsync(ChangePasswordDto dto, CancellationToken ct = default);
}