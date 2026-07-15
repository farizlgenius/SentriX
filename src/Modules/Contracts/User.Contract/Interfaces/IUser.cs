using SharedKernel.Domain;
using User.Contract.DTOs;

namespace User.Contract.Interfaces;

public interface IUser
{
      Task<Pagination<UserDto>> GetUserPaginationAsync(PaginationParams param);
      Task<UserDto> CreateUserAsync(CreateUserDto dto);
      Task<UserDto> UpdateUserAsync(UserDto dto);
      Task<UserDto> DeleteUserAsync(int id);
      Task UploadImageAsync(string userid, Stream stream);
      Task<Stream> GetImageByUserIdAsync(string userid);

      // Company
      Task<Pagination<CompanyDto>> GetCompanyPaginationAsync(PaginationParams param);
      Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto);
      Task<CompanyDto> UpdateCompanyAsync(CompanyDto dto);
      Task<CompanyDto> DeleteCompanyAsync(int id);
      Task<IEnumerable<CompanyDto>> GetCompanyByLocationIdAsync(int LocationId);
      Task<IEnumerable<OptionDto>> GetCompanyOptionByLocationAsync(int LocationId);

      // Department
      Task<Pagination<DepartmentDto>> GetDepartmentPaginationAsync(PaginationParams param);
      Task<Pagination<DepartmentDto>> GetDepartmentByCompanyAsync(PaginationParams param,int CompanyId);
      Task<IEnumerable<DepartmentDto>> GetDepartmentByCompanyAsync(int CompanyId);
      Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto);
      Task<DepartmentDto> UpdateDepartmentAsync(DepartmentDto dto);
      Task<DepartmentDto> DeleteDepartmentAsync(int id);
      Task<IEnumerable<OptionDto>> GetDepartmentOptionByCompanyAsync(int CompanyId);
      // Position
      Task<Pagination<PositionDto>> GetPositionPaginationAsync(PaginationParams param);
      Task<Pagination<PositionDto>> GetPositionByDepartmentAsync(PaginationParams param,int DepartmentId);
      Task<PositionDto> CreatePositionAsync(CreatePositionDto dto);
      Task<PositionDto> UpdatePositionAsync(PositionDto dto);
      Task<PositionDto> DeletePositionAsync(int id);
      Task<IEnumerable<OptionDto>> GetPositionOptionByDepartmentAsync(int CompanyId);

      Task<IEnumerable<OptionDto>> GetUserFlagOptionAsync();
      
}