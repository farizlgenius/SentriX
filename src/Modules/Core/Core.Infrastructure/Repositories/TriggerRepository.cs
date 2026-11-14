using Core.Application.Interfaces;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Trigger;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Persistences.Entities;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class TriggerRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Trigger,Domain.Entities.Trigger,TriggerDto>(
      context,
      d => new Persistences.Entities.Trigger(d),
      (en,dom) =>
      {
            en.name = dom.Name;
            en.command = dom.Command;
            en.src_device_module_id = dom.SrcDeviceModuleId;
            en.src_input_id = dom.SrcInputId;
            en.src_output_id = dom.SrcOutputId;
            en.src_door_id = dom.SrcDoorId;
            en.src_timezone_id = dom.SrcTimeZoneId;
            en.src_monitor_group_id = dom.SrcMonitorGroupId;
            en.src_area_id = dom.SrcAreaId;
            en.device_id = dom.DeviceId;
            en.procedure_id = dom.ProcedureId;
            en.trigger_source_id = dom.TriggerSourceId;
            en.trigger_type_id  = dom.TriggerTypeId;
            en.timezone_id = dom.TimeZoneId;
            en.trigger_map_trigger_codes = [.. dom.TriggerCodeIds.Select(x => new TriggerMapTriggerCode(x))];
      },
      TriggerProjections.ToDto,
      TriggerProjections.ToCommand
),ITriggerRepository
{
   

      public async Task<IEnumerable<OptionDto>> GetCodeAsync(Guid type, CancellationToken ct = default)
      {
            return await Context.TriggerCodes
                  .AsNoTracking()
                  .Where(x => x.trigger_type == null || x.trigger_type.guid == type)
                  .Select(x => new OptionDto(
                        x.name,
                        x.value,
                        x.guid.ToString()
                  )).ToArrayAsync(ct);
      }

    
      public async Task<Pagination<TriggerDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
                  param,
                  param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
                  [x => x.name]
                  ,ct);
      }



      public async Task<IEnumerable<OptionDto>> GetSourceAsync(CancellationToken ct = default)
      {
            return await Context.TriggerSources
                  .AsNoTracking()
                  .Select(x => new OptionDto(
                        x.name,
                        x.value,
                        x.guid.ToString()
                  )).ToArrayAsync(ct);
      }

      public async Task<IEnumerable<OptionDto>> GetTypeAsync(Guid source, CancellationToken ct = default)
      {
            return await Context.TriggerTypes
                  .AsNoTracking()
                  .Where(x => x.trigger_source == null || x.trigger_source.guid == source)
                  .Select(x => new OptionDto(
                        x.name,
                        x.value,
                        x.guid.ToString()
                  )).ToArrayAsync(ct);
      }

      public override async Task UpdateAsync(Domain.Entities.Trigger dom,CancellationToken ct = default)
      {
             await using var transaction = await Context.Database.BeginTransactionAsync(ct);
            try
            {
                  var en = await Context.Triggers.Where(x => x.guid == dom.Guid).FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.TRIGGER.ToString(),dom.Guid.ToString());

                  en.name = dom.Name;
                  en.command = dom.Command;
                  en.src_device_module_id = dom.SrcDeviceModuleId;
                  en.src_input_id = dom.SrcInputId;
                  en.src_output_id = dom.SrcOutputId;
                  en.src_door_id = dom.SrcDoorId;
                  en.src_timezone_id = dom.SrcTimeZoneId;
                  en.src_monitor_group_id = dom.SrcMonitorGroupId;
                  en.src_area_id = dom.SrcAreaId;
                  en.device_id = dom.DeviceId;
                  en.procedure_id = dom.ProcedureId;
                  en.trigger_source_id = dom.TriggerSourceId;
                  en.trigger_type_id  = dom.TriggerTypeId;
                  en.timezone_id = dom.TimeZoneId;
                  
                  var codes = await Context.TriggerMapTriggerCodes.Where(x => x.trigger_id == en.id).ToArrayAsync(ct);

                  Context.TriggerMapTriggerCodes.RemoveRange(codes);

                  en.trigger_map_trigger_codes.Clear();

                  en.trigger_map_trigger_codes = [.. dom.TriggerCodeIds.Select(x => new TriggerMapTriggerCode(x))];

                  // Commit updates
                  await Context.SaveChangesAsync(ct);
                  await transaction.CommitAsync(ct);
            }
            catch
            {
                  await Context.Database.RollbackTransactionAsync(ct);
                  throw;
            }
      }



      public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<(int id,short value)> GetSourceIdAndValueByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await Context.TriggerSources
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new
                  {
                        x.id,
                        x.value
                  })
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException("Trigger Source",guid.ToString());

            return (res.id,res.value);
      }

      public async Task<(int id,short value)> GetTypeIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await Context.TriggerTypes
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new
                  {
                        x.id,
                        x.value
                  })
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException("Trigger Type",guid.ToString());

            return (res.id,res.value);
      }

      public async Task<IEnumerable<(int id,short value)>> GetCodeIdsAndVeluesByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            var res = await Context.TriggerCodes
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .Select(x => new {x.id,x.value})
                  .ToArrayAsync(ct);

            return [.. res.Select(x => (x.id,x.value))];
      }

      public async Task<IEnumerable<(int id, short value)>> GetArgIdsAndVeluesByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            var res = await Context.TriggerArgs
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .Select(x => new {x.id,x.value})
                  .ToArrayAsync(ct);

            return [.. res.Select(x => (x.id,x.value))];
      }

      public async Task<IEnumerable<(int id, short externald)>> GetTrigVarIdsAndExternalIdsByGuidsAndDeviceGuidAsync(IEnumerable<Guid> guids, Guid guid, CancellationToken ct = default)
      {
           var res = await Context.TriggerVariables
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid) && x.device.guid == guid)
                  .Select(x => new {x.id,x.external_id})
                  .ToArrayAsync(ct);

            return [.. res.Select(x => (id:x.id,externald:x.external_id ?? 0))];
      }

      public async Task<(int id, short externaId)> GetTrigVarIdAndExternalIdByGuideAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await Context.TriggerVariables
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new { x.id, x.external_id })
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException("Trigger Variable",guid.ToString());

            return (res.id,res.external_id ?? 0);
      }
}