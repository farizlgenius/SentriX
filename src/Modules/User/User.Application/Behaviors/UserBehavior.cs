using System.Net;
using System.Security.Cryptography.X509Certificates;
using Adapter.Abstraction.Interfaces;
using Device.Contract.Queries;
using Group.Contract.Queries;
using Microsoft.EntityFrameworkCore.Storage.Internal;
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

public sealed class UserBehavior(
      ICompanyRepository comRepo,
      IDepartmentRepository depRepo,
      IPositionRepository posRepo,
      IUserRepository userRepo,
      ICredentialRepository credRepo,
      IStorage file,
      IAdapterFactory factory,
      IMessageBus bus
      ) : IUser
{
      public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto)
      {
            var d = new Domain.Entities.Company(
                  Guid.NewGuid(),
                  dto.Name.Trim(),
                  dto.Address,
                  dto.Description,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
            );

            // Check that Company already exists with the same name

            if (await comRepo.IsAnyNameAsync(dto.Name.Trim()))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(dto.Name)));

            await comRepo.AddAsync(d);

            return new CompanyDto(
                  d.Guid,
                  d.Name,
                  d.Address,
                  d.Description,
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );
      }

      public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto)
      {
            var d = new Domain.Entities.Department(
                  Guid.NewGuid(),
                  dto.Name.Trim(),
                  dto.Description,
                  dto.CompanyGuid,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
            );

            // Check that Department already exists with the same name

            if (await depRepo.IsAnyNameAsync(dto.CompanyGuid,dto.Name.Trim()))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(dto.Name)));



            await depRepo.AddAsync(d);

            return new DepartmentDto(
                  d.Guid,
                  d.Name,
                  d.Description,
                  d.CompanyGuid,
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );
      }

      public async Task<PositionDto> CreatePositionAsync(CreatePositionDto dto)
      {
            var d = new Domain.Entities.Position(
                  Guid.NewGuid(),
                  dto.Name.Trim(),
                  dto.Description,
                  dto.DepartmentGuid,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
            );

            // Check that Position already exists with the same name

            if (await posRepo.IsAnyNameAsync(dto.DepartmentGuid, dto.Name.Trim()))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(dto.Name)));



            await posRepo.AddAsync(d);

            return new PositionDto(
                  d.Guid,
                  d.Name,
                  d.Description,
                  d.DepartmentGuid,
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );
      }


      public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
      {
            var userGuid = Guid.NewGuid();
            var credGuid = Guid.NewGuid();
            var d = new Domain.Entities.Users(
                  userGuid,
                  dto.Identification.Trim(),
                  dto.Title,
                  dto.FirstName.Trim(),
                  dto.MiddleName.Trim(),
                  dto.LastName.Trim(),
                  dto.Gender,
                  dto.DateOfBirth,
                  dto.Email,
                  dto.Phone,
                  dto.CompanyGuid,
                  dto.DepartmentGuid,
                  dto.PositionGuid,
                  dto.Address.Trim(),
                  dto.ActiveTime,
                  dto.ExpireTime,
                  dto.Additionals,
                  new Card
                  (
                        Guid.NewGuid(),
                        dto.Card.Bits,
                        dto.Card.CardNumber,
                        credGuid
                  ),
                  new LicensePlate(
                        Guid.NewGuid(),
                        dto.LicensePlate.LicensePlate,
                        credGuid
                  ),
                   new Pin(
                        Guid.NewGuid(),
                        dto.Pin.Pin,
                        credGuid
                  ),
                  new QrCode(
                        Guid.NewGuid(),
                        dto.QrCode.QrCode,
                        credGuid
                  ),
                  new Face(
                        Guid.NewGuid(),
                        dto.Face.ImageName,
                        credGuid
                  ),
                  dto.Groups,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
            );

            // Check userid dup 

            if (await userRepo.IsAnyUserByIdentificationAsync(d.Identification))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(d.Identification)));

                  if (d.Card.CardNumber == -1)
                  {
                        if (await credRepo.IsAnyCardNumberAsync(d.Card.CardNumber))
                              throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(d.Card.CardNumber)));
                  }

                  if (!string.IsNullOrWhiteSpace(d.LicensePlate.LicensePlates))
                  {
                        if (await credRepo.IsAnyLicensePlateAsync(d.LicensePlate.LicensePlates))
                              throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(d.LicensePlate.LicensePlates)));
                  }

                  if (!string.IsNullOrWhiteSpace(d.QrCode.Qr))
                  {
                        if (await credRepo.IsAnyLicensePlateAsync(d.QrCode.Qr))
                              throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(d.QrCode.Qr)));
                  }

                  if (!string.IsNullOrWhiteSpace(d.Pin.Pins))
                  {
                        if (await credRepo.IsAnyLicensePlateAsync(d.Pin.Pins))
                              throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(d.Pin.Pins)));
                  }


            // Query Group
            var gps = await bus.QueryAsync(new GroupsListByRangeGuidQuery(d.Groups));

            foreach (var g in gps)
            {
                  // Send data to Controller
                  await factory.GetAdapter(g.Type).User.AddUserAsync(
                        g.Mac,
                        g.DeviceComponentId,
                        dto.Identification,
                        $"{d.Title} {d.FirstName} {d.MiddleName} ${d.LastName}",
                        (int)DateTimeHelper.DateTimeToElapeSecond(d.ActiveTime),
                       (int)DateTimeHelper.DateTimeToElapeSecond(d.ExpireTime),
                        d.Card.CardNumber,
                        d.LicensePlate.LicensePlates,
                        d.Pin.Pins,
                        d.QrCode.Qr,
                        string.Empty,
                        g.GroupComponentId
                  );

            }


            await userRepo.AddAsync(d);

            return new UserDto(
                  d.Guid,
                  d.Identification,
                  d.Title,
                  d.FirstName,
                  d.MiddleName,
                  d.LastName,
                  d.Gender,
                  d.DateOfBirth,
                  d.Email,
                  d.Phone,
                  d.CompanyGuid ?? Guid.Empty,
                  "",
                  d.DepartmentGuid ?? Guid.Empty,
                  "",
                  d.PositionGuid ?? Guid.Empty,
                  "",
                  d.Address,
                  d.ActiveTime,
                  d.ExpireTime,
                  d.Additionals,
                   new CardDto(
                              d.Card.Guid,
                              d.Card.Bits,
                              d.Card.CardNumber
                        ),
                 new LicensePlateDto(
                        d.LicensePlate.Guid,
                        d.LicensePlate.LicensePlates
                 ),
                 new QrCodeDto(
                        d.QrCode.Guid,
                        d.QrCode.Qr
                 ),
                  new FaceDto(
                        d.Face.Guid,
                        d.Face.ImageName
                  ),
                  new PinDto(
                        d.Pin.Guid,
                        d.Pin.Pins
                  ),
                  d.Groups,
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );
      }

      public async Task<CompanyDto> DeleteCompanyAsync(Guid guid)
      {
            var dto = await comRepo.GetByGuidAsync(guid);
            if (dto.Guid == Guid.Empty)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Company", guid.ToString()));

            // Check any relate record 
            var IsAnyRelate = await comRepo.IsAnyRelateAsync(guid);
            if (IsAnyRelate)
                  throw new BadRequestException(MessageHelper.Common.FoundRelatedRecord());

            await comRepo.DeleteAsync(guid);

            return dto;
      }

      public async Task<DepartmentDto> DeleteDepartmentAsync(Guid guid)
      {
            var dto = await depRepo.GetByGuidAsync(guid);
            if (dto.Guid == Guid.Empty)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Department", guid.ToString()));

            // Check any relate record 
            var IsAnyRelate = await depRepo.IsAnyRelateAsync(guid);
            if (IsAnyRelate)
                  throw new BadRequestException(MessageHelper.Common.FoundRelatedRecord());

            await depRepo.DeleteAsync(guid);

            return dto;
      }

      public async Task<PositionDto> DeletePositionAsync(Guid guid)
      {
            var dto = await posRepo.GetByGuidAsync(guid);
            if (dto.Guid == Guid.Empty)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Position", guid.ToString()));

            // Check any relate record 
            var IsAnyRelate = await posRepo.IsAnyRelateAsync(guid);
            if (IsAnyRelate)
                  throw new BadRequestException(MessageHelper.Common.FoundRelatedRecord());

           await posRepo.DeleteAsync(guid);

           return dto;
      }

      public async Task<UserDto> DeleteUserAsync(Guid guid)
      {
            var dto = await userRepo.GetByGuidAsync(guid);
            if (dto.Guid == Guid.Empty)
                  throw new BadRequestException(MessageHelper.Common.NotFound("User", guid.ToString()));

            var gps = await bus.QueryAsync(new GroupsListByRangeGuidQuery(dto.Groups));

            foreach (var g in gps)
            {
                  await factory.GetAdapter(g.Type).User.DeleteUserAsync(
                        g.Mac,
                        g.DeviceComponentId,
                        dto.Card.CardNumber,
                        dto.LicensePlate.LicensePlate,
                        dto.Pin.Pin,
                        dto.QrCode.QrCode,
                        dto.Face.ImageName
                        );

            }


            await userRepo.DeleteAsync(guid);

            return dto;

      }

      public async Task UploadImageAsync(string userid, Stream stream)
      {
            if (string.IsNullOrEmpty(userid))
                  throw new BadRequestException(MessageHelper.Common.Empty(nameof(userid)));

            if (!await userRepo.IsAnyUserByIdentificationAsync(userid))
                  throw new BadRequestException(MessageHelper.Common.NotFound("User", userid));

            var path = await file.SaveUserAsync(stream, userid);

            await userRepo.UpdateImagePathAsync(path, userid);

      }

      public async Task<Pagination<CompanyDto>> GetCompanyPaginationAsync(PaginationParams param)
      {
            return await comRepo.GetPaginationAsync(param);
      }

      public async Task<Pagination<DepartmentDto>> GetDepartmentPaginationAsync(PaginationParams param)
      {
            return await depRepo.GetPaginationAsync(param);
      }

      public async Task<Pagination<PositionDto>> GetPositionPaginationAsync(PaginationParams param)
      {
            return await posRepo.GetPaginationAsync(param);
      }

      public async Task<Pagination<UserDto>> GetUserPaginationAsync(PaginationParams param)
      {
            return await userRepo.GetPaginationAsync(param);
      }

      public async Task<CompanyDto> UpdateCompanyAsync(CompanyDto dto)
      {
            // Check first that company is there or not 
            if (!await comRepo.IsAnyByGuidAsync(dto.Guid))
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(dto.Guid),dto.Guid.ToString()));

            // Check that any company with the same name in this location
            if(!await comRepo.IsAnyNameAsync(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(dto.Name)));

            var d = new Domain.Entities.Company(
                  dto.Guid,
                  dto.Name,
                  dto.Address,
                  dto.Description,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
            );

            await comRepo.UpdateAsync(d);

            return dto;

      }

      public async Task<DepartmentDto> UpdateDepartmentAsync(DepartmentDto dto)
      {
            // Check first that company is there or not 
            if (!await depRepo.IsAnyByGuidAsync(dto.Guid))
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(dto.Guid),dto.Guid.ToString()));

            // Check that any company with the same name in this location
            if(!await depRepo.IsAnyNameAsync(dto.CompanyGuid,dto.Name))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(dto.Name)));

            var domain = new Domain.Entities.Department(
                  dto.Guid,
                  dto.Name,
                  dto.Description,
                  dto.CompanyGuid,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
            );

            await depRepo.UpdateAsync(domain);

            return dto;
      }

      public async Task<PositionDto> UpdatePositionAsync(PositionDto dto)
      {
            // Check first that company is there or not 
            if (!await posRepo.IsAnyByGuidAsync(dto.Guid))
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(dto.Guid),dto.Guid.ToString()));

            // Check that any company with the same name in this location
            if(!await posRepo.IsAnyNameAsync(dto.DepartmentGuid,dto.Name))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(dto.Name)));

            var domain = new Domain.Entities.Position(
                  dto.Guid,
                  dto.Name,
                  dto.Description,
                  dto.DepartmentGuid,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
            );

            await posRepo.UpdateAsync(domain);

            return dto;
      }

      public async Task<UserDto> UpdateUserAsync(UserDto dto)
      {

            // Check first that company is there or not 
            if (!await posRepo.IsAnyByGuidAsync(dto.Guid))
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(dto.Guid),dto.Guid.ToString()));

            if (!await userRepo.IsAnyUserByIdentificationAsync(dto.Identification))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(dto.Identification));

            var d = new Domain.Entities.Users(
                  dto.Guid,
                  dto.Identification,
                  dto.Title,
                  dto.FirstName,
                  dto.MiddleName,
                  dto.LastName,
                  dto.Gender,
                  dto.DateOfBirth ?? default,
                  dto.Email,
                  dto.Phone,
                  dto.CompanyGuid,
                  dto.DepartmentGuid,
                  dto.PositionGuid,
                  dto.Address,
                  dto.ActiveTime,
                  dto.ExpireTime,
                  dto.Additionals,
                  new Card(
                        dto.Card.Guid,
                        dto.Card.Bits,
                        dto.Card.CardNumber,
                        dto.Guid
                  ),
                  new LicensePlate(
                        dto.LicensePlate.Guid,
                        dto.LicensePlate.LicensePlate,
                        dto.Guid
                  ),
                  new Pin(
                        dto.Pin.Guid,
                        dto.Pin.Pin,
                        dto.Guid
                  ),
                  new QrCode(
                        dto.QrCode.Guid,
                        dto.QrCode.QrCode,
                        dto.Guid
                  ),
                  new Face(
                        dto.Face.Guid,
                        dto.Face.ImageName,
                        dto.Guid
                  ),
                  dto.Groups,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
                  );

            

            await userRepo.UpdateAsync(d);

            return new UserDto(
                  d.Guid,
                  d.Identification,
                  d.Title,
                  d.FirstName,
                  d.MiddleName,
                  d.LastName,
                  d.Gender,
                  d.DateOfBirth,
                  d.Email,
                  d.Phone,
                  d.CompanyGuid ?? Guid.Empty,
                  "",
                  d.DepartmentGuid ?? Guid.Empty,
                  "",
                  d.PositionGuid ?? Guid.Empty,
                  "",
                  d.Address,
                  d.ActiveTime,
                  d.ExpireTime,
                  d.Additionals,
                   new CardDto(
                              d.Card.Guid,
                              d.Card.Bits,
                              d.Card.CardNumber
                        ),
                 new LicensePlateDto(
                        d.LicensePlate.Guid,
                        d.LicensePlate.LicensePlates
                 ),
                 new QrCodeDto(
                        d.QrCode.Guid,
                        d.QrCode.Qr
                 ),
                  new FaceDto(
                        d.Face.Guid,
                        d.Face.ImageName
                  ),
                  new PinDto(
                        d.Pin.Guid,
                        d.Pin.Pins
                  ),
                  d.Groups,
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );
      }


      public async Task<Stream> GetImageByUserIdAsync(string userid)
      {
            
            if (string.IsNullOrEmpty(userid))
                  throw new BadRequestException(MessageHelper.Common.Empty(nameof(userid)));

            return await file.ReadUserAsync(userid);
      }

      public async Task<IEnumerable<CompanyDto>> GetCompanyByLocationIdAsync(int LocationId)
      {
            return await comRepo.GetByLocationIdAsync(LocationId);
      }

      public async Task<Pagination<DepartmentDto>> GetDepartmentPaginationByCompanyGuidAsync(PaginationParams param, Guid companyGuid)
      {
            var res = await depRepo.GetPaginationByCompanyGuidAsync(param, companyGuid);
            return res;
      }

      public async Task<IEnumerable<DepartmentDto>> GetDepartmentByCompanyGuidAsync(Guid companyGuid)
      {
            var res = await depRepo.GetDepartmentByCompanyGuidAsync(companyGuid);
            return res;
      }

      public async Task<Pagination<PositionDto>> GetPositionPaginationByDepartmentGuidAsync(PaginationParams param, Guid guid)
      {
            return await posRepo.GetPositionPaginationByDepartmentGuidAsync(param, guid);
      }

      public async Task<IEnumerable<OptionDto>> GetCompanyOptionByLocationIdAsync(int LocationId)
      {
            return await comRepo.GetCompanyOptionByLocationIdAsync(LocationId);
      }

      public async Task<IEnumerable<OptionDto>> GetDepartmentOptionByCompanyGuidAsync(Guid guid)
      {
            return await depRepo.GetDepartmentOptionByCompanyGuidAsync(guid);
      }

      public async Task<IEnumerable<OptionDto>> GetPositionOptionByDepartmentGuidAsync(Guid departmentGuid)
      {
            return await posRepo.GetPositionOptionByDepartmentGuidAsync(departmentGuid);
      }

      public async Task<IEnumerable<OptionDto>> GetUserFlagOptionAsync()
      {
            return await userRepo.GetUserFlagOptionAsync();
      }
}