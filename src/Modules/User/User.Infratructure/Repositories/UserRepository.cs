using System.Text;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using User.Application.Interfaces;
using User.Contract.DTOs;
using User.Domain.Entities;
using User.Infratructure.Persistences;
using User.Infratructure.Persistences.Entities;

namespace User.Infratructure.Repositories;

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
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }

      public async Task<UserDto> CreateUserAsync(Domain.Entities.Users dto, CancellationToken ct = default)
      {
            var entity = new Persistences.Entities.Users(dto);
            var data = await context.Users.AddAsync(entity, ct);

            // Add Additional
            await context.UserAdditionals.AddRangeAsync(
                  dto.additionals.Select(
                        x => new UserAdditional(
                              data.Entity.id,
                              x
                              )
                  )
            );

            // Credential

            await context.Credentials.AddRangeAsync(
                  dto.credentials.Select(x => new Persistences.Entities.Credential(
                        x
                  )).ToList()
            );

            // User Group


            await context.UserGroups.AddRangeAsync(
                  dto.user_groups.Select(x => new Persistences.Entities.UserGroup(
                        entity.id,
                        x,
                        entity.location_id,
                        true
                  )).ToList()
            );

            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            var res = await context.Users.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Include(x => x.credentials)
            .Include(x => x.additionals)
            .Include(x => x.user_groups)
            .Where(x => x.id == data.Entity.id && x.user_id == data.Entity.user_id)
            .FirstOrDefaultAsync();

            if(res == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            return new UserDto(
                  res.id,
                  res.user_id,
                  res.title,
                  res.first_name,
                  res.middle_name,
                  res.last_name,
                  res.gender,
                  res.date_of_birth,
                  res.email,
                  res.phone,
                  res.company_id,
                  res.department_id,
                  res.position_id,
                  res.address,
                  res.additionals.Select(x => x.additional).ToList(),
                  res.image,
                  res.credentials.Select(c => new CredentialDto(
                        c.id,
                        c.flag,
                        c.card_number,
                        c.issue_code,
                        c.pin,
                        c.use_count,
                        c.act_time,
                        c.deact_time,
                        c.location_id,
                        c.is_active
                  )).ToList(),
                  res.user_groups.Select(x => x.group_id).ToList(),
                  res.location_id,
                  res.is_active
            );
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
                  entity.location_id,
                  entity.is_active
            );
      }

      public async Task<UserDto> DeleteUserAsync(int id, CancellationToken ct = default)
      {
            throw new NotImplementedException();
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
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<DepartmentDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
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
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<PositionDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
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
                  u.department_id,
                  u.position_id,
                  u.address,
                  u.additionals.Select(s => s.additional).ToList(),
                  u.image,
                  u.credentials.Select(c => new CredentialDto(
                        c.id,
                        c.flag,
                        c.card_number,
                        c.issue_code,
                        c.pin,
                        c.use_count,
                        c.act_time,
                        c.deact_time,
                        c.location_id,
                        c.is_active
                  )).ToList(),
                  u.user_groups.Select(ug => ug.group_id).ToList(),
                  u.location_id,
                  u.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<UserDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
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
                  data.Entity.location_id,
                  data.Entity.is_active
            );
      }

      public async Task<UserDto> UpdateUserAsync(Domain.Entities.Users user, CancellationToken ct = default)
      {

            var entity = await context.Users.OrderByDescending(x => x.id)
            .Where(x => x.id == user.Id)
            .FirstOrDefaultAsync();

            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            // Additional
            var additionals = await context.UserAdditionals
            .OrderByDescending(x => x.id)
            .Where(x => x.user_id == user.Id)
            .ToListAsync();

            context.UserAdditionals.RemoveRange(additionals);

            await context.UserAdditionals.AddRangeAsync(
                  user.additionals.Select(x => new UserAdditional(
                        entity.id,
                        x
                  )).ToList()
            );
            
            // Credential
            var credential = await context.Credentials
            .OrderByDescending(x => x.id)
            .Where(x => x.user_id == user.Id)
            .ToListAsync();

            context.Credentials.RemoveRange(credential);

            await context.Credentials.AddRangeAsync(
                  user.credentials.Select(x => new Persistences.Entities.Credential(
                        x
                  )).ToList()
            );

            // User Group
            var group = await context.UserGroups
            .OrderByDescending(x => x.id)
            .Where(x => x.user_id == user.Id)
            .ToListAsync();

            context.UserGroups.RemoveRange(group);

            await context.UserGroups.AddRangeAsync(
                  user.user_groups.Select(x => new Persistences.Entities.UserGroup(
                        user.Id,
                        x,
                        user.LocationId,
                        true
                  )).ToList()
            );

            entity.Update(user);

            var data = context.Users.Update(entity);
            var save = await context.SaveChangesAsync();

            if (data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

            var res = await context.Users.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Include(x => x.credentials)
            .Include(x => x.additionals)
            .Include(x => x.user_groups)
            .Where(x => x.id == data.Entity.id && x.user_id == data.Entity.user_id)
            .FirstOrDefaultAsync();

            if(res == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            return new UserDto(
                  res.id,
                  res.user_id,
                  res.title,
                  res.first_name,
                  res.middle_name,
                  res.last_name,
                  res.gender,
                  res.date_of_birth,
                  res.email,
                  res.phone,
                  res.company_id,
                  res.department_id,
                  res.position_id,
                  res.address,
                  res.additionals.Select(x => x.additional).ToList(),
                  res.image,
                  res.credentials.Select(c => new CredentialDto(
                        c.id,
                        c.flag,
                        c.card_number,
                        c.issue_code,
                        c.pin,
                        c.use_count,
                        c.act_time,
                        c.deact_time,
                        c.location_id,
                        c.is_active
                  )).ToList(),
                  res.user_groups.Select(x => x.group_id).ToList(),
                  res.location_id,
                  res.is_active
            );
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
            var add = NewAdditional.Where(x => duplicate.Any(d => !x.Equals(d)) && remove.Any(r => !r.additional.Equals(x))).Select(x => new UserAdditional(UserId,x)).ToArray();
            
            
            context.UserAdditionals.RemoveRange(remove);
            await context.UserAdditionals.AddRangeAsync(add);  
            var save = await context.SaveChangesAsync();

            if(save <= 0)
                  return false;  

            return true;
      }


}