using SharedKernel.Domain;
using User.Contract.DTOs;

namespace User.Contract.Interfaces;

public interface IUser
{
      Task<Pagination<UserDto>> GetUserPaginationAsync(PaginationParams param);
      Task<UserDto> CreateUserAsync(CreateUserDto dto);
      Task<UserDto> UpdateUserAsync(UserDto dto);
      Task<UserDto> DeleteUserAsync(Guid guid);
      Task UploadImageAsync(string userid, Stream stream);
      Task<Stream> GetImageByUserIdAsync(string userid);

      // Company
      Task<Pagination<CompanyDto>> GetCompanyPaginationAsync(PaginationParams param);
      Task<CompanyDto> DeleteCompanyAsync(Guid guid);
      Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto);
      Task<CompanyDto> UpdateCompanyAsync(CompanyDto dto);
      Task<IEnumerable<CompanyDto>> GetCompanyByLocationIdAsync(int LocationId);
      Task<IEnumerable<OptionDto>> GetCompanyOptionByLocationIdAsync(int LocationId);

      // Department
      Task<Pagination<DepartmentDto>> GetDepartmentPaginationAsync(PaginationParams param);
      Task<Pagination<DepartmentDto>> GetDepartmentPaginationByCompanyGuidAsync(PaginationParams param,Guid comapnyGuid);
      Task<IEnumerable<DepartmentDto>> GetDepartmentByCompanyGuidAsync(Guid companyGuid);
      Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto);
      Task<DepartmentDto> UpdateDepartmentAsync(DepartmentDto dto);
      Task<IEnumerable<OptionDto>> GetDepartmentOptionByCompanyGuidAsync(Guid guid);
      Task<DepartmentDto> DeleteDepartmentAsync(Guid guid);
      // Position
      Task<Pagination<PositionDto>> GetPositionPaginationAsync(PaginationParams param);
      Task<Pagination<PositionDto>> GetPositionPaginationByDepartmentGuidAsync(PaginationParams param,Guid guid);
      Task<PositionDto> CreatePositionAsync(CreatePositionDto dto);
      Task<PositionDto> UpdatePositionAsync(PositionDto dto);
      Task<IEnumerable<OptionDto>> GetPositionOptionByDepartmentGuidAsync(Guid guid);
      Task<PositionDto> DeletePositionAsync(Guid guid);

      Task<IEnumerable<OptionDto>> GetUserFlagOptionAsync();
      
}