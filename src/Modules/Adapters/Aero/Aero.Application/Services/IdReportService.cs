using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Core.Contract.DTOs.Device;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class IdReportService(
  IMessageBus bus,
  ISetting setting,
  IComponentMapping mapping,
  IDeviceRepository repo
  ) : IIdReportService
{
  public async Task HandleInCommingDeviceAsync(ReplyMessage.SCPReplyIDReport dto, CancellationToken ct = default)
  {
    // Get Setting
    var scpDevice = await setting.GetAeroDriverSettingAsync();

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
      (short)scpDevice.GmtOffset,
      scpDevice.IsDaylightSaving ? (short)100 : (short)0,
      (short)scpDevice.nTz,
      (short)scpDevice.nHol,
      (short)scpDevice.nMpg,
      (short)scpDevice.nTranLimit,
      0,
      1,
      0
    );

    await bus.QueryAsync(new InsertAdapterEventQuery(res), ct);

    // Check the already have mac in device table
    if (await bus.QueryAsync(new IsAnyMacQuery(UtilitiesHelper.ByteToHexStr(dto.mac_addr))))
    {

    }
    else
    {
      var id = await mapping.GetFreeIdByMacAndEntityAndVendorAsync(EntityType.Device, Vendor.aero, scpDevice.nScps);

      if (id == null)
        throw new Exception("Device number Exceed.");

      repo.SetScpId(
        UtilitiesHelper.ByteToHexStr(dto.mac_addr),
        dto.scp_id,
        (short)id
      );

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