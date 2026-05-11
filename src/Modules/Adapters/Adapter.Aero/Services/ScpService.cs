using System;
using Adapter.Aero.Entities;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model;
using Adapter.Aero.Repositories;
using AeroAdapter.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Contract.Events;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;

public sealed class ScpService(IScpRepository repo,IMessageBus bus,IScpWriter writer,IIdReportService idReport,ISioWriter sioWriter,IMpWriter mpWriter,IMpRepository mpRepository) : IScp
{
      public async Task AssignIpAddressAsync(int ScpId, SCPReplyMessageDto.CC_WEB_CONFIG_NETWORKDto message)
      {
            var mac = await bus.QueryAsync<string>(new DeviceMacByComponentIdQuery((short)ScpId));
            if (string.IsNullOrWhiteSpace(mac))
                  // Log here
                  return;
            // return await repo.UpdateIpAddressAsync(mac, UtilitiesHelper.IntegerToIp(message.cIpAddr));
            // publish message for update ip 

      }

      public async Task AssignPortAsync(int ScpId, SCPReplyMessageDto.CC_WEB_CONFIG_HOST_COMM_PRIMDto message)
      {

            var mac = await bus.QueryAsync<string>(new DeviceMacByComponentIdQuery((short)ScpId));
            if (string.IsNullOrWhiteSpace(mac))
                  // Log here
                  return;


      }

      public async Task HandleIdReport(SCPReplyMessageDto.SCPReplyIDReportDto id)
      {
            // Get Default Settings           
            var spec = await repo.GetScpDeviceSpecificationByIdAndMacAsync(0, string.Empty);
            if (spec.n_msp1_port == 0)
                  // Log here that no database detail
                  return;

            if (!await writer.ScpDeviceSpecification(id.scp_id, UtilitiesHelper.ByteToHexStr(id.mac_addr), spec))
                  return;

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

                  if(!idReport.IsMacExist(UtilitiesHelper.ByteToHexStr(id.mac_addr)))
                  {
                        idReport.AddIdReport(report);
                        
                  }

                  // Publish Broker for new controller
                  await bus.PublishAsync(new IdReportUpdatedEvent(idReport.IdReportInMemory.Select(x => new IdReportDto(x.ScpId, x.SerialNumber, x.Mac, x.Ip, x.Port, x.Fw)).ToList()));
            }
            else
            {
                  // Update ScpId if already Exists
                  await bus.PublishAsync(new ComponentIdUpdatedEvent(id.scp_id, UtilitiesHelper.ByteToHexStr(id.mac_addr)));

                  // Publish Broker for update
            }

            var db = await repo.GetAccessDatabaseSpecificationByIdAndMacAsync(0, string.Empty);
            if (db.n_card == 0)
                  // Log here the no database detail
                  return;


            if (!await writer.AccessDatabaseSpecification(id.scp_id, UtilitiesHelper.ByteToHexStr(id.mac_addr), db))
                  return;


            var elev = await repo.GetElevatorAccessLevelSpecificationByIdAndMacAsync(0, string.Empty);
            if (elev.max_ealvl == 0)
                  // Log here
                  return;

            // if (!await writer.ElevatorAccessLevelSpecification(id.scp_id, elev))
            //       return;

            if (!await writer.TimeSet(id.scp_id, UtilitiesHelper.ByteToHexStr(id.mac_addr)))
                  return;


            // // Read Structure 
            // if (!await writer.SCPStructureStatusRead(id.scp_id,
            //       [
            //             (short)SCPStructure.SCPSID_TRAN,
            //             (short)SCPStructure.SCPSID_TZ,
            //             (short)SCPStructure.SCPSID_HOL,
            //             (short)SCPStructure.SCPSID_MSP1,
            //             (short)SCPStructure.SCPSID_SIO,
            //             (short)SCPStructure.SCPSID_MP,
            //             (short)SCPStructure.SCPSID_CP,
            //             (short)SCPStructure.SCPSID_ACR,
            //             (short)SCPStructure.SCPSID_ALVL,
            //             (short)SCPStructure.SCPSID_TRIG,
            //             (short)SCPStructure.SCPSID_PROC,
            //             (short)SCPStructure.SCPSID_MPG,
            //             (short)SCPStructure.SCPSID_AREA,
            //             (short)SCPStructure.SCPSID_EAL,
            //             (short)SCPStructure.SCPSID_CRDB
            //       ]
            // ))
            //       return;





            // // Send to get IP and Port 
            // await writer.ReadsConfiguration(id.scp_id, WebConfigReadType.NetworkSettingss);
            // await writer.ReadsConfiguration(id.scp_id, WebConfigReadType.HostCommunicationPrimarySettings);


      }

      public async Task InitialScpConfiguration(short ScpId)
      {

            string Mac = await bus.QueryAsync<string>(new DeviceMacByComponentIdQuery(ScpId));

            var config = await repo.GetDriverConfigurationByIdAndMacAndPortNumberAsync(0, string.Empty, 3); // 3 is internal port in x1100
            if (config.baudrate == 0)
                  // Log here the no database detail
                  return;

            if (!await writer.DriverConfiguration(ScpId, Mac, config))
                  return;


            var sio = await repo.GetSioPanelConfigurationByIdAndMacAndAddressAsync(0, string.Empty, 0);
            if (sio.model == 0)
                  // Log here the no database detail
                  return;

            if (!await sioWriter.SioPanelConfiguration(ScpId, Mac, sio))
                  return;


            List<int> inputs = Enumerable.Range(SioModelHelper.nInputByModel(SioModel.x1100) - 3, 3).ToList();

            var input = await mpRepository.GetInputPointSpecificationByIdAndMacAndSioNumber(0, string.Empty, 0);
            if (input.hold_time == 0)
                  // Log here the no database detail
                  return;


            // Setting Input for Alarm 
            foreach (var i in inputs)
            {
                  input.UpdateInputNumber((short)i);
                  if (!await mpWriter.InputPointSpecification(ScpId, Mac, input))
                        return;
            }
      }

      public async Task<bool> SendASCIICommandAsync(string Command)
      {
            return writer.SendASCIICommandAsync(Command);
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

            var spec = await repo.GetScpDeviceSpecificationByIdAndMacAsync(0, string.Empty);
            if (spec.n_msp1_port == 0)
                  // Log here that no database detail
                  return false;

            var db = await repo.GetAccessDatabaseSpecificationByIdAndMacAsync(0, string.Empty);
            if (db.n_card == 0)
                  // Log here the no database detail
                  return false;

            var elev = await repo.GetElevatorAccessLevelSpecificationByIdAndMacAsync(0, string.Empty);
            if (elev.max_ealvl == 0)
                  // Log here
                  return false;

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

            // string mac = await repo.GetMacFromScpIdAsync((short)ScpId);
            // if (isVerify)
            // {
            //       // Publish verify 

            // }
            // else
            // {
            //       // Publish verify 

            // }



            return isVerify;

      }
}
