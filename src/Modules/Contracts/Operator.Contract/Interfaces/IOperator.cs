using System;
using Operator.Contract.DTOs;
using SharedKernel.Domain;

namespace Operator.Contract.Interfaces;

public interface IOperator
{
      Task<Pagination<OperatorDto>> GetPaginationWithLocationIdAsync(int location, int Page, int PageSize,string Search);
      Task<OperatorDto> CreateAsync(CreateOperatorDto dto);
      Task<OperatorDto> UpdateAsync(UpdateOperatorDto dto);
      Task<OperatorDto> DeleteByIdAsync(int id);
      Task<bool> IsOperatorExistsAsync(string Username);
      Task<string> GetHashedPasswordAsync(string Username);
      Task<OperatorDto> GetOperatorAsync(string Username);
}
