using Adapter.Aero.Interfaces;
using Adapter.Aero.Model;
using Adapter.Aero.Persistences;
using Adapter.Aero.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Adapter.Aero.Repositories;

public sealed class AeroRepository(AeroDbContext context) : IAeroRepository
{
      public async Task<AccessDatabaseSpecification> GetAccessDatabaseSpecificationAsync(CancellationToken ct = default)
      {
            return await context.AccessDatabaseSpecifications.AsNoTracking()
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync() ?? new AccessDatabaseSpecification();
      }

      public async Task<ElevatorAccessLevelSpecification> GetElevatorAccessLevelSpecificationAsync(CancellationToken ct = default)
      {
            return await context.ElevatorAccessLevelSpecifications.AsNoTracking()
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync() ?? new ElevatorAccessLevelSpecification();
      }

      public async Task<IEnumerable<OptionDto>> GetRelayOptionAsync(CancellationToken ct = default)
      {
            return await context.RelayModes.AsNoTracking().Select(x => new OptionDto(
                  x.label,
                  x.value,
                  string.Empty,
                  Guid.Empty,
                  false
            )).ToArrayAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetTimezoneModeAsync(CancellationToken ct = default)
      {
            return await context.TimezoneModes.AsNoTracking().Select(x => new OptionDto(
                  x.label,
                  x.value,
                  x.description,
                  Guid.Empty,
                  false
            )).ToArrayAsync();
      }

      public async Task<ScpDeviceSpecification> GetScpDeviceSpecificationAsync(CancellationToken ct = default)
      {
            return await context.ScpDeviceSpecifications.AsNoTracking()
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync() ?? new ScpDeviceSpecification();
      }


      // Slot Allocate

      public async Task AddSlotAsync<TEntity>(
            Guid guid,
            int slot,
             Func<Guid, int, TEntity> factory, 
            CancellationToken ct = default
            ) where TEntity : BaseSlot
      {
            var entity = factory(guid, slot);

            await context.Set<TEntity>().AddAsync(entity, ct);
            await context.SaveChangesAsync(ct);
      }



      public async Task InsertSlotAsync<TEntity>(
            Guid device_guid,
            Guid module_guid,
            int slot,
            CancellationToken ct = default
            ) where TEntity : BaseSlot
      {
            var e = await context.Set<TEntity>()
            .Where(x => x.device_guid == device_guid && x.slot_id == slot)
            .FirstOrDefaultAsync();

             if(e == null)
                  throw new Exception(MessageHelper.Common.NotFound("Slot",slot));

            e.Inserted(module_guid);
            context.Set<TEntity>().Update(e);
            await context.SaveChangesAsync();
      }

      public async Task EjectSlotAsync<TEntity>(
            int slot,
            CancellationToken ct = default
            ) where TEntity : BaseSlot
      {
            var e = await context.Set<TEntity>()
            .Where(x => x.slot_id == slot)
            .FirstOrDefaultAsync();

             if(e == null)
                  throw new Exception(MessageHelper.Common.NotFound("Slot",slot));

            e.Ejected();
            context.Set<TEntity>().Update(e);
            await context.SaveChangesAsync(ct);
      }



      public async Task EjectSlotAsync<TEntity>(
            Guid deviceGuid,
            int slot,
            CancellationToken ct = default
            ) where TEntity : BaseSlot
      {
            var e = await context.Set<TEntity>()
            .Where(x => x.device_guid == deviceGuid && x.slot_id == slot)
            .FirstOrDefaultAsync();

             if(e == null)
                  throw new Exception(MessageHelper.Common.NotFound("Slot",slot));

            e.Ejected();
            context.Set<TEntity>().Update(e);
            await context.SaveChangesAsync(ct);
      }



      public async Task DeleteSlotAsync<TEntity>(Guid guid, CancellationToken ct = default) where TEntity : BaseSlot
      {
            var e = await context.Set<TEntity>()
            .Where(x => x.device_guid == guid)
            .ToArrayAsync(ct);

            context.Set<TEntity>().RemoveRange(e);
            await context.SaveChangesAsync(ct);
      }

      public async Task<int> GetFreeSlotAsync<TEntity>(Guid guid,short Except = -1, CancellationToken ct = default) where TEntity : BaseSlot
      {
            return await context.Set<TEntity>()
            .AsNoTracking()
            .OrderByDescending(x => x.slot_id)
            .Where(x => x.device_guid == guid && x.is_available == true && x.slot_id != Except)
            .Select(x => x.slot_id)
            .FirstOrDefaultAsync();
      }

      public async Task<int> GetScpFreeSlotAsync(CancellationToken ct = default)
      {
            return await context.ScpSlots.AsNoTracking()
            .OrderByDescending(x => x.slot_id)
            .Where(x => x.is_available == true)
            .Select(x => x.slot_id)
            .FirstOrDefaultAsync();
      }

      public async Task<int> GetScpSlotByMacAsync(string mac, CancellationToken ct = default)
      {
           return await context.ScpSlots.AsNoTracking()
           .OrderByDescending(x => x.id)
           .Where(x => x.mac.Equals(mac))
           .Select(x => x.slot_id)
           .FirstOrDefaultAsync();
      }

      public async Task InsertScpSlotAsync(Guid guid, string mac,int slot,CancellationToken ct = default)
      {
            var e = await context.ScpSlots
            .Where(x => x.slot_id == slot)
            .FirstOrDefaultAsync(ct);

            if(e is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            e.Inserted(guid,mac);

            context.ScpSlots.Update(e);
            await context.SaveChangesAsync(ct);
      }

      public async Task EjectScpSlotAsync(Guid guid,CancellationToken ct = default)
      {
            var e = await context.ScpSlots
            .Where(x => x.component_guid == guid)
            .FirstOrDefaultAsync(ct);

            if(e is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            e.Ejected();

            context.ScpSlots.Update(e);
            await context.SaveChangesAsync(ct);
      }

      public async Task<ScpSlot> GetScpSlotByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.ScpSlots.AsNoTracking()
            .Where(x => x.component_guid == guid)
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync() ?? new ScpSlot();
      }

      public async Task<TEntity> GetSlotByGuidAsync<TEntity>(Guid guid, CancellationToken ct = default) where TEntity : BaseSlot
      {
            return (TEntity)(await context.Set<TEntity>()
            .AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.component_guid == guid)
            .FirstOrDefaultAsync() ?? new BaseSlot());
      }



      public async Task<Guid> GetScpGuidBySlotAsync(int slot, CancellationToken ct = default)
      {
           return await context.ScpSlots.AsNoTracking()
           .Where(x => x.slot_id == slot)
           .Select(x => x.component_guid)
           .FirstOrDefaultAsync() ?? Guid.Empty;
      }



 
      public async Task<IEnumerable<TEntity>> GetSlotsByGuidAsync<TEntity>(Guid guid, CancellationToken ct = default) where TEntity : BaseSlot
      {
           return await context.Set<TEntity>()
            .AsNoTracking()
            .Where(x => x.component_guid == guid)
            .ToArrayAsync();
      }

      public async Task<int> GetSlotIdByGuidAsync<TEntity>(Guid guid, CancellationToken ct = default) where TEntity : BaseSlot
      {
            return await context.Set<TEntity>()
            .Where(x => x.component_guid == guid)
            .Select(x => x.slot_id)
            .FirstOrDefaultAsync();
      }

      public async Task<IEnumerable<int>> GetSlotIdsByGuidAsync<TEntity>(Guid guid, CancellationToken ct = default) where TEntity : BaseSlot
      {
            return await context.Set<TEntity>()
            .Where(x => x.component_guid == guid)
            .Select(x => x.slot_id)
            .ToArrayAsync();
      }

      public async Task<int> GetFreeSlotAsync<TEntity>(short Except = -1, CancellationToken ct = default) where TEntity : BaseSlot
      {
            var res = await context.Set<TEntity>()
            .AsNoTracking()
            .OrderByDescending(x => x.slot_id)
            .Where(x => x.is_available == true && x.slot_id != Except)
            .Select(x => x.slot_id)
            .DefaultIfEmpty(-1)
            .FirstAsync();

            if(res == -1)
                  throw new Exception(MessageHelper.Common.SlotNotAvailable(nameof(TEntity)));

            return res;
      }

      public async Task InsertSlotAsync<TEntity>(Guid module_guid, int slot, CancellationToken ct = default) where TEntity : BaseSlot
      {
            var e = await context.Set<TEntity>()
            .Where(x =>  x.slot_id == slot)
            .FirstOrDefaultAsync();

             if(e == null)
                  throw new Exception(MessageHelper.Common.NotFound("Slot",slot));

            e.Inserted(module_guid);
            context.Set<TEntity>().Update(e);
            await context.SaveChangesAsync();
      }
}