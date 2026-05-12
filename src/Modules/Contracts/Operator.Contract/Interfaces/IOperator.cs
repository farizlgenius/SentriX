using System;
using Operator.Contract.DTOs;
using SharedKernel.Domain;

namespace Operator.Contract.Interfaces;

public interface IOperator
{
      Task<Pagination<OperatorDto>> GetPagination(PaginationParams param);
      Task<OperatorDto> CreateAsync(CreateOperatorDto dto);
      Task<OperatorDto> UpdateAsync(UpdateOperatorDto dto);
      Task<OperatorDto> DeleteByIdAsync(int id);
      Task<bool> IsOperatorExistsAsync(string Username);
      Task<string> GetHashedPasswordAsync(string Username);
      Task<OperatorDto> GetOperatorAsync(string Username);
      Task<PasswordRuleDto> GetPassowrdRuleAsync();
       Task<PasswordRuleDto> CreatePasswordRuleAsync(PasswordRuleDto dto);
}
