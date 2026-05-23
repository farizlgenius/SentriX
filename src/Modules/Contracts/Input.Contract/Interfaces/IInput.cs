using Input.Contract.DTOs;
using SharedKernel.Domain;

namespace Input.Contract.Interfaces;

public interface IInput
{
      Task<Pagination<InputDto>> GetInputPagination(PaginationParams param);
      Task<InputDto> CreateInputAsync(CreateInputDto dto);
      Task<InputDto> UpdateInputAsync(InputDto dto);
      Task<InputDto> DeleteInputAsync(int id);
      Task<BaseResponse> InputMaskAsync(int id,bool IsMask); 
      Task<InputGroupDto> CreateInputGroupAsync(CreateInputGroupDto dto);
      Task<InputGroupDto> UpdateInputGroupAsync(InputGroupDto dto);
      Task<InputGroupDto> DeleteInputGroupAsync(int id);
      Task<Pagination<InputGroupDto>> GetGroupPaginationAsync(PaginationParams param);
}