using Core.Application.Interfaces;
using Core.Contract.DTOs.Company;
using Core.Contract.Interfaces;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class CompanyService(
  ICompanyRepository repo,
  ILocationRepository loc
  ) : ICompany
{
  public async Task<CompanyDto> CreateAsync(CreateCompanyDto dto, CancellationToken ct = default)
  {
    if(!await loc.IsAnyGuidAsync(dto.LocationGuid))
      throw new NotFoundException("Location",dto.LocationGuid.ToString());

    var locationId = await loc.GetIdByGuidAsync(dto.LocationGuid,ct);

    var d = new Core.Domain.Entities.Company(
      dto.Name,
      dto.Description,
      dto.Address,
      locationId
    );

    // Check name is duplicate 
    if (await repo.IsAnyByNameAndLocationGuidAsync(dto.Name))
      throw new DuplicateException(EntityType.Company, dto.Name);

    

    await repo.AddAsync(d, ct);

    return new CompanyDto(
      d.Guid,
      d.Name,
      d.Address,
      d.Description,
      dto.LocationGuid,
      true,
      false
    );


  }

  public async Task<Guid> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Company, guid.ToString());

    // Check is default location
    if (await repo.IsDefaultAsync(guid, ct))
      throw new DefaultRecordException(MethodType.Delete, EntityType.Company, guid.ToString());

    // Check relate object here
    if (await repo.IsAnyDepartmentAsync(guid, ct))
      throw new FoundRelateException(EntityType.Company, guid.ToString(), EntityType.Department);

    if (await repo.IsAnyUserAsync(guid, ct))
      throw new FoundRelateException(EntityType.Company, guid.ToString(), EntityType.User);


    await repo.DeleteAsync(guid, ct);

    return guid;
  }

  public async Task<IEnumerable<Guid>> DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    // Check if guids is empty 
    if (guids.Count() == 0)
      throw new NotFoundException(EntityType.Company);

    foreach (var guid in guids)
    {
      // Check is any location with guid
      if (!await repo.IsAnyGuidAsync(guid, ct))
        throw new NotFoundException(EntityType.Company, guid.ToString());

      // Check relate object here
      if (await repo.IsAnyDepartmentAsync(guid, ct))
        throw new FoundRelateException(EntityType.Company, guid.ToString(), EntityType.Department);

      if (await repo.IsAnyUserAsync(guid, ct))
        throw new FoundRelateException(EntityType.Company, guid.ToString(), EntityType.User);
    }

    await repo.DeleteRangeAsync(guids);

    return guids;
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Company, guid.ToString());

    return await repo.DisableAsync(guid, ct);
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Company, guid.ToString());

    return await repo.EnableAsync(guid, ct);
  }

  public async Task<CompanyDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetAsync(guid, ct);
  }

  public async Task<Pagination<CompanyDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<CompanyDto> UpdateAsync(UpdateCompanyDto dto, CancellationToken ct = default)
  {
    // Check is any Company with guid
    if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
      throw new NotFoundException(EntityType.Company, dto.Guid.ToString());

    var locationId = await loc.GetIdByGuidAsync(dto.LocationGuid,ct);

    var d = new Core.Domain.Entities.Company(
      dto.Guid,
      dto.Name,
      dto.Description,
      dto.Address,
      locationId
    );

    await repo.UpdateAsync(d);

    return new CompanyDto(
      dto.Guid,
      dto.Name,
      dto.Description,
      dto.Address,
      dto.LocationGuid,
      true,
      false
    );
  }
}