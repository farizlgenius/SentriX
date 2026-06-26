using SharedKernel.Domain;
using User.Contract.DTOs;
using User.Domain.Entities;

namespace User.Application.Interfaces;

public interface IUserRepository
{
      Task<CompanyDto> CreateCompanyAsync(Company dto,CancellationToken ct = default);
      Task<bool> IsCompanyNameExistAsync(string name, CancellationToken ct = default);
      Task<bool> IsDepartmentExistAsync(string name, CancellationToken ct = default);
      Task<bool> IsPositionExistAsync(string name, CancellationToken ct = default);
      Task<bool> IsAnyCompanyByIdAsync(int id,CancellationToken ct = default);
      Task<bool> IsAnyDepartmentByIdAsync(int id,CancellationToken ct = default);
      Task<bool> IsAnyPositionByIdAsync(int id,CancellationToken ct = default);
      Task<bool> IsAnyUserByIdAsync(int id,CancellationToken ct = default);
      Task<bool> IsAnyUserByUserIdAsync(string userid,CancellationToken ct = default);
      Task<string> CheckCompanyRelateRecordAsync(int id,CancellationToken ct = default);
      Task<string> CheckDepartmentRelateRecordAsync(int id,CancellationToken ct = default);
      Task<string> CheckPositionRelateRecordAsync(int id,CancellationToken ct = default);
      Task UpdateImagePathAsync(string path,string userid,CancellationToken ct = default);
      Task<DepartmentDto> CreateDepartmentAsync(Department dto,CancellationToken ct = default);
      Task<PositionDto> CreatePositionAsync(Position dto,CancellationToken ct = default);
      Task<UserDto> CreateUserAsync(Users dto,CancellationToken ct = default);
      Task<CompanyDto> DeleteCompanyAsync(int id, CancellationToken ct = default);
      Task<DepartmentDto> DeleteDepartmentAsync(int id,CancellationToken ct = default);
      Task<PositionDto> DeletePositionAsync(int id,CancellationToken ct = default);
      Task<UserDto> DeleteUserAsync(int id,CancellationToken ct = default);
      Task<Pagination<CompanyDto>> GetCompanyPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<Pagination<DepartmentDto>> GetDepartmentPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<Pagination<PositionDto>> GetPositionPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<Pagination<UserDto>> GetUserPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<CompanyDto> UpdateCompanyAsync(Domain.Entities.Company company,CancellationToken ct = default);
      Task<DepartmentDto> UpdateDepartmentAsync(Domain.Entities.Department department,CancellationToken ct = default);
      Task<PositionDto> UpdatePositionAsync(Domain.Entities.Position position,CancellationToken ct = default);
      Task<UserDto> UpdateUserAsync(Domain.Entities.Users user,CancellationToken ct = default);
      Task<IEnumerable<CompanyDto>> GetCompanyByLocationIdAsync(int LocationId,CancellationToken ct = default);
      Task<Pagination<DepartmentDto>> GetDepartmentByCompanyAsync(PaginationParams param,int companyId,CancellationToken ct = default);
      Task<IEnumerable<DepartmentDto>> GetDepartmentByCompanyAsync(int companyId,CancellationToken ct = default);
      Task<Pagination<PositionDto>> GetPositionByDepartmentAsync(PaginationParams param,int departmentId,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetCompanyOptionByLocationAsync(int locationId,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetDepartmentOptionByCompanyAsync(int CompanyId,CancellationToken ct = default);
       Task<IEnumerable<OptionDto>> GetPositionOptionByDepartmentAsync(int DepartmentId,CancellationToken ct = default);
       Task<IEnumerable<OptionDto>> GetUserFlagOptionAsync(CancellationToken ct = default);
}