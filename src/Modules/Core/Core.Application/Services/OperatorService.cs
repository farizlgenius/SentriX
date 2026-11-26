using Core.Application.Interfaces;
using Core.Contract.DTOs.Operator;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;
using Storage.Contract.Interfaces;

namespace Core.Application.Services;

public sealed class OperatorService(
      IOperatorRepository oper,
      IRoleRepository role,
      ILocationRepository loc,
      IStorage file) : IOperator
{
      public async Task<Guid> CreateAsync(CreateOperatorDto dto, CancellationToken ct = default)
      {
            if (await oper.IsAnyUsernameAsync(dto.Username, ct))
                  throw new BadRequestException(EntityType.OPERATOR.ToString(), "Username already exists.");

            if (await oper.IsAnyEmailAsync(dto.Email, ct))
                  throw new BadRequestException(EntityType.OPERATOR.ToString(), "Email already exists.");

            if (!await role.IsAnyGuidAsync(dto.Role.Guid ?? Guid.Empty,ct))
                  throw new BadRequestException(EntityType.OPERATOR.ToString(), $"Role Guid {dto.Role.Guid} is not valid.");

            if(await loc.IsAnyInvalidGuidsAsync(dto.Locations.Select(x => x.Guid ?? Guid.Empty), ct))
                  throw new BadRequestException(EntityType.OPERATOR.ToString(), $"Location Guid(s) is/are not valid.");

            var roleId = await role.GetIdByGuidAsync(dto.Role.Guid ?? Guid.Empty,ct);
            var locationIds = await loc.GetIdsByGuidsAsync(dto.Locations.Select(x => x.Guid ?? Guid.Empty),ct);


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
                  locationIds
            );

            await oper.AddAsync(d, ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            if (!await oper.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.OPERATOR.ToString(), guid.ToString());

            await oper.DeleteAsync(guid, ct);

            return true;
      }

      public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            if (!guids.Any())
                  throw new BadRequestException(EntityType.OPERATOR.ToString(), "Guid list is empty.");

            foreach (var guid in guids)
            {
                  if (!await oper.IsAnyGuidAsync(guid, ct))
                        throw new NotFoundException(EntityType.OPERATOR.ToString(), guid.ToString());
            }

            await oper.DeleteRangeAsync(guids, ct);

            return guids;
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            if (!await oper.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.OPERATOR.ToString(), guid.ToString());

            await oper.DisableAsync(guid, ct);

            return true;
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            if (!await oper.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.OPERATOR.ToString(), guid.ToString());

            await oper.EnableAsync(guid, ct);

            return true;
      }

      public async Task<OperatorDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            if (!await oper.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.OPERATOR.ToString(), guid.ToString());

            return await oper.GetAsync(guid, ct);
      }


      public async Task<IEnumerable<OperatorDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            return await oper.GetByLocationAsync(guid, ct);
      }

      public async Task<string> GetHashedPasswordByUsernameAsync(string username, CancellationToken ct = default)
      {
            return await oper.GetPassowrdByUsernameAsync(username, ct);
      }

      public async Task<OperatorDto> GetOperatorByUsernameAsync(string username, CancellationToken ct = default)
      {
            return await oper.GetOperatorByUsernameAsync(username, ct);
      }

      public async Task<Pagination<OperatorDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await oper.GetPaginationAsync(param, ct);
      }

      public async Task<Guid> UpdateAsync(UpdateOperatorDto dto, CancellationToken ct = default)
      {
            if (!await oper.IsAnyGuidAsync(dto.Guid, ct))
                  throw new NotFoundException(EntityType.OPERATOR.ToString(), dto.Guid.ToString());

            if (!await oper.IsAnyUsernameAsync(dto.Username, ct))
                  throw new BadRequestException(EntityType.OPERATOR.ToString(), "Username already exists.");

            if (!await oper.IsAnyEmailAsync(dto.Email, ct))
                  throw new BadRequestException(EntityType.OPERATOR.ToString(), "Email already exists.");

            if (!await role.IsAnyGuidAsync(dto.Role.Guid ?? Guid.Empty,ct))
                  throw new BadRequestException(EntityType.OPERATOR.ToString(), $"Role Guid {dto.Role.Guid} is not valid.");

            if(await loc.IsAnyInvalidGuidsAsync(dto.Locations.Select(x => x.Guid ?? Guid.Empty), ct))
                  throw new BadRequestException(EntityType.OPERATOR.ToString(), $"Location Guid(s) is/are not valid.");


            var roleId = await role.GetIdByGuidAsync(dto.Role.Guid ?? Guid.Empty,ct);
            var locationIds = await loc.GetIdsByGuidsAsync(dto.Locations.Select(x => x.Guid ?? Guid.Empty),ct);

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
                  locationIds
            );

            await oper.UpdateAsync(d, ct);

            return d.Guid;
      }

      public async Task<bool> UploadImageAsync(Guid guid, Stream stream, CancellationToken ct = default)
      {
            if (!await oper.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.USER.ToString(), guid.ToString());


            var path = await file.SaveUserAsync(stream, guid.ToString());

            return true;
      }

      public async Task<Stream> GetImageByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            if (!await oper.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.USER.ToString(), guid.ToString());

            return await file.ReadUserAsync(guid.ToString());
      }
}