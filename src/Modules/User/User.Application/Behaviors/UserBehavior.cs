using System.Net;
using System.Security.Cryptography.X509Certificates;
using Adapter.Abstraction.Interfaces;
using Device.Contract.Queries;
using Group.Contract.Queries;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;
using Storage.Contract.Interfaces;
using User.Application.Interfaces;
using User.Contract.DTOs;
using User.Contract.Interfaces;
using User.Domain.Entities;

namespace User.Application.Behaviors;

public sealed class UserBehavior(IUserRepository repo, IStorage file, IAdapterFactory factory, IMessageBus bus) : IUser
{
      public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto)
      {
            var domain = new Domain.Entities.Company(
                  0,
                  dto.Name.Trim(),
                  dto.Address,
                  dto.Description,
                  dto.LocationId,
                  dto.IsActive
            );

            // Check that Company already exists with the same name

            if (await repo.IsCompanyNameExistAsync(dto.Name.Trim()))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(dto.Name)));

            return await repo.CreateCompanyAsync(domain);
      }

      public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto)
      {
            var domain = new Domain.Entities.Department(
                  0,
                  dto.Name.Trim(),
                  dto.Description,
                  dto.CompanyId,
                  dto.LocationId,
                  dto.IsActive
            );

            // Check that Department already exists with the same name

            if (await repo.IsDepartmentExistAsync(dto.Name.Trim()))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(dto.Name)));



            return await repo.CreateDepartmentAsync(domain);
      }

      public async Task<PositionDto> CreatePositionAsync(CreatePositionDto dto)
      {
            var domain = new Domain.Entities.Position(
                  0,
                  dto.Name.Trim(),
                  dto.Description,
                  dto.DepartmentId,
                  dto.LocationId,
                  dto.IsActive
            );

            // Check that Position already exists with the same name

            if (await repo.IsPositionExistAsync(dto.Name.Trim()))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(dto.Name)));



            return await repo.CreatePositionAsync(domain);
      }


      public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
      {
            var domain = new Domain.Entities.Users(
                  0,
                  dto.UserId.Trim(),
                  dto.Title,
                  dto.FirstName.Trim(),
                  dto.MiddleName.Trim(),
                  dto.LastName.Trim(),
                  dto.Gender,
                  dto.DateOfBirth,
                  dto.Email,
                  dto.Phone,
                  dto.CompanyId,
                  dto.DepartmentId,
                  dto.PositionId,
                  dto.Address.Trim(),
                  dto.Flag,
                  dto.Additionals,
                  dto.Image,
                  dto.Credentials.Select(c =>
                  new Domain.Entities.Credential(
                        0,
                        0,
                        dto.Flag,
                        c.Bits,
                        c.Fac,
                        c.CardNumber,
                        c.IssueCode,
                        c.Pin,
                        c.UseCount,
                        c.ApbLoc,
                        c.Active,
                        c.Expire,
                        c.LocationId,
                        c.IsActive
                  ))
                  .ToList(),
                  dto.Groups,
                  dto.LocationId,
                  dto.IsActive
            );

            // Check userid dup 

            if (await repo.IsAnyUserByUserIdAsync(domain.UserId))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(domain.UserId)));

            foreach (var c in domain.Credentials)
            {
                  if (await repo.IsAnyCardNumberAsync(c.CardNumber))
                        throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(c.CardNumber)));
            }


            // Query Group
            var gps = await bus.QueryAsync(new GroupsListByRangeIdQuery(domain.Groups));


            foreach (var g in gps)
            {
                  foreach (var c in domain.Credentials)
                  {
                        // Send data to Controller
                        await factory.GetAdapter(g.Type).User.CreateUserAsync(
                              g.Mac,
                              (short)await bus.QueryAsync(new ComponentIdByMacQuery(g.Mac)),
                              c.Flag,
                              c.CardNumber,
                              c.IssueCode,
                              c.Pin,
                              g.GroupComponentId,
                              c.ApbLoc,
                              c.UseCount,
                              (int)DateTimeHelper.DateTimeToElapeSecond(c.ActiveTime),
                             (int)DateTimeHelper.DateTimeToElapeSecond(c.DeactiveTime),
                              -1,
                              -1,
                              -1,
                              -1
                              );
                  }

            }


            return await repo.CreateUserAsync(domain);
      }

      public async Task<CompanyDto> DeleteCompanyAsync(int id)
      {
            if (!await repo.IsAnyCompanyByIdAsync(id))
                  throw new BadRequestException(MessageHelper.Common.NotFound("Company", id));

            // Check any relate record 
            var relateRecord = await repo.CheckCompanyRelateRecordAsync(id);
            if (!string.IsNullOrEmpty(relateRecord))
                  throw new BadRequestException(MessageHelper.Common.FoundRelatedRecord(relateRecord));

            return await repo.DeleteCompanyAsync(id);
      }

      public async Task<DepartmentDto> DeleteDepartmentAsync(int id)
      {
            if (!await repo.IsAnyDepartmentByIdAsync(id))
                  throw new BadRequestException(MessageHelper.Common.NotFound("Department", id));

            // Check any relate record 
            var relateRecord = await repo.CheckDepartmentRelateRecordAsync(id);
            if (!string.IsNullOrEmpty(relateRecord))
                  throw new BadRequestException(MessageHelper.Common.FoundRelatedRecord(relateRecord));

            return await repo.DeleteDepartmentAsync(id);
      }

      public async Task<PositionDto> DeletePositionAsync(int id)
      {
            if (!await repo.IsAnyPositionByIdAsync(id))
                  throw new BadRequestException(MessageHelper.Common.NotFound("Position", id));

            // Check any relate record 
            var relateRecord = await repo.CheckPositionRelateRecordAsync(id);
            if (!string.IsNullOrEmpty(relateRecord))
                  throw new BadRequestException(MessageHelper.Common.FoundRelatedRecord(relateRecord));

            return await repo.DeletePositionAsync(id);
      }

      public async Task<UserDto> DeleteUserAsync(int id)
      {
            if (!await repo.IsAnyUserByIdAsync(id))
                  throw new BadRequestException(MessageHelper.Common.NotFound("User", id));

            var credentials = await repo.GetCredentialsByUserIdAysnc(id);

            var groupIds = await repo.GetGroupIdsByIdAsync(id);

            var macs = await bus.QueryAsync(new MacsByGroupIdsQuery(groupIds));

            foreach (var mac in macs)
            {
                  var device = await bus.QueryAsync(new DeviceByMacAsyncQuery(mac));
                  // Get All Device that user in 
                  foreach (var cred in credentials)
                  {
                        await factory.GetAdapter(device.Type).User.DeleteUserAsync(
                        device.Mac,
                        device.ComponentId,
                        cred.CardNumber);

                  }

            }


            return await repo.DeleteUserAsync(id);

      }

      public async Task<BaseResponse> UploadImageAsync(string userid, Stream stream)
      {
            if (string.IsNullOrEmpty(userid))
                  throw new BadRequestException(MessageHelper.Common.Empty(nameof(userid)));

            if (!await repo.IsAnyUserByUserIdAsync(userid))
                  throw new BadRequestException(MessageHelper.Common.NotFound("User", userid));

            var path = await file.SaveUserAsync(stream, userid);

            await repo.UpdateImagePathAsync(path, userid);

            return new BaseResponse(HttpStatusCode.OK, MessageHelper.Common.Success, DateTime.UtcNow);
      }

      public async Task<Pagination<CompanyDto>> GetCompanyPaginationAsync(PaginationParams param)
      {
            return await repo.GetCompanyPaginationAsync(param);
      }

      public async Task<Pagination<DepartmentDto>> GetDepartmentPaginationAsync(PaginationParams param)
      {
            return await repo.GetDepartmentPaginationAsync(param);
      }

      public async Task<Pagination<PositionDto>> GetPositionPaginationAsync(PaginationParams param)
      {
            return await repo.GetPositionPaginationAsync(param);
      }

      public async Task<Pagination<UserDto>> GetUserPaginationAsync(PaginationParams param)
      {
            return await repo.GetUserPaginationAsync(param);
      }

      public async Task<CompanyDto> UpdateCompanyAsync(CompanyDto dto)
      {
            var domain = new Domain.Entities.Company(
                  dto.Id,
                  dto.Name,
                  dto.Address,
                  dto.Description,
                  dto.LocationId,
                  dto.IsActive
            );

            // Check first that company is there or not 
            if (!await repo.IsAnyCompanyByIdAsync(dto.Id))
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(dto.Id), dto.Id));

            return await repo.UpdateCompanyAsync(domain);

      }

      public async Task<DepartmentDto> UpdateDepartmentAsync(DepartmentDto dto)
      {
            var domain = new Domain.Entities.Department(
                  dto.Id,
                  dto.Name,
                  dto.Description,
                  dto.CompanyId,
                  dto.LocationId,
                  dto.IsActive
            );

            // Check first that company is there or not 
            if (!await repo.IsAnyDepartmentByIdAsync(dto.Id))
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(dto.Id), dto.Id));



            return await repo.UpdateDepartmentAsync(domain);
      }

      public async Task<PositionDto> UpdatePositionAsync(PositionDto dto)
      {
            var domain = new Domain.Entities.Position(
                  dto.Id,
                  dto.Name,
                  dto.Description,
                  dto.DepartmentId,
                  dto.LocationId,
                  dto.IsActive
            );

            // Check first that company is there or not 
            if (!await repo.IsAnyPositionByIdAsync(dto.Id))
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(dto.Id), dto.Id));

            return await repo.UpdatePositionAsync(domain);
      }

      public async Task<UserDto> UpdateUserAsync(UserDto dto)
      {
            var domain = new Domain.Entities.Users(
                  dto.Id,
                  dto.UserId,
                  dto.Title,
                  dto.FirstName,
                  dto.MiddleName,
                  dto.LastName,
                  dto.Gender,
                  dto.DateOfBirth ?? default,
                  dto.Email,
                  dto.Phone,
                  dto.CompanyId,
                  dto.DepartmentId,
                  dto.PositionId,
                  dto.Address,
                  dto.Flag,
                  dto.Additionals,
                  dto.Image,
                  dto.Credentials.Select(c =>
                        new Credential(
                              c.Id,
                              dto.Id,
                              dto.Flag,
                              c.Bits,
                              c.Fac,
                              c.CardNumber,
                              c.IssueCode,
                              c.Pin,
                              c.UseCount,
                              c.ApbLoc,
                              c.Active,
                              c.Expire,
                              c.LocationId,
                              c.IsActive
                        )
                  ).ToList(),
                  dto.Groups,
                  dto.LocationId,
                  dto.IsActive
                  );

            if (!await repo.IsAnyUserByUserIdAsync(dto.UserId))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(dto.UserId));

            return await repo.UpdateUserAsync(domain);
      }


      public async Task<Stream> GetImageByUserIdAsync(string userid)
      {
            if (string.IsNullOrEmpty(userid))
                  throw new BadRequestException(MessageHelper.Common.Empty(nameof(userid)));

            return await file.ReadUserAsync(userid);
      }

      public async Task<IEnumerable<CompanyDto>> GetCompanyByLocationIdAsync(int LocationId)
      {
            return await repo.GetCompanyByLocationIdAsync(LocationId);
      }

      public async Task<Pagination<DepartmentDto>> GetDepartmentByCompanyAsync(PaginationParams param, int CompanyId)
      {
            var res = await repo.GetDepartmentByCompanyAsync(param, CompanyId);
            return res;
      }

      public async Task<IEnumerable<DepartmentDto>> GetDepartmentByCompanyAsync(int CompanyId)
      {
            var res = await repo.GetDepartmentByCompanyAsync(CompanyId);
            return res;
      }

      public async Task<Pagination<PositionDto>> GetPositionByDepartmentAsync(PaginationParams param, int DepartmentId)
      {
            return await repo.GetPositionByDepartmentAsync(param, DepartmentId);
      }

      public async Task<IEnumerable<OptionDto>> GetCompanyOptionByLocationAsync(int LocationId)
      {
            return await repo.GetCompanyOptionByLocationAsync(LocationId);
      }

      public async Task<IEnumerable<OptionDto>> GetDepartmentOptionByCompanyAsync(int CompanyId)
      {
            return await repo.GetDepartmentOptionByCompanyAsync(CompanyId);
      }

      public async Task<IEnumerable<OptionDto>> GetPositionOptionByDepartmentAsync(int CompanyId)
      {
            return await repo.GetPositionOptionByDepartmentAsync(CompanyId);
      }

      public async Task<IEnumerable<OptionDto>> GetUserFlagOptionAsync()
      {
            return await repo.GetUserFlagOptionAsync();
      }
}