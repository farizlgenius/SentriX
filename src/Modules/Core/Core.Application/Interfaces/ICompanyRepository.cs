using Core.Contract.DTOs.Company;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface ICompanyRepository : IBaseRepository<CompanyDto, Company>
{

}