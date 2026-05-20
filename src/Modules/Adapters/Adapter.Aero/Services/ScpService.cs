using System;
using System.Text.Json;
using Adapter.Abstraction.Events;
using Adapter.Aero.Entities;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using Device.Contract.Command;
using Device.Contract.DTOs;
using Device.Contract.Events;
using Device.Contract.Queries;
using Events.Contract.Command;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Logging;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;

public sealed class ScpService(ILogger<ScpService> logger, IMessageBus bus, IScpCommand scpCommand, IIdReportService idReport, IModuleCommand sioWriter, IInputCommand mpWriter,IAeroRepository repo) : IScp
{



      public async Task HandleIdReport(SCPReplyMessageDto.SCPReplyIDReportDto id)
      {
            // Get Default Settings           

            var res = scpCommand.ScpDeviceSpecification(
                  UtilitiesHelper.ByteToHexStr(id.mac_addr),
                  id.scp_id,
                  3,
                  60000,
                  33,
                  615,
                  388,
                  64,
                  32000,
                  1024,
                  1024,
                  -25200,
                  0,
                  255,
                  255,
                  128,
                  60000,
                  0,
                  1,
                  0
                  );


            await bus.SendAsync(new AddCommandEvent(res));


            if (!await bus.QueryAsync(new IsAnyWithMacQuery(UtilitiesHelper.ByteToHexStr(id.mac_addr))))
            {
                  // New Contoller

                  IdReport report = new IdReport(
                        id.scp_id,
                        id.serial_number.ToString(),
                        UtilitiesHelper.ByteToHexStr(id.mac_addr),
                        string.Empty,
                        0,
                        $"{id.sft_rev_major}.{id.sft_rev_minor}"
                        );

                  if (!idReport.IsMacExist(UtilitiesHelper.ByteToHexStr(id.mac_addr)))
                  {
                        idReport.AddIdReport(report);

                  }


                  await bus.PublishAsync(new IdReportUpdatedEvent(idReport.IdReportInMemory.Select(x => new IdReportDto(x.ScpId, x.SerialNumber, x.Mac, x.Ip, x.Port, x.Fw)).ToList()));


            }
            else
            {

                  var scp_id = await bus.QueryAsync(new ComponentIdByMacQuery(UtilitiesHelper.ByteToHexStr(id.mac_addr)));


                  if (scp_id != id.scp_id)
                  {
                        res = scpCommand.SetScpId(UtilitiesHelper.ByteToHexStr(id.mac_addr), id.scp_id, (short)scp_id);
                  }


                  // Read Structure 
                  res = scpCommand.ScpStructureStatusRead(UtilitiesHelper.ByteToHexStr(id.mac_addr), (short)scp_id,
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



            }

      }

      public async Task InitialScpConfigurationAsync(int ScpId)
      {

            var device = await bus.QueryAsync(new DeviceByComponentIdQuery(ScpId));


            var res = scpCommand.AccessDatabaseSpecification(
                  device.Mac,
                  (short)ScpId,
                  1000,
                  8,
                  324,
                  1,
                  1,
                  2,
                  2,
                  1,
                  1,
                  7,
                  1,
                  1,
                  64,
                  0,
                  5,
                  0,
                  15,
                  15
                  );


            await bus.SendAsync(new AddCommandEvent(res));


            res = scpCommand.ElevatorAccessLevelSpecification(device.Mac, (short)ScpId, 256, 128);
            await bus.SendAsync(new AddCommandEvent(res));

            res = scpCommand.TimeSet(device.Mac, (short)ScpId);

            await bus.SendAsync(new AddCommandEvent(res));


            res = scpCommand.ReadsConfiguration(device.Mac, (short)ScpId, WebConfigReadType.NetworkSettingss);

            await bus.SendAsync(new AddCommandEvent(res));

            res = scpCommand.ReadsConfiguration(device.Mac, (short)ScpId, WebConfigReadType.HostCommunicationPrimarySettings);

            await bus.SendAsync(new AddCommandEvent(res));


            res = scpCommand.DriverConfiguration(device.Mac, device.ComponentId, 0, 3, -1, 0, 0, 0);

            await bus.SendAsync(new AddCommandEvent(res));


            res = sioWriter.SioPanelConfiguration(
                  device.Mac,
                  (short)ScpId,
                  0,
                  AeroModuleModelHelper.nInputByModel(SioModel.x1100),
                  AeroModuleModelHelper.nOutputByModel(SioModel.x1100),
                  AeroModuleModelHelper.nReaderByModel(SioModel.x1100),
                  (short)SioModel.x1100,
                  1,
                  0,
                  0,
                  3,
                  0,
                  -1,
                  -1,
                  -1);

            await bus.SendAsync(new AddCommandEvent(res));





            List<int> inputs = Enumerable.Range(AeroModuleModelHelper.nInputByModel(SioModel.x1100) - 3, 3).ToList();



            // Setting Input for Alarm 
            foreach (var i in inputs)
            {

                  res = mpWriter.InputPointSpecification(
                        device.Mac,
                        device.ComponentId,
                        0,
                        (short)i,
                        0,
                        2,
                        0
                  );

                  await bus.SendAsync(new AddCommandEvent(res));


            }


            var Metadata = JsonSerializer.Deserialize<AeroMetadata>(device.Metadata);
            if (Metadata != null)
            {
                  if (Metadata.PortOne)
                  {
                        res = scpCommand.DriverConfiguration(device.Mac, device.ComponentId,1,1,Metadata.BaudRateOne, 0, Metadata.ProtocolOne, 0);

                        await bus.SendAsync(new AddCommandEvent(res));
                  }

                  if (Metadata.PortTwo)
                  {
                        res = scpCommand.DriverConfiguration(device.Mac, device.ComponentId,2,2,Metadata.BaudRateTwo, 0, Metadata.ProtocolTwo,0);

                        await bus.SendAsync(new AddCommandEvent(res));
                  }
            }
      }


      public async Task<bool> UploadScpComponentAsync(int ScpId)
      {
            // Query Each Component and Send Command here.
            throw new NotImplementedException();
      }

      public async Task<bool> VerifyScpComponentAsync(int ScpId)
      {
            // Query Each Component and Send Command here.
            // throw new NotImplementedException();
            return true;
      }

      public async Task<bool> VerifySCPStructureMemoryAllocate(int ScpId, SCPReplyMessageDto.SCPReplyStrStatusDto message)
      {
            bool isVerify = true;

            var spec = await repo.GetScpDeviceSpecificationAsync();
            if (spec.n_msp1_port == 0)
            {
                  logger.LogError("Scp device specification setting not found.");
                  return false;
            }


            var db = await repo.GetAccessDatabaseSpecificationAsync();
            if (db.n_card == 0)
            {
                  logger.LogError("Access database specification setting not found.");
                  return false;
            }


            var elev = await repo.GetElevatorAccessLevelSpecificationAsync();
            if (elev.max_ealvl == 0)
            {
                  logger.LogError("Elevator access level specification setting not found.");
                  return false;
            }


            // Switch
            foreach (var str in message.sStrSpec)
            {
                  switch (str.nStrType)
                  {
                        case (short)SCPStructure.SCPSID_TRAN: // 1 Transactions
                              isVerify = spec.n_transcations > str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_TZ: // 2 Time zones
                              isVerify = spec.n_tz + 1 == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_HOL: // 3 Holidays
                              isVerify = spec.n_hol == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_MSP1: // 4 Msp1 ports (SIO drivers)
                              // isVerify = spec.n_msp1_port == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_SIO: // 5 SIOs
                              isVerify = spec.n_sio == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_MP: // 6 Monitor points
                              isVerify = spec.n_mp == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_CP: // 7 Control points
                              isVerify = spec.n_cp == str.nRecords;

                              break;

                        case (short)SCPStructure.SCPSID_ACR: // 8 Access control readers
                              isVerify = spec.n_acr == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_ALVL: // 9 Access levels
                              isVerify = spec.n_alvl == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_TRIG: // 10 Triggers
                              isVerify = spec.n_trgr == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_PROC: // 11 Procedures
                              isVerify = spec.n_proc == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_MPG: // 12 Monitor point groups
                              isVerify = spec.n_mpg == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_AREA: // 13 Access areas
                              // isVerify = spec.n_area == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_EAL: // 14 Elevator access levels
                              // isVerify = elev.MaxElalvl == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_CRDB: // 15 Cardholder database
                              // isVerify = db.nCards == str.nRecords;
                              break;

                        case (short)SCPStructure.SCPSID_FLASH: // 20 FLASH specs
                              break;

                        case (short)SCPStructure.SCPSID_BSQN: // 21 Build sequence number
                              break;

                        case (short)SCPStructure.SCPSID_SAVE_STAT: // 22 Flash save status
                              break;

                        case (short)SCPStructure.SCPSID_MAB1_FREE: // 23 Memory alloc block 1 free
                              break;

                        case (short)SCPStructure.SCPSID_MAB2_FREE: // 24 Memory alloc block 2 free
                              break;

                        case (short)SCPStructure.SCPSID_ARQ_BUFFER: // 26 Access request buffers
                              break;

                        case (short)SCPStructure.SCPSID_PART_FREE_CNT: // 27 Partition memory free info
                              break;

                        default:
                              break;
                  }

                  if (!isVerify)
                        break;
            }


            if (isVerify)
            {
                  // Publish verify 
                  await bus.PublishAsync(new MemoryAllocateEvent(ScpId, ScpSyncStatus.SYNC.ToString()));

            }
            else
            {
                  // Publish verify 
                  await bus.PublishAsync(new MemoryAllocateEvent(ScpId, ScpSyncStatus.RESET.ToString()));
            }



            return isVerify;

      }
}
