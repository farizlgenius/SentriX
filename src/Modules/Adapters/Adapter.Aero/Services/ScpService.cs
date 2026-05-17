using System;
using System.Text.Json;
using Adapter.Abstraction.Events;
using Adapter.Aero.Entities;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model;
using Adapter.Aero.Persistences.Entities;
using Adapter.Aero.Repositories;
using AeroAdapter.Application.Interfaces;
using Device.Contract.Command;
using Device.Contract.DTOs;
using Device.Contract.Events;
using Device.Contract.Queries;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Logging;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;

public sealed class ScpService(ILogger<ScpService> logger,IScpRepository repo,ISioRepository sioRepo,IMessageBus bus,IScpWriter writer,IIdReportService idReport,ISioWriter sioWriter,IMpWriter mpWriter,IMpRepository mpRepository) : IScp
{

      

      public async Task HandleIdReport(SCPReplyMessageDto.SCPReplyIDReportDto id)
      {
            // Get Default Settings           
            var spec = await repo.GetScpDeviceSpecificationByIdAndMacAsync(0, string.Empty);
            if (spec.n_msp1_port == 0)
            {
                  // Log here that no database detail
                  logger.LogError("Scp Device Specification Setting not found.");
                  return;
            }
                  

            if (!await writer.ScpDeviceSpecification(id.scp_id, UtilitiesHelper.ByteToHexStr(id.mac_addr), spec))
            {
                  logger.LogError("Scp Device Specification send failed.");
                   return;
            }
                 

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

                  // Add Scp
                  if(!await repo.AddScpAsync(id.scp_id, UtilitiesHelper.ByteToHexStr(id.mac_addr)))
                  {
                        logger.LogError("Add Scp failed.");
                        return;  
                  }

                  await bus.PublishAsync(new IdReportUpdatedEvent(idReport.IdReportInMemory.Select(x => new IdReportDto(x.ScpId, x.SerialNumber, x.Mac, x.Ip, x.Port, x.Fw)).ToList()));

                 
            }
            else
            {


                  // Update Scp
                  if(!await repo.UpdateScpAsync(id.scp_id, UtilitiesHelper.ByteToHexStr(id.mac_addr)))
                  {
                        logger.LogError("Update Scp Id failed.");
                        return;
                  }


                        // Read Structure 
                  if (!await writer.SCPStructureStatusRead(id.scp_id,UtilitiesHelper.ByteToHexStr(id.mac_addr),
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
                  ))
                  {
                        logger.LogError("Scp Structure Status Read send failed.");
                         return;
                  }
                        
                  
            }

      }

      public async Task InitialScpConfigurationAsync(int ScpId)
      {
            string Mac = await repo.MacByScpIdAsync(ScpId);
            var aero_id = await repo.GetAeroIdByScpIdAsync(ScpId);
            var ModuleId = await bus.QueryAsync(new ModuleIdByMacAndAddressQuery(Mac,0));

            var db = await repo.GetAccessDatabaseSpecificationByIdAndMacAsync(0, string.Empty);
            if (db.n_card == 0)
            {
                  logger.LogError("Access database specification setting not found.");
                  return;     
            }


            if (!await writer.AccessDatabaseSpecification((short)ScpId, Mac, db))
            {
                  logger.LogError("Access database specification send failed.");
                  return;
            }
                  


            var elev = await repo.GetElevatorAccessLevelSpecificationByIdAndMacAsync(0, string.Empty);
            if (elev.max_ealvl == 0)
            {
                  logger.LogError("Elevator access level specification setting not found.");
                  return;     
            }

            // if (!await writer.ElevatorAccessLevelSpecification(id.scp_id, elev))
            //       return;

            if (!await writer.TimeSet((short)ScpId, Mac))
            {
                  logger.LogError("Time set send failed.");
                  return;
            }
                  

            // Send to get IP and Port 
            if(!await writer.ReadsConfiguration((short)ScpId, Mac, WebConfigReadType.NetworkSettingss))
            {
                  logger.LogError("Read configuration send failed.");
            }

            if(!await writer.ReadsConfiguration((short)ScpId, Mac, WebConfigReadType.HostCommunicationPrimarySettings))
            {
                  logger.LogError("Read configuration send failed.");
            }



            // Save Driver Configuration to DB
            var driverConfig = new DriverConfiguration(
                  aero_id,
                  0,
                  3,
                  -1,
                  0,
                  0,
                  0
                  );

            var drivCon = await repo.AddDriverConfigurationAsync(driverConfig);

            if(drivCon.id == 0)
            {
                  logger.LogError("Driver configuration save failed.");
                  return;
            }

                  

            if (!await writer.DriverConfiguration((short)ScpId, Mac, driverConfig))
            {
                  logger.LogError("Driver configuration send failed.");
                  return;
            }
                  



            var sioConfig = new SioPanelConfiguration(
                  aero_id,
                  0,
                  SioModelHelper.nInputByModel(SioModel.x1100),
                  SioModelHelper.nOutputByModel(SioModel.x1100),
                  SioModelHelper.nReaderByModel(SioModel.x1100),
                  (short)SioModel.x1100,
                  1,
                  0,
                  0,
                  3,
                  0,
                  -1,
                  -1
                  -1,
                  -1,
                  ModuleId
            );

            var config = await sioRepo.AddSioPanelConfigurationAsync(sioConfig);

            if(config.id == 0)
            {
                  logger.LogError("Sio pane configuration add failed.");
                  return;
            }


                  

            if (!await sioWriter.SioPanelConfiguration((short)ScpId, Mac, config))
            {
                  logger.LogError("Sio panel configuration send failed.");
                   return;
            }
                 

            List<int> inputs = Enumerable.Range(SioModelHelper.nInputByModel(SioModel.x1100) - 3, 3).ToList();



            // Setting Input for Alarm 
            foreach (var i in inputs)
            {
                  var cInput = new InputPointSpecification(
                        aero_id,
                        0,
                        (short)i,
                        0,
                        2,
                        0
                        );

                  var sInput = await mpRepository.AddInputPointSpecificationAsync(cInput);

                  if (!await mpWriter.InputPointSpecification((short)ScpId, Mac, sInput))
                  {
                        logger.LogError("Input point specification send failed.");
                  }
                        
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
            {
                  logger.LogError("Scp device specification setting not found.");
                  return false;
            }
                  

            var db = await repo.GetAccessDatabaseSpecificationByIdAndMacAsync(0, string.Empty);
            if (db.n_card == 0)
            {
                  logger.LogError("Access database specification setting not found.");
                  return false;
            }


            var elev = await repo.GetElevatorAccessLevelSpecificationByIdAndMacAsync(0, string.Empty);
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

            string Mac = await repo.MacByScpIdAsync(ScpId);
            if (isVerify)
            {
                  // Publish verify 
                  await bus.PublishAsync(new MemoryAllocateEvent(Mac, ScpSyncStatus.SYNC.ToString()));

            }
            else
            {
                  // Publish verify 
                  await bus.PublishAsync(new MemoryAllocateEvent(Mac, ScpSyncStatus.RESET.ToString()));
            }



            return isVerify;

      }
}
