using Core.Application.Interfaces;
using Core.Contract.DTOs.Device;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Core.Infrastructure.Repositories;

public sealed class DeviceRepository(CoreDbContext context) : IDeviceRepository
{
      public async Task AddAsync(Device entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<DeviceDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<Guid> GetGuidByMacAsync(string mac, CancellationToken ct = default)
      {
            return await context.Devices
                  .AsNoTracking()
                  .Where(x => x.mac.Equals(mac))
                  .Select(x => x.guid)
                  .FirstOrDefaultAsync();
      }

      public async Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsAnyByNameAndLocationGuidAsync(string name, Guid locationGuid = default, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task UpdateAsync(Device entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}