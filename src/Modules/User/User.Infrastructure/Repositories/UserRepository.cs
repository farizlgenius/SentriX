using System.Text;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using User.Application.Interfaces;
using User.Contract.DTOs;
using User.Domain.Entities;
using User.Infrastructure.Persistences;
using User.Infrastructure.Persistences.Entities;

namespace User.Infrastructure.Repositories;

public sealed class UserRepository(UserDbContext context) : IUserRepository
{

      public async Task AddAsync(Domain.Entities.Users domain, CancellationToken ct = default)
      {
            var entity = new Persistences.Entities.Users(domain);

            // Add Additional
            entity.additionals = domain.Additionals.Select(x => new UserAdditional(x)).ToArray();

            // Add Credential
            if (domain.Card is not null)
                  entity.card = new Persistences.Entities.Card(domain.Card);

            if (domain.Pin is not null)
                  entity.pin = new Persistences.Entities.Pin(domain.Pin);

            if (domain.QrCode is not null)
                  entity.qr_code = new Persistences.Entities.QrCode(domain.QrCode);

            if (domain.LicensePlate is not null)
                  entity.license_plate = new Persistences.Entities.LicensePlate(domain.LicensePlate);

            if (domain.Face is not null)
                  entity.face = new Persistences.Entities.Face(domain.Face);

            // Add User Group
            entity.user_groups = domain.Groups.Select(x => new UserGroup(
                  Guid.NewGuid(),
                  x,
                  domain.Guid
                  )).ToArray();

            await context.Users.AddAsync(entity, ct);

            await context.SaveChangesAsync(ct);

      }




      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Users
            .OrderByDescending(x => x.id)
            .Where(x => x.guid == guid)
            .FirstOrDefaultAsync();

            if (entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            context.Users.Remove(entity);
            await context.SaveChangesAsync(ct);

      }


      public async Task<IEnumerable<OptionDto>> GetCompanyOptionByLocationAsync(int locationId, CancellationToken ct = default)
      {
            var res = await context.Companies.AsNoTracking()
            .Where(x => x.location_id == locationId)
            .Select(x => new OptionDto(
                  x.name,
                  x.id,
                  string.Empty,
                  Guid.Empty,
                  false
                  )).ToArrayAsync();

            return res;
      }
     

   

      public async Task<IEnumerable<OptionDto>> GetUserFlagOptionAsync(CancellationToken ct = default)
      {
            return await context.UserFlags.AsNoTracking()
            .Select(x => new OptionDto(
             x.label,
             x.value,
             x.description))
            .ToArrayAsync();
      }

      public async Task<Pagination<UserDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Users.AsNoTracking().Where(x => x.location_id == param.locationId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.identification, pattern) ||
                                  EF.Functions.ILike(x.title, pattern) ||
                                  EF.Functions.ILike(x.first_name, pattern) ||
                                  EF.Functions.ILike(x.middle_name, pattern) ||
                                  EF.Functions.ILike(x.last_name, pattern) ||
                                  EF.Functions.ILike(x.gender, pattern) ||
                                  EF.Functions.ILike(x.email, pattern) ||
                                  EF.Functions.ILike(x.phone, pattern) ||
                                  EF.Functions.ILike(x.address, pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.identification.Contains(search) ||
                                  x.title.Contains(search) ||
                                  x.first_name.Contains(search) ||
                                  x.middle_name.Contains(search) ||
                                  x.last_name.Contains(search) ||
                                  x.gender.Contains(search) ||
                                  x.email.Contains(search) ||
                                  x.phone.Contains(search) ||
                                  x.address.Contains(search)
                              );
                        }
                  }
            }


            if (param.startDate != null)
            {
                  var startUtc = DateTime.SpecifyKind(param.startDate.Value, DateTimeKind.Utc);
                  query = query.Where(x => x.created_at >= startUtc);
            }

            if (param.endDate != null)
            {
                  var endUtc = DateTime.SpecifyKind(param.endDate.Value, DateTimeKind.Utc);
                  query = query.Where(x => x.created_at <= endUtc);
            }

            var totalItems = await query.CountAsync();
            var items = await query.OrderByDescending(r => r.id)
            .Skip((param.pageNumber - 1) * param.pageSize)
            .Take(param.pageSize)
            .Select(x => new UserDto(
            x.guid,
            x.identification,
            x.title,
            x.first_name,
            x.middle_name,
            x.last_name,
            x.gender,
            x.date_of_birth,
            x.email,
            x.phone,
            x.company_guid ?? Guid.Empty,
            x.company == null ? "" : x.company.name,
            x.department_guid ?? Guid.Empty,
            x.department == null ? "" : x.department.name,
            x.position_guid ?? Guid.Empty,
            x.position == null ? "" : x.position.name,
            x.address,
            x.active_time,
            x.expire_time,
            x.additionals.Select(x => x.additional).ToList(),
            x.card != null ? new CardDto(
                  x.card.guid,
                  x.card.bits,
                  x.card.card_number
            ) : new CardDto(),
            x.license_plate != null ? new LicensePlateDto(
                  x.license_plate.guid,
                  x.license_plate.license_plate
            ) : new LicensePlateDto(),
            x.qr_code != null ? new QrCodeDto(
                  x.qr_code.guid,
                  x.qr_code.qr_code
            ) : new QrCodeDto(),
            x.face != null ? new FaceDto(
                  x.face.guid,
                  x.face.image_name
            ) : new FaceDto(),
            x.pin != null ? new PinDto(
                  x.pin.guid,
                  x.pin.pin
            ) : new PinDto(),
            x.user_groups.Select(x => x.group_guid).ToList(),
            x.location_id,
            x.is_active,
            x.is_default
           ))
            .ToListAsync(ct);

            return new Pagination<UserDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }


      public async Task<bool> IsAnyUserByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Users.AsNoTracking().AnyAsync(x => x.guid == guid, ct);
      }

      public async Task<bool> IsAnyUserByIdentificationAsync(string userid, CancellationToken ct = default)
      {
            return await context.Users.AnyAsync(x => x.identification == userid, ct);
      }

      public async Task<bool> IsCompanyNameExistAsync(string name, CancellationToken ct = default)
      {
            return await context.Companies.AnyAsync(x => x.name.Equals(name), ct);
      }

      public async Task<bool> IsDepartmentExistAsync(string name, CancellationToken ct = default)
      {
            return await context.Departments.AnyAsync(x => x.name.Equals(name), ct);
      }

      public async Task<bool> IsPositionExistAsync(string name, CancellationToken ct = default)
      {
            return await context.Positions.AnyAsync(x => x.name.Equals(name), ct);
      }




      public async Task UpdateImagePathAsync(string path, string userid, CancellationToken ct = default)
      {
            var entity = await context.Users.FirstOrDefaultAsync(x => x.identification.Equals(userid), ct);
            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            // entity.image = path;
            var data = context.Users.Update(entity);
            var save = await context.SaveChangesAsync(ct);

            if (data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

      }



      public async Task UpdateAsync(Domain.Entities.Users user, CancellationToken ct = default)
      {

            var entity = await context.Users
                        .Include(x => x.card)
                        .Include(x => x.license_plate)
                        .Include(x => x.pin)
                        .Include(x => x.qr_code)
                        .Include(x => x.face)
                        .Include(x => x.additionals)
                        .Include(x => x.user_groups)
                        .FirstOrDefaultAsync(x => x.guid == user.Guid, ct);

            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(user);

            context.UserAdditionals.RemoveRange(entity.additionals);
            context.UserGroups.RemoveRange(entity.user_groups);

            entity.additionals = user.Additionals
                  .Select(x => new UserAdditional(x))
                  .ToList();

            entity.user_groups = user.Groups
            .Select(x => new UserGroup(
                  Guid.NewGuid(),
                  x,
                  user.Guid
                  ))
            .ToList();


            context.Users.Update(entity);
            await context.SaveChangesAsync(ct);

          
      }

      private async Task<bool> UpdateAdditionalAsync(Guid guid, List<string> news,CancellationToken ct = default)
      {
            var additionals = await context.UserAdditionals
            .OrderByDescending(x => x.id)
            .Where(x => x.guid == guid)
            .ToListAsync(ct);

            // Remove 
            var remove = additionals.Where(x => !news.Contains(x.additional)).ToArray();
            // Dup
            var duplicate = news.Where(x => additionals.Any(a => x.Equals(a.additional))).ToArray();
            // Add
            var add = news.Where(x => duplicate.Any(d => !x.Equals(d)) && remove.Any(r => !r.additional.Equals(x))).Select(x => new UserAdditional(x)).ToArray();


            context.UserAdditionals.RemoveRange(remove);
            await context.UserAdditionals.AddRangeAsync(add);
            var save = await context.SaveChangesAsync(ct);

            if (save <= 0)
                  return false;

            return true;
      }



      public async Task<bool> IsAnyUserNotSyncAsync(IEnumerable<Guid> GpIds, int LocationId, DateTime SyncAt, CancellationToken ct = default)
      {
            return await context.Users.AsNoTracking()
            .AnyAsync(x => x.location_id == LocationId && x.updated_at > SyncAt && x.user_groups.Any(g => GpIds.Contains(g.group_guid)));
      }



      public async Task<UserDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Users.AsNoTracking()
             .Include(x => x.card)
             .Include(x => x.license_plate)
             .Include(x => x.pin)
             .Include(x => x.face)
             .Include(x => x.qr_code)
            .Where(x => x.guid == guid)
            .Select(x => new UserDto(
             x.guid,
             x.identification,
             x.title,
             x.first_name,
             x.middle_name,
             x.last_name,
             x.gender,
             x.date_of_birth,
             x.email,
             x.phone,
             x.company_guid ?? Guid.Empty,
             x.company == null ? "" : x.company.name,
             x.department_guid ?? Guid.Empty,
             x.department == null ? "" : x.department.name,
             x.position_guid ?? Guid.Empty,
             x.position == null ? "" : x.position.name,
             x.address,
             x.active_time,
             x.expire_time,
             x.additionals.Select(x => x.additional).ToList(),
             x.card != null ? new CardDto(
                   x.card.guid,
                   x.card.bits,
                   x.card.card_number
             ) : new CardDto(),
             x.license_plate != null ? new LicensePlateDto(
                   x.license_plate.guid,
                   x.license_plate.license_plate
             ) : new LicensePlateDto(),
             x.qr_code != null ? new QrCodeDto(
                   x.qr_code.guid,
                   x.qr_code.qr_code
             ) : new QrCodeDto(),
             x.face != null ? new FaceDto(
                   x.face.guid,
                   x.face.image_name
             ) : new FaceDto(),
             x.pin != null ? new PinDto(
                   x.pin.guid,
                   x.pin.pin
             ) : new PinDto(),
             x.user_groups.Select(x => x.group_guid).ToList(),
             x.location_id,
             x.is_active,
             x.is_default
            )).FirstOrDefaultAsync() ?? new UserDto();

      }



      public async Task<IEnumerable<UserDto>> GetUserByGroupGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await context.Users.AsNoTracking()
            .Where(x => x.user_groups.Any(x => guids.Contains(x.group_guid)))
            .Select(x => new UserDto(
             x.guid,
             x.identification,
             x.title,
             x.first_name,
             x.middle_name,
             x.last_name,
             x.gender,
             x.date_of_birth,
             x.email,
             x.phone,
             x.company_guid ?? Guid.Empty,
             x.company == null ? "" : x.company.name,
             x.department_guid ?? Guid.Empty,
             x.department == null ? "" : x.department.name,
             x.position_guid ?? Guid.Empty,
             x.position == null ? "" : x.position.name,
             x.address,
             x.active_time,
             x.expire_time,
             x.additionals.Select(x => x.additional).ToList(),
             x.card != null ? new CardDto(
                   x.card.guid,
                   x.card.bits,
                   x.card.card_number
             ) : new CardDto(),
             x.license_plate != null ? new LicensePlateDto(
                   x.license_plate.guid,
                   x.license_plate.license_plate
             ) : new LicensePlateDto(),
             x.qr_code != null ? new QrCodeDto(
                   x.qr_code.guid,
                   x.qr_code.qr_code
             ) : new QrCodeDto(),
             x.face != null ? new FaceDto(
                   x.face.guid,
                   x.face.image_name
             ) : new FaceDto(),
             x.pin != null ? new PinDto(
                   x.pin.guid,
                   x.pin.pin
             ) : new PinDto(),
             x.user_groups.Select(x => x.group_guid).ToList(),
             x.location_id,
             x.is_active,
             x.is_default
            )).ToArrayAsync();
      }
}