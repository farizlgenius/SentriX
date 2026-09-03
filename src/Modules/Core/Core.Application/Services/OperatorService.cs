using Core.Application.Interfaces;
using Core.Contract.DTOs.Operator;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;
using Storage.Contract.Interfaces;

namespace Core.Application.Services;

public sealed class OperatorService(
      IOperatorRepository repo,
      IStorage file,
      IMessageBus bus) : IOperator
{
      public async Task<Guid> CreateAsync(CreateOperatorDto dto, CancellationToken ct = default)
      {
            if (await repo.IsAnyUsernameAsync(dto.Username, ct))
                  throw new BadRequestException(EntityType.Operator, "Username already exists.");

            if (await repo.IsAnyEmailAsync(dto.Email, ct))
                  throw new BadRequestException(EntityType.Operator, "Email already exists.");

            if (!await bus.QueryAsync(new IsValidRoleByGuidQuery(dto.RoleGuid)))
                  throw new BadRequestException(EntityType.Operator, $"Role Guid {dto.RoleGuid} is not valid.");

            var invalidLoc = await bus.QueryAsync(new IsAnyInvalidLocationsByGuidsQuery(dto.LocationGuids));
            if (invalidLoc.Any())
            {
                  var invalidLocStr = string.Join(", ", invalidLoc);
                  throw new BadRequestException(EntityType.Operator, $"Location Guid(s) {invalidLocStr} is/are not valid.");
            }

            var roleId = await bus.QueryAsync(new RoleIdByGuidQuery(dto.RoleGuid), ct);
            var locationIds = await bus.QueryAsync(new LocationIdsByGuidsQuery(dto.LocationGuids), ct);


            var d = new Domain.Entities.Operator(
                  dto.Username,
                  dto.Password,
                  dto.Title,
                  dto.Firstname,
                  dto.Middlename,
                  dto.Lastname,
                  dto.Gender,
                  dto.Email,
                  dto.Phone,
                  dto.JoinedDate,
                  dto.ExpiredDate,
                  roleId,
                  locationIds.ToList()
            );

            await repo.AddAsync(d, ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            if (!await repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.Operator, guid.ToString());

            await repo.DeleteAsync(guid, ct);

            return true;
      }

      public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            if (guids.Count() == 0)
                  throw new BadRequestException(EntityType.Operator, "Guid list is empty.");

            foreach (var guid in guids)
            {
                  if (!await repo.IsAnyGuidAsync(guid, ct))
                        throw new NotFoundException(EntityType.Operator, guid.ToString());
            }

            await repo.DeleteRangeAsync(guids, ct);

            return guids;
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            if (!await repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.Operator, guid.ToString());

            await repo.DisableAsync(guid, ct);

            return true;
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            if (!await repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.Operator, guid.ToString());

            await repo.EnableAsync(guid, ct);

            return true;
      }

      public async Task<OperatorDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            if (!await repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.Operator, guid.ToString());

            return await repo.GetAsync(guid, ct);
      }


      public async Task<IEnumerable<OperatorDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(guid));
            return await repo.GetByLocationAsync(locationId, ct);
      }

      public async Task<string> GetHashedPasswordByUsernameAsync(string username, CancellationToken ct = default)
      {
            return await repo.GetPassowrdByUsernameAsync(username, ct);
      }

      public async Task<OperatorDto> GetOperatorByUsernameAsync(string username, CancellationToken ct = default)
      {
            return await repo.GetOperatorByUsernameAsync(username, ct);
      }

      public async Task<Pagination<OperatorDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await repo.GetPaginationAsync(param, ct);
      }

      public async Task<Guid> UpdateAsync(UpdateOperatorDto dto, CancellationToken ct = default)
      {
            if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
                  throw new NotFoundException(EntityType.Operator, dto.Guid.ToString());

            if (!await repo.IsAnyUsernameAsync(dto.Username, ct))
                  throw new BadRequestException(EntityType.Operator, "Username already exists.");

            if (!await repo.IsAnyEmailAsync(dto.Email, ct))
                  throw new BadRequestException(EntityType.Operator, "Email already exists.");

            if (!await bus.QueryAsync(new IsValidRoleByGuidQuery(dto.RoleGuid)))
                  throw new BadRequestException(EntityType.Operator, $"Role Guid {dto.RoleGuid} is not valid.");

            var invalidLoc = await bus.QueryAsync(new IsAnyInvalidLocationsByGuidsQuery(dto.LocationGuids));
            if (invalidLoc.Any())
            {
                  var invalidLocStr = string.Join(", ", invalidLoc);
                  throw new BadRequestException(EntityType.Operator, $"Location Guid(s) {invalidLocStr} is/are not valid.");
            }

            var roleId = await bus.QueryAsync(new RoleIdByGuidQuery(dto.RoleGuid), ct);
            var locationIds = await bus.QueryAsync(new LocationIdsByGuidsQuery(dto.LocationGuids), ct);

            var d = new Domain.Entities.Operator(
                  dto.Guid,
                  dto.Username,
                  dto.Title,
                  dto.Firstname,
                  dto.Middlename,
                  dto.Lastname,
                  dto.Gender,
                  dto.Email,
                  dto.Phone,
                  dto.JoinedDate,
                  dto.ExpiredDate,
                  roleId,
                  locationIds.ToList()
            );

            await repo.UpdateAsync(d, ct);

            return d.Guid;
      }

      public async Task<bool> UploadImageAsync(Guid guid, Stream stream, CancellationToken ct = default)
      {
            if (!await repo.IsAnyGuidAsync(guid))
                  throw new NotFoundException(EntityType.User, guid.ToString());


            var path = await file.SaveUserAsync(stream, guid.ToString());

            return true;
      }

      public async Task<Stream> GetImageByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            if (!await repo.IsAnyGuidAsync(guid))
                  throw new NotFoundException(EntityType.User, guid.ToString());

            return await file.ReadUserAsync(guid.ToString());
      }
}