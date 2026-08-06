using System;
using System.Text.Json;
using Adapter.Abstraction;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Constants;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Contract.Queries;
using Events.Contract.Command;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Adapter.Aero.Adapters;

public sealed class AeroDeviceAdapter(
      ILogger<AeroDeviceAdapter> logger,
      IScpService scp,
      IScpCommand writer, 
      IIdReportService idReport, 
      IModuleCommand sioWriter, 
      IMessageBus bus,
      IAeroRepository repo
      ) : IAeroDeviceAdapter
{
      public async Task<List<IdReportDto>> GetIdReportsAsync()
      {
            return idReport.IdReportInMemory.Select(x => new IdReportDto(x.ScpId, x.SerialNumber, x.Mac, x.Ip, x.Port, x.Fw)).ToList();
      }

      public async Task CreateDeviceAsync(
            Guid Guid,
            string Ip,
            string Mac,
            short ScpId,
            int LocationId
      )
      {

            logger.LogInformation("Create Device {Mac} {ScpId} {LocationId}", Mac, ScpId, LocationId);

            // Read Structure 
            var res = writer.ScpStructureStatusRead(
                  Mac,
                 ScpId,
                  [
                        (short)SCPStructure.SCPSID_TRAN,
                        (short)SCPStructure.SCPSID_TZ,
                        (short)SCPStructure.SCPSID_HOL,
                        (short)SCPStructure.SCPSID_MSP1,
                        (short)SCPStructure.SCPSID_SIO,
                        (short)SCPStructure.SCPSID_MP,
                        (short)SCPStructure.SCPSID_CP,
                        (short)SCPStructure.SCPSID_ACR,
                        (short)SCPStructure.SCPSID_ALVL,
                        (short)SCPStructure.SCPSID_TRIG,
                        (short)SCPStructure.SCPSID_PROC,
                        (short)SCPStructure.SCPSID_MPG,
                        (short)SCPStructure.SCPSID_AREA,
                        (short)SCPStructure.SCPSID_EAL,
                        (short)SCPStructure.SCPSID_CRDB
                  ]
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if (!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ScpStructureStatusRead, Mac, ScpId));


            idReport.IdReportInMemory.RemoveAll(x => x.Mac.Equals(Mac));

            var setting = await repo.GetScpDeviceSpecificationAsync();

            await repo.InsertScpSlotAsync(Guid,Mac,ScpId);

            var sios = Enumerable.Range(1,setting.n_sio-1);
            foreach(var sio in sios)
            {
                  await repo.AddSlotAsync(
                        Guid,
                        sio,
                        (g,s) => new SioSlot(g,s)
                        );
            }
            
            var mpgs = Enumerable.Range(0,setting.n_mpg-1);
            foreach(var mpg in mpgs)
            {
                  await repo.AddSlotAsync(
                        Guid,
                        mpg,
                        (g,s) => new MpgSlot(g,s)
                        );
            }

            var acrs = Enumerable.Range(0,setting.n_acr - 1);
            foreach(var acr in acrs)
            {
                  await repo.AddSlotAsync(
                        Guid,
                        acr,
                        (g,s) => new AcrSlot(g,s)
                        );
            }
            
            var cps = Enumerable.Range(0,setting.n_cp -1);
            foreach(var cp in cps)
            {
                  await repo.AddSlotAsync(
                        Guid,
                        cp,
                        (g,s) => new CpSlot(g,s)
                        );
            }
            
            var mps = Enumerable.Range(0,setting.n_mp -1);
            foreach(var mp in mps)
            {
                  await repo.AddSlotAsync(
                        Guid,
                        mp,
                        (g,s) => new MpSlot(g,s)
                        );
            }
            


      }

      public async Task<bool> GetDeviceStatusAsync(Guid guid)
      {
            var slots = await repo.GetScpSlotByGuidAsync(guid);
            return SCPDLL.scpCheckOnline((short)slots.slot_id) == 1;
      }

      public async Task ResetDeviceAsync(Guid guid)
      {
            var slots = await repo.GetScpSlotByGuidAsync(guid);
            var res = writer.ScpReset(slots.mac, (short)slots.slot_id);
            if (!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ScpReset, slots.mac, (short)slots.slot_id));

            await bus.SendAsync(new AddCommandEvent(res));

      }

      public async Task CreateModuleAsync(
            Guid DeviceGuid,
            Guid ModuleGuid,
            short Model,
            short Address,
            short Port
            )
      {
            var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
            var slot = await repo.GetFreeSlotAsync<SioSlot>(DeviceGuid);

            var res = sioWriter.SioPanelConfiguration(
                  deviceSlot.mac,
                  (short)deviceSlot.slot_id,
                  (short)slot,
                  AeroModuleModelHelper.nInputByModel((SioModel)Model),
                  AeroModuleModelHelper.nOutputByModel((SioModel)Model),
                  AeroModuleModelHelper.nReaderByModel((SioModel)Model),
                  Model,
                  1,
                  Address,
                  Port,
                  3,
                  0,
                  -1,
                  -1,
                  -1
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if (!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.SioPanelConfiguration, deviceSlot.mac, deviceSlot.slot_id));

            await repo.InsertSlotAsync<SioSlot>(
                  DeviceGuid,
                  ModuleGuid,
                  slot
            );
      }

      public async Task<bool> AsciiCommandAsync(Guid guid, string Command)
      {
            var detail = await repo.GetScpSlotByGuidAsync(guid);
            var res = writer.AsciiCommandAsync(detail.mac,(short)detail.slot_id, Command);
            if (!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AsciiCommandAsync, detail.mac,(short)detail.slot_id));
            await bus.SendAsync(new AddCommandEvent(res));
            return res.IsSend;
      }

      public async Task<bool> GetEventStatusAsync(string Mac, short ComponentId)
      {
            var res = writer.TransactionLogStatusAsync(Mac,ComponentId);
            return res.IsSend;
      }

      public async Task<bool> SetEventStatusAsync(string Mac, short ComponentId, bool IsEnable)
      {
            var res = writer.SetTransactionLogIndexAsync(Mac, ComponentId, IsEnable);
            await bus.SendAsync(new AddCommandEvent(res));
            return res.IsSend;
      }

      public Task<string> GetDeviceInformationByMacAsync(string Mac)
      {
            throw new NotImplementedException();
      }

      public async Task VerifyDeviceComponentAsync(short ComponentId)
      {
            await scp.VerifyScpComponentAsync(ComponentId);
      }

      public Task<JsonElement> GetDeviceInformationByIpAsync(string Ip, bool? IsFirst)
      {
            throw new NotImplementedException();
      }

      public async Task DeleteDeviceAsync(Guid Guid, string Ip, string Mac, short ComponentId)
      {
            var res = writer.DeleteScp(
                  Mac,
                  ComponentId
                  );

            await bus.SendAsync(new AddCommandEvent(res));

            if (!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.DeleteScp, Mac, ComponentId));

            res = writer.DetachScpFromChannel(Mac, ComponentId);

            await bus.SendAsync(new AddCommandEvent(res));

            if (!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.DetachScpChannel, Mac, ComponentId));

            await repo.EjectScpSlotAsync(Guid);
      }
}

