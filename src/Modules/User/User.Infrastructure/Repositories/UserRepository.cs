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
      public async Task<string> CheckCompanyRelateRecordAsync(int id, CancellationToken ct = default)
      {
            List<string> relatedRecords = new List<string>();
            var hasUsers = await context.Companies.AsNoTracking().Where(x => x.id == id).AnyAsync(x => x.users.Any(), ct);
            var hasDepartments = await context.Companies.AsNoTracking().Where(x => x.id == id).AnyAsync(x => x.departments.Any(), ct);

            if (hasUsers)
                  relatedRecords.Add("Users");
            if (hasDepartments)
                  relatedRecords.Add("Departments");

            return string.Join(",", relatedRecords);
      }

      public async Task<string> CheckDepartmentRelateRecordAsync(int id, CancellationToken ct = default)
      {
            List<string> relatedRecords = new List<string>();
            var hasUsers = await context.Departments.AsNoTracking().Where(x => x.id == id).AnyAsync(x => x.users.Any(), ct);
            var hasPositions = await context.Departments.AsNoTracking().Where(x => x.id == id).AnyAsync(x => x.positions.Any(), ct);

            if (hasUsers)
                  relatedRecords.Add("Users");
            if (hasPositions)
                  relatedRecords.Add("Positions");

            return string.Join(", ", relatedRecords);
      }

      public async Task<string> CheckPositionRelateRecordAsync(int id, CancellationToken ct = default)
      {
            List<string> relatedRecords = new List<string>();
            var hasUsers = await context.Positions.AsNoTracking().Where(x => x.id == id).AnyAsync(x => x.users.Any(), ct);

            if (hasUsers)
                  relatedRecords.Add("Users");

            return string.Join(",", relatedRecords);
      }

      public async Task<CompanyDto> CreateCompanyAsync(Domain.Entities.Company dto, CancellationToken ct = default)
      {
            var entity = new Persistences.Entities.Company(dto);
            var data = await context.Companies.AddAsync(entity, ct);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new CompanyDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.address,
                  data.Entity.description,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }

      public async Task<DepartmentDto> CreateDepartmentAsync(Domain.Entities.Department dto, CancellationToken ct = default)
      {
            var entity = new Persistences.Entities.Department(dto);
            var data = await context.Departments.AddAsync(entity, ct);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new DepartmentDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.description,
                  data.Entity.company_id,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }

      public async Task<PositionDto> CreatePositionAsync(Domain.Entities.Position dto, CancellationToken ct = default)
      {
            var entity = new Persistences.Entities.Position(dto);
            var data = await context.Positions.AddAsync(entity, ct);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new PositionDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.description,
                  data.Entity.component_id,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }

      public async Task<UserDto> CreateUserAsync(Domain.Entities.Users dto, CancellationToken ct = default)
      {
            var entity = new Persistences.Entities.Users(dto);

            // Add Additional
            entity.additionals = dto.Additionals.Select(x => new UserAdditional(x)).ToArray();

            // Add Credential
            entity.credentials = dto.Credentials.Select(x => new Persistences.Entities.Credential(x)).ToArray();

            // Add User Group
            entity.user_groups = dto.Groups.Select(x => new UserGroup(
                  x,
                  dto.LocationId,
                  true
                  )).ToArray();

            var data = await context.Users.AddAsync(entity, ct);

            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return await context.Users.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.id == data.Entity.id && x.user_id == data.Entity.user_id)
            .Select(x => new UserDto(
                  x.id,
                  x.user_id,
                  x.title,
                  x.first_name,
                  x.middle_name,
                  x.last_name,
                  x.gender,
                  x.date_of_birth,
                  x.email,
                  x.phone,
                  x.company_id,
                  x.company.name,
                  x.department_id,
                  x.department.name,
                  x.position_id,
                  x.position.name,
                  x.address,
                  x.additionals.Select(x => x.additional).ToList(),
                  x.image,
                  x.credentials.Select(c => new CredentialDto(
                        c.id,
                        c.flag,
                        c.bits,
                        c.fac,
                        c.card_number,
                        c.issue_code,
                        c.pin,
                        c.use_count,
                        c.apb_loc,
                        c.act_time,
                        c.deact_time,
                        c.location_id,
                        c.is_active
                  )).ToList(),
                  x.user_groups.Select(g => g.group_id).ToList(),
                  x.vacation_id ?? 0,
                  x.location_id,
                  x.is_active
            ))
            .FirstOrDefaultAsync() ?? new UserDto();

          
      }

      public async Task<CompanyDto> DeleteCompanyAsync(int id, CancellationToken ct = default)
      {
            var entity = await context.Companies.FirstOrDefaultAsync(x => x.id == id, ct);
            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);
      

            var data = context.Companies.Remove(entity);
            var save = await context.SaveChangesAsync(ct);

            if (data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return new CompanyDto(
                  entity.id,
                  entity.name,
                  entity.address,
                  entity.description,
                  entity.location_id,
                  entity.is_active
            );
      }

      public async Task<DepartmentDto> DeleteDepartmentAsync(int id, CancellationToken ct = default)
      {
            var entity = await context.Departments.FirstOrDefaultAsync(x => x.id == id, ct);
            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            var data = context.Departments.Remove(entity);
            var save = await context.SaveChangesAsync(ct);

            if (data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return new DepartmentDto(
                  entity.id,
                  entity.name,
                  entity.description,
                  entity.company_id,
                  entity.location_id,
                  entity.is_active
            );
      }
      

      public async Task<PositionDto> DeletePositionAsync(int id, CancellationToken ct = default)
      {
            var entity = await context.Positions.FirstOrDefaultAsync(x => x.id == id, ct);
            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            var data = context.Positions.Remove(entity);
            var save = await context.SaveChangesAsync(ct);

            if (data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return new PositionDto(
                  entity.id,
                  entity.name,
                  entity.description,
                  entity.department_id,
                  entity.location_id,
                  entity.is_active
            );
      }

      public async Task<UserDto> DeleteUserAsync(int id, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<CompanyDto>> GetCompanyByLocationIdAsync(int LocationId, CancellationToken ct = default)
      {
            var res = await context.Companies.AsNoTracking()
            .Where(x => x.location_id == LocationId)
            .Select(x => new CompanyDto(
                  x.id,
                  x.name,
                  x.address,
                  x.description,
                  x.location_id,
                  x.is_active
                  )).ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetCompanyOptionByLocationAsync(int locationId, CancellationToken ct = default)
      {
            var res = await context.Companies.AsNoTracking()
            .Where(x => x.location_id == locationId)
            .Select(x => new OptionDto(
                  x.name,
                  x.id,
                  string.Empty,
                  0,
                  false
                  )).ToArrayAsync();

            return res;
      }

      public async Task<Pagination<CompanyDto>> GetCompanyPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Companies.AsNoTracking().Where(x => x.location_id == param.locationId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.name, pattern) ||
                                  EF.Functions.ILike(x.address, pattern) ||
                                  EF.Functions.ILike(x.description, pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.address.Contains(search) ||
                                  x.description.Contains(search) 
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
            .Select(u => new CompanyDto(
                  u.id,
                  u.name,
                  u.address,
                  u.description,
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<CompanyDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
           
      }

      public async Task<Pagination<DepartmentDto>> GetDepartmentByCompanyAsync(PaginationParams param, int companyId, CancellationToken ct = default)
      {
            var query = context.Departments.AsNoTracking().Where(x => x.location_id == param.locationId && x.company_id == companyId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.name, pattern) ||
                                  EF.Functions.ILike(x.description, pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.description.Contains(search) 
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
            .Select(u => new DepartmentDto(
                  u.id,
                  u.name,
                  u.description,
                  u.company_id,
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<DepartmentDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }

      public async Task<IEnumerable<DepartmentDto>> GetDepartmentByCompanyAsync(int companyId, CancellationToken ct = default)
      {
            var res = await context.Departments.AsNoTracking()
            .Where(x => x.company_id == companyId)
            .Select(x => new DepartmentDto(
                  x.id,
                  x.name,
                  x.description,
                  x.company_id,
                  x.location_id,
                  x.is_active
            )).ToArrayAsync();

            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetDepartmentOptionByCompanyAsync(int CompanyId, CancellationToken ct = default)
      {
            return await context.Departments.AsNoTracking()
            .Where(x => x.company_id == CompanyId)
            .Select(x => new OptionDto(
                  x.name,
                  x.id,
                  string.Empty,
                  x.component_id,
                  false
                  )).ToArrayAsync();
      }

      public async Task<Pagination<DepartmentDto>> GetDepartmentPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Departments.AsNoTracking().Where(x => x.location_id == param.locationId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.name, pattern) ||
                                  EF.Functions.ILike(x.description, pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.description.Contains(search) 
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
            .Select(u => new DepartmentDto(
                  u.id,
                  u.name,
                  u.description,
                  u.company_id,
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<DepartmentDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }

      public async Task<Pagination<PositionDto>> GetPositionByDepartmentAsync(PaginationParams param, int departmentId, CancellationToken ct = default)
      {
            var query = context.Positions.AsNoTracking().Where(x => x.location_id == param.locationId && x.department_id == departmentId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.name, pattern) ||
                                  EF.Functions.ILike(x.description, pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.description.Contains(search) 
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
            .Select(u => new PositionDto(
                  u.id,
                  u.name,
                  u.description,
                  u.department_id,
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<PositionDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }

      public async Task<IEnumerable<OptionDto>> GetPositionOptionByDepartmentAsync(int DepartmentId, CancellationToken ct = default)
      {
            return await context.Positions.AsNoTracking()
            .Where(x => x.department_id == DepartmentId)
            .Select(x => new OptionDto(
                  x.name,
                  x.id,
                  string.Empty,
                  x.component_id,
                  false
                  )).ToArrayAsync();
      }

      public async Task<Pagination<PositionDto>> GetPositionPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Positions.AsNoTracking().Where(x => x.location_id == param.locationId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.name, pattern) ||
                                  EF.Functions.ILike(x.description, pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.description.Contains(search) 
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
            .Select(u => new PositionDto(
                  u.id,
                  u.name,
                  u.description,
                  u.department_id,
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<PositionDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }

      public async Task<IEnumerable<CredentialDto>> GetCredentialByGroupListAsync(List<int> Groups, CancellationToken ct = default)
      {
            return await context.UserGroups.AsNoTracking()
            .Where(x => Groups.Contains(x.group_id))
            .SelectMany(x => x.user.credentials.Select(c => new CredentialDto(
                  c.id,
                  c.flag,
                  c.bits,
                  c.fac,
                  c.card_number,
                  c.issue_code,
                  c.pin,
                  c.use_count,
                  c.apb_loc,
                  c.act_time,
                  c.deact_time,
                  c.location_id,
                  c.is_active
            )))
            .ToArrayAsync();
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

      public async Task<Pagination<UserDto>> GetUserPaginationAsync(PaginationParams param, CancellationToken ct = default)
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
                                  EF.Functions.ILike(x.user_id, pattern) ||
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
                                  x.user_id.Contains(search) ||
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
            .Select(u => new UserDto(
                  u.id,
                  u.user_id,
                  u.title,
                  u.first_name,
                  u.middle_name,
                  u.last_name,
                  u.gender,
                  u.date_of_birth,
                  u.email,
                  u.phone,
                  u.company_id,
                  u.company.name,
                  u.department_id,
                  u.department.name,
                  u.position_id,
                  u.position.name,
                  u.address,
                  u.additionals.Select(s => s.additional).ToList(),
                  u.image,
                  u.credentials.Select(c => new CredentialDto(
                        c.id,
                        c.flag,
                        c.bits,
                        c.fac,
                        c.card_number,
                        c.issue_code,
                        c.pin,
                        c.use_count,
                        c.apb_loc,
                        c.act_time,
                        c.deact_time,
                        c.location_id,
                        c.is_active
                  )).ToList(),
                  u.user_groups.Select(ug => ug.group_id).ToList(),
                  u.location_id,
                  u.vacation_id ?? 0,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<UserDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }

      public async Task<bool> IsAnyCardNumberAsync(int CardNumber, CancellationToken ct = default)
      {
            return await context.Credentials.AsNoTracking().AnyAsync(x => x.card_number == CardNumber);
      }

      public async Task<bool> IsAnyCompanyByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Companies.AnyAsync(x => x.id == id, ct);
      }

      public async Task<bool> IsAnyDepartmentByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Departments.AnyAsync(x => x.id == id, ct);
      }

      public async Task<bool> IsAnyPositionByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Positions.AnyAsync(x => x.id == id, ct);
      }

      public async Task<bool> IsAnyUserByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Users.AnyAsync(x => x.id == id, ct);
      }

      public async Task<bool> IsAnyUserByUserIdAsync(string userid, CancellationToken ct = default)
      {
            return await context.Users.AnyAsync(x => x.user_id == userid, ct);
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

      public async Task<CompanyDto> UpdateCompanyAsync(Domain.Entities.Company company, CancellationToken ct = default)
      {
            var entity = await context.Companies.OrderByDescending(x => x.id)
            .Where(x => x.id == company.Id)
            .FirstOrDefaultAsync();

            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(company);

            var data = context.Companies.Update(entity);
            var save = await context.SaveChangesAsync();

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

            return new CompanyDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.address,
                  data.Entity.description,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
            
      }

      public async Task<DepartmentDto> UpdateDepartmentAsync(Domain.Entities.Department department, CancellationToken ct = default)
      {
            var entity = await context.Departments.OrderByDescending(x => x.id)
            .Where(x => x.id == department.Id)
            .FirstOrDefaultAsync();

            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(department);

            var data = context.Departments.Update(entity);
            var save = await context.SaveChangesAsync();

            if (data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

            return new DepartmentDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.description,
                  data.Entity.company_id,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }

      public async Task UpdateImagePathAsync(string path, string userid, CancellationToken ct = default)
      {
            var entity = await context.Users.FirstOrDefaultAsync(x => x.user_id.Equals(userid), ct);
            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.image = path;
            var data = context.Users.Update(entity);
            var save = await context.SaveChangesAsync(ct);
            
            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

      }

      public async Task<PositionDto> UpdatePositionAsync(Domain.Entities.Position position, CancellationToken ct = default)
      {
            var entity = await context.Positions.OrderByDescending(x => x.id)
            .Where(x => x.id == position.Id)
            .FirstOrDefaultAsync();

            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(position);

            var data = context.Positions.Update(entity);
            var save = await context.SaveChangesAsync();

            if (data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

            return new PositionDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.description,
                  data.Entity.department_id,
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }

      public async Task<UserDto> UpdateUserAsync(Domain.Entities.Users user, CancellationToken ct = default)
      {

            var entity = await context.Users
                        .Include(x => x.credentials)
                        .Include(x => x.additionals)
                        .Include(x => x.user_groups)
                        .FirstOrDefaultAsync(x => x.id == user.Id, ct);

            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(user);

            context.Credentials.RemoveRange(entity.credentials);
            context.UserAdditionals.RemoveRange(entity.additionals);
            context.UserGroups.RemoveRange(entity.user_groups);

            entity.credentials = user.Credentials
                  .Select(x => new Persistences.Entities.Credential(x))
                  .ToList();

            entity.additionals = user.Additionals
                  .Select(x => new UserAdditional( x))
                  .ToList();

            entity.user_groups = user.Groups
            .Select(x => new UserGroup(
                  x,
                  user.LocationId,
                  true))
            .ToList();


            var data = context.Users.Update(entity);
            var save = await context.SaveChangesAsync();

            if (data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);


            return await context.Users.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.id == data.Entity.id && x.user_id == data.Entity.user_id)
            .Select(x => new UserDto(
                  x.id,
                  x.user_id,
                  x.title,
                  x.first_name,
                  x.middle_name,
                  x.last_name,
                  x.gender,
                  x.date_of_birth,
                  x.email,
                  x.phone,
                  x.company_id,
                  x.company.name,
                  x.department_id,
                  x.department.name,
                  x.position_id,
                  x.position.name,
                  x.address,
                  x.additionals.Select(x => x.additional).ToList(),
                  x.image,
                  x.credentials.Select(c => new CredentialDto(
                        c.id,
                        c.flag,
                        c.bits,
                        c.fac,
                        c.card_number,
                        c.issue_code,
                        c.pin,
                        c.use_count,
                        c.apb_loc,
                        c.act_time,
                        c.deact_time,
                        c.location_id,
                        c.is_active
                  )).ToList(),
                  x.user_groups.Select(g => g.group_id).ToList(),
                  x.vacation_id ?? 0,
                  x.location_id,
                  x.is_active
            ))
            .FirstOrDefaultAsync() ?? new UserDto();
      }

      private async Task<bool> UpdateAdditionalAsync(int UserId,List<string> NewAdditional)
      {
            var additionals = await context.UserAdditionals
            .OrderByDescending(x => x.id)
            .Where(x => x.user_id == UserId)
            .ToListAsync();

            // Remove 
            var remove = additionals.Where(x => !NewAdditional.Contains(x.additional)).ToArray();
            // Dup
            var duplicate = NewAdditional.Where(x => additionals.Any(a => x.Equals(a.additional))).ToArray();
            // Add
            var add = NewAdditional.Where(x => duplicate.Any(d => !x.Equals(d)) && remove.Any(r => !r.additional.Equals(x))).Select(x => new UserAdditional(x)).ToArray();
            
            
            context.UserAdditionals.RemoveRange(remove);
            await context.UserAdditionals.AddRangeAsync(add);  
            var save = await context.SaveChangesAsync();

            if(save <= 0)
                  return false;  

            return true;
      }


}