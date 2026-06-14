using SharedKernel.Domain;
using User.Contract.DTOs;

namespace User.Contract.Interfaces;

public interface IUser
{
      Task<Pagination<UserDto>> GetUserPaginationAsync(PaginationParams param);
      Task<UserDto> CreateUserAsync(CreateUserDto dto);
      Task<UserDto> UpdateUserAsync(UserDto dto);
      Task<UserDto> DeleteUserAsync(int id);
      Task<BaseResponse> UploadImageAsync(string userid, Stream stream);
      Task<Stream> GetImageByUserIdAsync(string userid);

      // Company
      Task<Pagination<CompanyDto>> GetCompanyPaginationAsync(PaginationParams param);
      Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto);
      Task<CompanyDto> UpdateCompanyAsync(CompanyDto dto);
      Task<CompanyDto> DeleteCompanyAsync(int id);

      // Department
      Task<Pagination<DepartmentDto>> GetDepartmentPaginationAsync(PaginationParams param);
      Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto);
      Task<DepartmentDto> UpdateDepartmentAsync(DepartmentDto dto);
      Task<DepartmentDto> DeleteDepartmentAsync(int id);
      // Position
      Task<Pagination<PositionDto>> GetPositionPaginationAsync(PaginationParams param);
      Task<PositionDto> CreatePositionAsync(CreatePositionDto dto);
      Task<PositionDto> UpdatePositionAsync(PositionDto dto);
      Task<PositionDto> DeletePositionAsync(int id);
}