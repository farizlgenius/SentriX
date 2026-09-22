using Aero.Application.Enums;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Core.Contract.Commands.Events;
using Core.Contract.DTOs.Device;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Microsoft.Extensions.Logging;
using Setting.Contract.Interfaces;
using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class IdReportService(
  IMessageBus bus,
  ISetting setting,
  IComponentMapping mapping,
  IDeviceRepository repo,
  ITempDevice temp,
  ILogger<IdReportService> logger
  ) : IIdReportService
{
  public async Task HandleInCommingDeviceAsync(ReplyMessage.SCPReplyIDReport dto, CancellationToken ct = default)
  {
    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>> Here");
    // Get Setting
    var scpDevice = await setting.GetAeroDriverSettingAsync();

    var mac = UtilitiesHelper.ByteToHexStr(dto.mac_addr);

    // Send 1107 Command always 
    var res = repo.ScpDeviceSpecification(
      mac,
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


    await bus.SendAsync(new AdapterEventCommand(res), ct);

    // New 
    if (temp.Contains(mac))
    {
      return;
    }

    if (await bus.QueryAsync(new IsAnyMacQuery(mac)))
    {
      // Get Scp Id and Set it 
      var externalId = await mapping.GetExternalIdByMacAndEntityAsync(
        mac,
        EntityType.Device,
        ct);

      res = repo.SetScpId(mac, dto.scp_id, (short)externalId);

      await bus.SendAsync(new AdapterEventCommand(res), ct);

      // Update data 
      // Send Command to get Ip
    res = repo.ReadsConfiguration(
       UtilitiesHelper.ByteToHexStr(dto.mac_addr),
       dto.scp_id,
       WebConfigReadType.NetworkSettingss
      );

    await bus.SendAsync(new AdapterEventCommand(res), ct);

    // Port
    res = repo.ReadsConfiguration(
      UtilitiesHelper.ByteToHexStr(dto.mac_addr),
      dto.scp_id,
      WebConfigReadType.HostCommunicationPrimarySettings
     );

    await bus.SendAsync(new AdapterEventCommand(res), ct);


      // Start initial Device here


      // And Other Device

      return;
    }

    // Send Command to get Ip
    res = repo.ReadsConfiguration(
       UtilitiesHelper.ByteToHexStr(dto.mac_addr),
       dto.scp_id,
       WebConfigReadType.NetworkSettingss
      );

    await bus.SendAsync(new AdapterEventCommand(res), ct);

    // Port
    res = repo.ReadsConfiguration(
      UtilitiesHelper.ByteToHexStr(dto.mac_addr),
      dto.scp_id,
      WebConfigReadType.HostCommunicationPrimarySettings
     );

    await bus.SendAsync(new AdapterEventCommand(res), ct);

    // var existsIds = temp.TryGetUnavailableId();

    // Get Free Slot 
    // var id = await mapping.GetFreeIdByMacAndEntityAndVendorAsync(EntityType.Device, Vendor.aero, scpDevice.nScps, existsIds, ct);

    // if (id == null)
    //   throw new ExceedException(EntityType.Device, "");

    // res = repo.SetScpId(UtilitiesHelper.ByteToHexStr(dto.mac_addr),dto.scp_id,(short)dto);

    await bus.SendAsync(new AdapterEventCommand(res), ct);

    var added = temp.TryAdd(
      new TempDeviceDto(
        Guid.NewGuid(),
        dto.scp_id,
        dto.serial_number,
        UtilitiesHelper.ByteToHexStr(dto.mac_addr),
        Vendor.aero,
        $"{dto.sft_rev_major}.{dto.sft_rev_minor}",
        string.Empty,
        0
      )
    );

    if (!added)
    {
      return;
    }

    logger.LogInformation(
        "New unknown device discovered: {MacAddress}",
        UtilitiesHelper.ByteToHexStr(dto.mac_addr));


    // Check the already have mac in device table


    // if (await bus.QueryAsync(new IsAnyMacQuery(UtilitiesHelper.ByteToHexStr(dto.mac_addr))))
    // {

    // }
    // else
    // {
    //   var id = await mapping.GetFreeIdByMacAndEntityAndVendorAsync(EntityType.Device, Vendor.aero, scpDevice.nScps);

    //   if (id == null)
    //     throw new Exception("Device number Exceed.");

    //   repo.SetScpId(
    //     UtilitiesHelper.ByteToHexStr(dto.mac_addr),
    //     dto.scp_id,
    //     (short)id
    //   );

    //   // Save new device to table 
    //   var d = new CreateDeviceDto(
    //     $"Aero x1100 {dto.serial_number}",
    //     dto.serial_number.ToString(),
    //     UtilitiesHelper.ByteToHexStr(dto.mac_addr),
    //     string.Empty,
    //     0,
    //     $"{dto.sft_rev_major}.{dto.sft_rev_minor}",
    //     Vendor.aero,
    //     string.Empty,
    //     Guid.Empty,
    //     new List<Core.Contract.DTOs.DeviceModule.DeviceModuleDto>()
    //   );

    //   await bus.QueryAsync(new InsertInCommingDeviceQuery(d));
    // }

  }
}