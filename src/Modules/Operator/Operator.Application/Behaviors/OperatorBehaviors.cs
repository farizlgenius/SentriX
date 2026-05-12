using System;
using Location.Contract.DTOs;
using Operator.Application.Interfaces;
using Operator.Contract.DTOs;
using Operator.Contract.Interfaces;
using Operator.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;

namespace Operator.Application.Behaviors;

public sealed class OperatorBehaviors(IOperatorRepository repo) : IOperator
{
      public async Task<OperatorDto> CreateAsync(CreateOperatorDto dto)
      {

            if (string.IsNullOrWhiteSpace(dto.Username))
                  throw new BadRequestException(MessageHelper.Common.UsernameEmpty);

            if (string.IsNullOrWhiteSpace(dto.Password))
                  throw new BadRequestException(MessageHelper.Common.PasswordEmpty);

            if (await repo.IsOperatorExistsByUsernameAsync(dto.Username))
                  throw new BadRequestException(MessageHelper.Common.DuplicatedUsername);

            if (string.IsNullOrWhiteSpace(dto.Firstname))
                  throw new BadRequestException(MessageHelper.Common.FirstnameEmpty);

            if (string.IsNullOrWhiteSpace(dto.Lastname))
                  throw new BadRequestException(MessageHelper.Common.LastnameEmpty);

            if (string.IsNullOrWhiteSpace(dto.Email))
                  throw new BadRequestException(MessageHelper.Common.EmailEmpty);

            if (!await repo.IsLocationIdsValidAsync(dto.LocationId))
                  throw new BadRequestException(MessageHelper.Location.LocationInvalid);

            if (!await repo.IsValidRoleIdAsync(dto.RoleId))
                  throw new BadRequestException(MessageHelper.Role.RoleInvalid);


            var domain = new Operators(
              dto.Username,
              PasswordHasher.HashPassword(dto.Password.Trim()),
              dto.title,
              dto.Firstname,
              dto.Middlename,
              dto.Lastname,
              dto.Gender,
              dto.Email,
              dto.Mobile,
              dto.LocationId,
              dto.RoleId
            );

            return await repo.AddAsync(domain);


      }

      public async Task<OperatorDto> DeleteByIdAsync(int id)
      {
            if (!await repo.IsAnyByIdAsync(id))
                  throw new BadRequestException(MessageHelper.Common.RecordNotFound);

            return await repo.DeleteByIdAsync(id);
      }

      public async Task<Pagination<OperatorDto>> GetPagination(PaginationParams param)
      {

            var res = await repo.GetPagination(param);
            return res;
      }

      public async Task<OperatorDto> UpdateAsync(UpdateOperatorDto dto)
      {

            if (string.IsNullOrWhiteSpace(dto.Username))
                  throw new BadRequestException(MessageHelper.Common.UsernameEmpty);

            if (string.IsNullOrWhiteSpace(dto.Firstname))
                  throw new BadRequestException(MessageHelper.Common.FirstnameEmpty);

            if (string.IsNullOrWhiteSpace(dto.Lastname))
                  throw new BadRequestException(MessageHelper.Common.LastnameEmpty);

            if (string.IsNullOrWhiteSpace(dto.Email))
                  throw new BadRequestException(MessageHelper.Common.EmailEmpty);

            if (await repo.IsLocationIdsValidAsync(dto.LocationId))
                  throw new BadRequestException(MessageHelper.Location.LocationInvalid);

            if (!await repo.IsValidRoleIdAsync(dto.RoleId))
                  throw new BadRequestException(MessageHelper.Role.RoleInvalid);

            var domain = new Operators(
                  dto.Id,
                  dto.Username,
                  dto.Title,
                  dto.Firstname,
                  dto.Middlename,
                  dto.Lastname,
                  dto.Gender,
                  dto.Email,
                  dto.Mobile,
                  dto.LocationId,
                  dto.RoleId
      );

      return await repo.UpdateAsync(domain);
      }

      public async Task<string> GetHashedPasswordAsync(string Username)
      {
            return await repo.GetPassowrdByUsernameAsync(Username);
      }

      public async Task<bool> IsOperatorExistsAsync(string Username)
      {
            return await repo.IsOperatorExistsByUsernameAsync(Username);
      }

      public async Task<OperatorDto> GetOperatorAsync(string Username)
      {
            return await repo.GetOperatorByUsernameAsync(Username);
      }

      public async Task<PasswordRuleDto> CreatePasswordRuleAsync(PasswordRuleDto dto)
      {
            if(dto.Len == 0 )
                  throw new BadRequestException(MessageHelper.Common.PasswordLenEmpty);

            var domain = new PasswordRule(dto.Id,dto.Len,dto.IsDigit,dto.IsLower,dto.IsSymbol,dto.IsUpper,dto.Weaks);
            
            var res = await repo.CreatePasswordRuleAsync(domain);
            return res;
      }

      public async Task<PasswordRuleDto> GetPassowrdRuleAsync()
      {
            var res  = await repo.GetPassowrdRuleAsync();
            return res;
      }
}
