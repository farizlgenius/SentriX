using System;
using Adapter.Abstraction;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using AeroAdapter.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;

public sealed class AeroDeviceService(
      IScpRepository repo,IMessageBus bus,IScpWriter writer,IIdReportService idReport,ISioWriter sioWriter,IMpWriter mpWriter,IMpRepository mpRepository
      ) : IDeviceAdapter
{
      public async Task<List<IdReportDto>> GetIdReportsAsync()
      {
            return idReport.IdReportInMemory.Select(x => new IdReportDto(x.ScpId, x.SerialNumber, x.Mac, x.Ip, x.Port, x.Fw)).ToList();
      }

      public async Task CreateDeviceCommandAsync(CreateDeviceDto dto)
      {
            string Mac = await bus.QueryAsync<string>(new DeviceMacByComponentIdQuery(dto.ComponentId));

            // Read Structure 
            if (!await writer.SCPStructureStatusRead((short)dto.ComponentId,Mac,
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


            
      }
}
