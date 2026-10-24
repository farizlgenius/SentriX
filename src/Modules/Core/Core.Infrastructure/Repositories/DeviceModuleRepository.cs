using Core.Application.Interfaces;
using Core.Contract.DTOs;
using Core.Contract.DTOs.DeviceModule;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class DeviceModuleRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.DeviceModule,Domain.Entities.DeviceModule,DeviceModuleDto>(
      context,
      d => new Persistences.Entities.DeviceModule(d),
      (en,dom) =>
      {
            en.name = dom.Name;
            en.serial_number = dom.SerialNumber;
            en.firmware = dom.Firmware;
            en.mac = dom.Mac;
            en.port = dom.Port;
            en.address = dom.Address;
            en.model = dom.Model;
            en.reader_slot = dom.ReaderSlot;
            en.input_slot = dom.InputSlot;
            en.output_slot = dom.OutputSlot;
            en.updated_at = DateTime.UtcNow;
      },
      DeviceModuleProjections.ToDto,
      DeviceModuleProjections.ToCommand
),IDeviceModuleRepository
{
     



    

      public async Task<IEnumerable<DeviceModuleDto>> GetByDeviceAsync(Guid guid, CancellationToken ct = default)
      {
           return await Context.DeviceModules
            .AsNoTracking()
            .Where(x => x.device.guid == guid)
            .Select(x => new DeviceModuleDto(
                        x.guid,
                        x.name,
                        x.serial_number,
                        x.firmware,
                        x.mac,
                        x.port,
                        x.address,
                        x.model,
                        x.reader_slot,
                        x.output_slot,
                        x.input_slot,
                        x.device.guid,
                        x.device.name,
                        x.location.guid,
                        x.location.name,
                        x.is_active,
                        x.is_default
                  )).ToArrayAsync(ct);
      }

    

      public async Task<IEnumerable<DeviceModuleDto>> GetByVendorAndLocationAsync(int locationId, SharedKernel.Enums.Vendor vedor, CancellationToken ct = default)
      {
            return await Context.DeviceModules
                  .AsNoTracking()
                  .Where(x => x.device.vendor == vedor && x.location_id == locationId)
                  .Select(x => new DeviceModuleDto(
                        x.guid,
                        x.name,
                        x.serial_number,
                        x.firmware,
                        x.mac,
                        x.port,
                        x.address,
                        x.model,
                        x.reader_slot,
                        x.output_slot,
                        x.input_slot,
                        x.device.guid,
                        x.device.name,
                        x.location.guid,
                        x.location.name,
                        x.is_active,
                        x.is_default
                  )).ToArrayAsync();
      }



      public async Task<Dictionary<Guid, int>> GetDeviceModuleIdsMapGuidsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await Context.DeviceModules
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .ToDictionaryAsync(x => x.guid, x => x.id, ct);
      }

    

      public async Task<(int TotalSlots, List<int> InputSlots,List<int> RexSlots,List<int> SensorSlots,List<int> BreakGlassSlots)?> GetInputSlotAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await Context.DeviceModules
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new
                  {
                        TotalSlots = x.input_slot,
                        InputSlots = x.inputs.Select(x => x.slot_no).ToList(),
                        RexSlots = x.rexes.Select(x => x.slot_no).ToList(),
                        SensorSlots = x.sensors.Select(x => x.slot_no).ToList(),
                        BreakGlassSlots = x.break_glasses.Select(x => x.slot_no).ToList()
                  }).FirstOrDefaultAsync();

            if(res == null)
                  return null;

            return (res.TotalSlots,res.InputSlots,res.RexSlots,res.SensorSlots,res.BreakGlassSlots);
      }

   

      public async Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid, CancellationToken ct = default)
      {
           return await Context.DeviceModules
            .AsNoTracking()
            .Where(x => x.device.guid == guid)
            .OrderByDescending(x => x.id)
            .Select(x => new OptionDto(
                  x.name,
                  x.guid,
                  x.mac,
                  null,
                  false
            )).ToArrayAsync();
      }

      public async Task<(int TotalSlots, List<int> OutputSlots,List<int> BuzzerSlots,List<int> RelaySlots)?> GetOutputSlotAsync(Guid guid,CancellationToken ct = default)
      {
            var res = await Context.DeviceModules
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new
                  {
                        TotalSlots = x.output_slot,
                        OutputSlots = x.outputs.Select(x => x.slot_no).ToList(),
                        BuzzerSlots = x.buzzers.Select(x => x.slot_no).ToList(),
                        RelaySlots = x.relays.Select(x => x.slot_no).ToList()
                  }).FirstOrDefaultAsync();

            if(res == null)
                  return null;

            return (res.TotalSlots,res.OutputSlots,res.BuzzerSlots,res.RelaySlots);
      }

      public async Task<Pagination<DeviceModuleDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
                  param,
                  param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
                  [x => x.name,x => x.serial_number,x => x.firmware,x => x.mac,x => x.port.ToString(),x => x.address.ToString(),x => x.model.ToString()],
                  ct
            );
         
      }

      public async Task<(int TotalSlots, List<int> OccupiedSlots)?> GetReaderSlotAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await Context.DeviceModules
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new
                  {
                        TotalSlots = x.reader_slot,
                        OccupiedSlots = x.readers.Select(x => x.slot_no).ToList()
                  }).FirstOrDefaultAsync();

            if(res == null)
                  return null;

            return (res.TotalSlots,res.OccupiedSlots);
      }


  

      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

    
}