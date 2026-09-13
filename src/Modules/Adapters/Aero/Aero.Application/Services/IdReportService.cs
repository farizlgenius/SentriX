using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Core.Contract.DTOs.Device;
using Core.Contract.Queries;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class IdReportService(
  IMessageBus bus,
  IScpDeviceSpecification scpDevice,
  IDeviceRepository repo
  ) : IIdReportService
{
  public async Task HandleInCommingDeviceAsync(ReplyMessage.SCPReplyIDReport dto, CancellationToken ct = default)
  {
    // Send 1107 Command always 
    var res = repo.ScpDeviceSpecification(
      UtilitiesHelper.ByteToHexStr(dto.mac_addr),
      dto.scp_id,
      (short)scpDevice.nMsp1Port,
      scpDevice.nTransaction,
      (short)scpDevice.nSio,
      (short)scpDevice.nMp,
      (short)scpDevice.nCp,
      (short)scpDevice.nAcr,
      (short)scpDevice.nAlvl,
      (short)scpDevice.nTrgr,
      (short)scpDevice.nProc,
      -25200,
      (short)scpDevice.nDstID,
      (short)scpDevice.nTz,
      (short)scpDevice.nHol,
      (short)scpDevice.nMpg,
      (short)scpDevice.nTranLimit,
      (short)scpDevice.nOperModes,
      (short)scpDevice.OperType,
      0
    );

    // await bus.

    // Check the already have mac in device table
    if (await bus.QueryAsync(new IsAnyMacQuery(UtilitiesHelper.ByteToHexStr(dto.mac_addr))))
    {

    }
    else
    {
      // Save new device to table 
      var d = new CreateDeviceDto(
        $"Aero x1100 {dto.serial_number}",
        dto.serial_number.ToString(),
        UtilitiesHelper.ByteToHexStr(dto.mac_addr),
        string.Empty,
        0,
        $"{dto.sft_rev_major}.{dto.sft_rev_minor}",
        Vendor.aero,
        string.Empty,
        Guid.Empty,
        new List<Core.Contract.DTOs.DeviceModule.DeviceModuleDto>()
      );

      await bus.QueryAsync(new InsertInCommingDeviceQuery(d));
    }

  }
}