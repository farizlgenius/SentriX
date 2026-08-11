using Core.Application.Interfaces;
using Core.Contract.DTOs.Position;
using Core.Contract.Interfaces;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class PositionService(
      IPositionRepository dep,
      IDepartmentRepository com
      ) : IPosition
{
      public async Task<PositionDto> CreateAsync(CreatePositionDto dto, CancellationToken ct = default)
      {
            var d = new Core.Domain.Entities.Position(
                  dto.Name,
                  dto.Description,
                  dto.DepartmentGuid
            );

            // Check Company Exists
            if (!await com.IsAnyGuidAsync(dto.DepartmentGuid))
                  throw new NotFoundException(EntityType.Department, dto.DepartmentGuid.ToString());

            // Check name is duplicate 
            if (await dep.IsAnyNameByDepartmentGuidAsync(dto.Name, dto.DepartmentGuid))
                  throw new DuplicateException(EntityType.Position, dto.Name);

            await dep.AddAsync(d, ct);

            return new PositionDto(
              d.Guid,
              d.Name,
              d.Description,
              d.DepartmentGuid,
              true,
              false
            );
      }

      public async Task<Guid> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await dep.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.Company, guid.ToString());

            // Check is default location
            if (await dep.IsDefaultAsync(guid, ct))
                  throw new DefaultRecordException(MethodType.Delete, EntityType.Company, guid.ToString());

            // Check relate object here

            if (await dep.IsAnyUserAsync(guid, ct))
                  throw new FoundRelateException(EntityType.Company, guid.ToString(), EntityType.User);


            await dep.DeleteAsync(guid, ct);

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
                  if (!await dep.IsAnyGuidAsync(guid, ct))
                        throw new NotFoundException(EntityType.Company, guid.ToString());

                  // Check relate object here

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

      public async Task<PositionDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await dep.GetAsync(guid, ct);
      }

      public async Task<Pagination<PositionDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await dep.GetPaginationAsync(param, ct);
      }

      public async Task<Pagination<PositionDto>> GetPaginationByDepartmentGuidAsync(PaginationParams param, Guid companyGuid, CancellationToken ct = default)
      {
            return await dep.GetPaginationByDepartmentGuidAsync(param, companyGuid);
      }

      public async Task<PositionDto> UpdateAsync(UpdatePositionDto dto, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await dep.IsAnyGuidAsync(dto.Guid, ct))
                  throw new NotFoundException(EntityType.Location, dto.Guid.ToString());

            // Check Company Exists
            if (!await com.IsAnyGuidAsync(dto.DepartmentGuid))
                  throw new NotFoundException(EntityType.Department, dto.DepartmentGuid.ToString());

            var d = new Core.Domain.Entities.Position(
              dto.Guid,
              dto.Name,
              dto.Description,
              dto.DepartmentGuid
            );

            await dep.UpdateAsync(d);

            return new PositionDto(
              dto.Guid,
              dto.Name,
              dto.Description,
              dto.DepartmentGuid,
              true,
              false
            );
      }
}