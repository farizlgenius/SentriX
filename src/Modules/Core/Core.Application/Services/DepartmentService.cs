using Core.Application.Interfaces;
using Core.Contract.DTOs.Department;
using Core.Contract.Interfaces;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class DepartmentService(
      IDepartmentRepository dep,
      ICompanyRepository com
      ) : IDepartment
{
      public async Task<Guid> CreateAsync(CreateDepartmentDto dto, CancellationToken ct = default)
      {
            // Check Company Exists
            if(!await com.IsAnyGuidAsync(dto.CompanyGuid))
                  throw new NotFoundException(EntityType.Company,dto.CompanyGuid.ToString());

            // Check name is duplicate 
            if (await dep.IsAnyNameByCompanyGuidAsync(dto.Name,dto.CompanyGuid))
                  throw new DuplicateException(EntityType.Department, dto.Name);

            var companyId = await com.GetIdByGuidAsync(dto.CompanyGuid,ct);


            var d = new Core.Domain.Entities.Department(
                  dto.Name,
                  dto.Description,
                  companyId
            );

            
            await dep.AddAsync(d, ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await dep.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.Company, guid.ToString());

            // Check is default location
            if (await dep.IsDefaultAsync(guid, ct))
                  throw new DefaultRecordException(MethodType.Delete, EntityType.Company, guid.ToString());

            // Check relate object here
            if (await dep.IsAnyPositionAsync(guid, ct))
                  throw new FoundRelateException(EntityType.Company, guid.ToString(), EntityType.Department);

            if (await dep.IsAnyUserAsync(guid, ct))
                  throw new FoundRelateException(EntityType.Company, guid.ToString(), EntityType.User);


            await dep.DeleteAsync(guid, ct);

            return true;
      }

      public async Task<IEnumerable<Guid>> DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            // Check if guids is empty 
            if (guids.Count() == 0)
                  throw new NotFoundException(EntityType.Company);

            foreach (var guid in guids)
            {
                  // Check is any location with guid
                  if (!await dep.IsAnyGuidAsync(guid, ct))
                        throw new NotFoundException(EntityType.Company, guid.ToString());

                  // Check relate object here
                  if (await dep.IsAnyPositionAsync(guid, ct))
                        throw new FoundRelateException(EntityType.Company, guid.ToString(), EntityType.Department);

                  if (await dep.IsAnyUserAsync(guid, ct))
                        throw new FoundRelateException(EntityType.Company, guid.ToString(), EntityType.User);
            }

            await dep.DeleteRangeAsync(guids);

            return guids;
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await dep.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.Location, guid.ToString());

            return await dep.DisableAsync(guid, ct);
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await dep.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.Location, guid.ToString());

            return await dep.EnableAsync(guid, ct);
      }

      public async Task<DepartmentDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await dep.GetAsync(guid, ct);
      }

      public async Task<Pagination<DepartmentDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await dep.GetPaginationAsync(param, ct);
      }

      public async Task<Pagination<DepartmentDto>> GetPaginationByCompanyGuidAsync(PaginationParams param,Guid companyGuid, CancellationToken ct = default)
      {
            return await dep.GetPaginationByCompanyGuidAsync(param,companyGuid);
      }

      public async Task<Guid> UpdateAsync(UpdateDepartmentDto dto, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await dep.IsAnyGuidAsync(dto.Guid, ct))
                  throw new NotFoundException(EntityType.Location, dto.Guid.ToString());

            // Check Company Exists
            if(!await com.IsAnyGuidAsync(dto.CompanyGuid))
                  throw new NotFoundException(EntityType.Company,dto.CompanyGuid.ToString());

            var companyId = await com.GetIdByGuidAsync(dto.CompanyGuid,ct);

            var d = new Core.Domain.Entities.Department(
              dto.Guid,
              dto.Name,
              dto.Description,
              companyId
            );

            await dep.UpdateAsync(d);

            return d.Guid;
      }
}