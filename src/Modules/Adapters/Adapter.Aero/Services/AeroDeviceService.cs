using System;
using System.Text.Json;
using Adapter.Abstraction;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Contract.Queries;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;

public sealed class AeroDeviceService(
      ILogger<AeroDeviceService> logger,
      IScpRepository repo,ISioRepository sioRepo, IScpWriter writer, IIdReportService idReport, ISioWriter sioWriter
      ) : IDeviceAdapter
{
      public async Task<List<IdReportDto>> GetIdReportsAsync()
      {
            return idReport.IdReportInMemory.Select(x => new IdReportDto(x.ScpId, x.SerialNumber, x.Mac, x.Ip, x.Port, x.Fw)).ToList();
      }

      public async Task CreateDeviceAsync(DeviceDto dto)
      {

            var Metadata = JsonSerializer.Deserialize<AeroMetadata>(dto.Metadata);
            var aero_id = await repo.GetAeroIdByMacAsync(dto.Mac);
            var scp_id = await repo.GetScpIdByMacAsync(dto.Mac);

            // Save Driver Configuration to Database
            if (Metadata != null && Metadata.PortOne)
            {
                  await repo.AddDriverConfigurationAsync(
                        new Persistences.Entities.DriverConfiguration(
                              aero_id,
                              1,
                              1,
                              (short)Metadata.BaudRateOne,
                              90,
                              (short)Metadata.ProtocolOne,
                              0
                        )
                  );
            }

            if (Metadata != null && Metadata.PortTwo)
            {
                  await repo.AddDriverConfigurationAsync(
                        new Persistences.Entities.DriverConfiguration(
                              aero_id,
                              2,
                              2,
                              (short)Metadata.BaudRateTwo,
                              90,
                              (short)Metadata.ProtocolTwo,
                              0
                        )
                  );
            }

             // Read Structure 
            if (!await writer.SCPStructureStatusRead((short)scp_id,dto.Mac,
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
                  return;


            idReport.IdReportInMemory.RemoveAll(x => x.Mac.Equals(dto.Mac));

            // Add to Scp 
            


      }

      public async Task<bool> GetDeviceStatusByMacAsync(string mac)
      {
            var ScpId = await repo.ScpIdByMacAsync(mac);
            return SCPDLL.scpCheckOnline((short)ScpId) == 1;
      }

      public async Task<bool> ResetDeviceAsync(string Mac)
      {
            var ScpId = await repo.ScpIdByMacAsync(Mac);
            return await writer.SCPReset((short)ScpId, Mac);
      }

      public async Task CreateModuleAsync(CreateModuleDto dto)
      {
            int ScpId = await repo.ScpIdByMacAsync(dto.Mac);
            var aero_id = await repo.GetAeroIdByMacAsync(dto.Mac);
            var sio = new SioPanelConfiguration(
                  aero_id,
                  0,
                  SioModelHelper.nInputByModel((SioModel)dto.Model),
                  SioModelHelper.nOutputByModel((SioModel)dto.Model),
                  SioModelHelper.nReaderByModel((SioModel)dto.Model),
                  (short)dto.Model,
                  1,
                  (short)dto.Port,
                  (short)dto.Address,
                  3,
                  1,
                  -1,
                  -1,
                  -1,
                  dto.Module_id);

            var config = await sioRepo.AddSioPanelConfigurationAsync(sio);

            if(config.id == 0)
            {
                  logger.LogError("Sio panel configuration add failed.");
                  throw new Exception("Sio panel configuration add failed.");
            }

            if(!await sioWriter.SioPanelConfiguration((short)ScpId, dto.Mac, config))
            {
                  logger.LogError("Sio panel configuration send failed.");
                  throw new Exception("Sio panel configuration send failed.");
            }

      }

      public async Task<bool> AsciiCommandAsync(string Mac,string Command)
      {
            var ScpId = await repo.ScpIdByMacAsync(Mac);
            return await writer.AsciiCommandAsync(ScpId,Command);
      }
}

