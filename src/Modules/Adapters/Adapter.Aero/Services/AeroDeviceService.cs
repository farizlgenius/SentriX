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
using Events.Contract.Command;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;

public sealed class AeroDeviceService(
      ILogger<AeroDeviceService> logger,
      IScpCommand writer, IIdReportService idReport, IModuleCommand sioWriter,IMessageBus bus
      ) : IDeviceAdapter
{
      public async Task<List<IdReportDto>> GetIdReportsAsync()
      {
            return idReport.IdReportInMemory.Select(x => new IdReportDto(x.ScpId, x.SerialNumber, x.Mac, x.Ip, x.Port, x.Fw)).ToList();
      }

      public async Task CreateDeviceAsync(DeviceDto dto)
      {


             // Read Structure 
            var res = writer.ScpStructureStatusRead(
                  dto.Mac,
                  dto.ComponentId,
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


            idReport.IdReportInMemory.RemoveAll(x => x.Mac.Equals(dto.Mac));


      }

      public async Task<bool> GetDeviceStatusAsync(int ComponentId)
      {
            return SCPDLL.scpCheckOnline((short)ComponentId) == 1;
      }

      public async Task<bool> ResetDeviceAsync(string Mac,short ScpId)
      {
            var res = writer.ScpReset(Mac,ScpId);
             await bus.SendAsync(new AddCommandEvent(res));

             return res.IsSend;
      }

      public async Task CreateModuleAsync(
            string Mac,
            short ScpId,
            short SioNumber,
            short Model,
            short Address,
            short Port
            )
      {

            var res = sioWriter.SioPanelConfiguration(
                  Mac,
                  ScpId,
                  SioNumber,
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
      }

      public async Task<bool> AsciiCommandAsync(string Mac,int ScpId,string Command)
      {
            var res = writer.AsciiCommandAsync(Mac,(short)ScpId,Command);
            await bus.SendAsync(new AddCommandEvent(res));
            return res.IsSend;
      }
}

