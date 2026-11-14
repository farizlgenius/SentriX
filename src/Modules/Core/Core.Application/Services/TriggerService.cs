using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Trigger;
using Core.Contract.Interfaces;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class TriggerService(
      IDeviceModuleRepository module,
      IInputRepository input,
      IOutputRepository output,
      IDoorRepository door,
      ITimeRepository time,
      IMonitorGroupRepository monitorGroup,
      IAreaRepository area,
      ITriggerRepository repo,
      IDeviceRepository device,
      IAdapterFactory adapter,
      IProcedureRepository proc
      ) : BaseService<TriggerDto,Domain.Entities.Trigger>(
      repo
),ITrigger
{
      public async Task<Guid> CreateAsync(CreateTriggerDto dto, CancellationToken ct = default)
      {
            var dev = await device.GetCommandDetailAsync(dto.DeviceGuid,ct);

            var externalId = await Repo.GetFreeExternalIdAsync(100,[],ct);

            var (id, value) = await repo.GetSourceIdAndValueByGuidAsync(dto.TriggerSource,ct);

            var type = await repo.GetTypeIdByGuidAsync(dto.TriggerType,ct);

            var codes = await repo.GetCodeIdsAndVeluesByGuidsAsync(dto.TriggerCodes,ct);

            var args = await repo.GetArgIdsAndVeluesByGuidsAsync(dto.TriggerArgs,ct);

            (int id,short externalId)? FirstVars = dto.FirstTriggerVariable == null ? null : await repo.GetTrigVarIdAndExternalIdByGuideAsync(dto.FirstTriggerVariable ?? Guid.Empty,ct);
            (int id,short externalId)? SecondVars = dto.SecondTriggerVariable == null ? null : await repo.GetTrigVarIdAndExternalIdByGuideAsync(dto.SecondTriggerVariable ?? Guid.Empty,ct);

            (int id,short externalId)? ThirdVars = dto.ThirdTriggerVariable == null ? null : await repo.GetTrigVarIdAndExternalIdByGuideAsync(dto.ThirdTriggerVariable ?? Guid.Empty,ct);
            (int id,short externalId)? ForthVars = dto.ForthTriggerVariable == null ? null : await repo.GetTrigVarIdAndExternalIdByGuideAsync(dto.ForthTriggerVariable ?? Guid.Empty,ct);


            var timeId = await time.GetIdAndExternalIdByGuidAsync(dto.TimeZoneGuid,ct);

            var procedure = await proc.GetIdAndExternalIdByGuidAsync(dto.PrcedureGuid,ct);

            (int id,short externalId)? srcDetail = null;

            switch (dto.SrcType)
            {
                  case SharedKernel.Enums.EntityType.DEVICE:
                  srcDetail = (dev.DeviceId,dev.DeviceExternalId);
                  break;
                  case SharedKernel.Enums.EntityType.DEVICE_MODULE:
                  srcDetail = await module.GetIdAndExternalIdByGuidAsync(dto.SrcGuid,ct);
                  break;
                  case SharedKernel.Enums.EntityType.INPUT:
                  srcDetail = await input.GetIdAndExternalIdByGuidAsync(dto.SrcGuid,ct);
                  break;
                  case SharedKernel.Enums.EntityType.OUTPUT:
                  srcDetail = await output.GetIdAndExternalIdByGuidAsync(dto.SrcGuid,ct);
                  break;
                  case SharedKernel.Enums.EntityType.DOOR:
                  srcDetail = await door.GetIdAndExternalIdByGuidAsync(dto.SrcGuid,ct);
                  break;
                  case SharedKernel.Enums.EntityType.TIMEZONE:
                  srcDetail = await time.GetIdAndExternalIdByGuidAsync(dto.SrcGuid,ct);
                  break;
                  case SharedKernel.Enums.EntityType.MONITOR_GROUP:
                  srcDetail = await monitorGroup.GetIdAndExternalIdByGuidAsync(dto.SrcGuid,ct);
                  break;
                  case SharedKernel.Enums.EntityType.AREA:
                  srcDetail = await area.GetIdAndExternalIdByGuidAsync(dto.SrcGuid,ct);
                  break;
                  default:
                  break;
            }

            var d = new Domain.Entities.Trigger(
                  dto.Name,
                  dev.DeviceId,
                  procedure.id,
                  dto.Command,
                  id,
                  type.id,
                  codes.Select(x => x.id),
                  args.Select(x => x.id),
                  timeId.id,
                  externalId,
                  FirstVars?.id,
                  SecondVars?.id,
                  ThirdVars?.id,
                  ForthVars?.id,
                  dto.SrcType == SharedKernel.Enums.EntityType.DEVICE_MODULE && srcDetail != null ? srcDetail.Value.id : null,
                  dto.SrcType == SharedKernel.Enums.EntityType.INPUT && srcDetail != null ? srcDetail.Value.id : null,
                  dto.SrcType == SharedKernel.Enums.EntityType.OUTPUT && srcDetail != null ? srcDetail.Value.id : null,
                  dto.SrcType == SharedKernel.Enums.EntityType.DOOR && srcDetail != null ? srcDetail.Value.id : null,
                  dto.SrcType == SharedKernel.Enums.EntityType.TIMEZONE && srcDetail != null ? srcDetail.Value.id : null,
                  dto.SrcType == SharedKernel.Enums.EntityType.MONITOR_GROUP && srcDetail != null ? srcDetail.Value.id : null,
                  dto.SrcType == SharedKernel.Enums.EntityType.AREA && srcDetail != null ? srcDetail.Value.id : null
            );

            // Command
            await adapter.GetAdapter(dev.Vendor).Trigger.AddTriggerAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  (short)dev.DeviceId,
                  externalId,
                  dto.Command,
                  procedure.externalId,
                  value,
                  srcDetail == null ? (short)-1 : srcDetail.Value.externalId,
                  type.value,
                  codes.Select(x => x.value),
                  timeId.externalId,
                  [
                        FirstVars == null ? (short)0 : FirstVars.Value.externalId,
                        SecondVars == null ? (short)0 : SecondVars.Value.externalId,
                        ThirdVars == null ? (short)0 : ThirdVars.Value.externalId,
                        ForthVars == null ? (short)0 : ForthVars.Value.externalId,
                  ],
                  [.. args.Select(x => x.value)],
                  ct
            );

            await Repo.AddAsync(d,ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var dev = await Repo.GetCommandDetailAsync(guid,ct);

            var trig = await Repo.GetAsync(guid,ct);

            var procedureExternalId = await Repo.GetExternalIdByGuidAsync(trig.Prcedure.Guid ?? Guid.Empty,ct);


            // Command to send 
            // await adapter.GetAdapter(dev.Vendor).Trigger.DeleteTriggerAsync(
            //       dev.DeviceMac,
            //       dev.DeviceIp,
            //       dev.DeviceId,
            //       dev.ComponentExternalId,
            //       trig.Command,
            //       procedureExternalId,
            //       ,
            //       ,
            //       ,
            //       ,
            //       ,
            //       ct
            // );


            await Repo.DeleteAsync(guid,ct);

            return true;
      }

      public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }


      public async Task<IEnumerable<OptionDto>> GetCodeAsync(Guid type,CancellationToken ct = default)
      {
            return await repo.GetCodeAsync(type,ct);
      }


      public async Task<IEnumerable<OptionDto>> GetSourceAsync(CancellationToken ct = default)
      {
            return await repo.GetSourceAsync(ct);
      }

      public async Task<IEnumerable<OptionDto>> GetTypeAsync(Guid source,CancellationToken ct = default)
      {
            return await repo.GetTypeAsync(source,ct);
      }

      public async Task<Guid> UpdateAsync(UpdateTriggerDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}