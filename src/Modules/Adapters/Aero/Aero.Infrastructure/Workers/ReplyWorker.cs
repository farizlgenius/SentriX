using System.Diagnostics.Contracts;
using System.Threading.Channels;
using Adapter.Contract.Interfaces;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.Device;
using Aero.Domain.Entities;
using Aero.Infrastructure.Helpers;
using Core.Contract.DTOs.Events.Event;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Contract.Queries.Device;
using Core.Contract.Queries.DeviceModule;
using Core.Contract.Queries.Door;
using Core.Contract.Queries.Input;
using Core.Contract.Queries.MonitorGroup;
using Core.Contract.Queries.Output;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notifier.Contract.Constants;
using Notifier.Contract.Interfaces;
using Notifier.Contract.Topics;
using Setting.Contract.Queries;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;
using SharedKernel.Model.Status;

namespace Aero.Infrastructure.Workers;

public sealed class ReplyWorker(Channel<ReplyMessage> queue, ILogger<ReplyWorker> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{
  protected async override Task ExecuteAsync(CancellationToken ct)
  {

    Console.WriteLine("Aero Background worker started.");
    while (!ct.IsCancellationRequested)
    {

      // Aero Message
      await foreach (var message in queue.Reader.ReadAllAsync(ct))
      {
        Console.WriteLine("Message received");

        using var scope = scopeFactory.CreateScope();

        try
        {
          Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>> Message Type: " + DescriptionHelper.GetMessageTypeDesc(message.ReplyType));
          switch (message.ReplyType)
          {

            case (int)enSCPReplyType.enSCPReplyNAK:
              Console.WriteLine(message.nak.description_code);
              break;
            case (int)enSCPReplyType.enSCPReplyTransaction:
              Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<< Tran Type: " + DescriptionHelper.GetTranTypeDesc(message.tran.tran_type));
              // define mac , name , actor , image
              var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              var idevice = scope.ServiceProvider.GetRequiredService<IDevice>();
              var dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId,Vendor.aero),ct);
              var mac = dev.Mac;
              var name = dev.Name;
              var actor = string.Empty;
              var locationGuid = dev.LocationGuid;
              var image = string.Empty;
              switch (message.tran.tran_type)
              {

                case (short)tranType.tranTypeSioComm:
                  break;
                case (short)tranType.tranTypeCardFull:
                  // if (isWaitingCardScan && ScanScpId == message.ScpId && ScanAcrNo == message.tran.source_number)
                  // {
                  //     var status = new CardScanStatus
                  //     {
                  //         Mac = await qhw.GetMacFromComponentAsync((short)message.ScpId),
                  //         FormatNumber = message.tran.c_full.format_number,
                  //         Fac = message.tran.c_full.facility_code,
                  //         CardId = message.tran.c_full.cardholder_id,
                  //         Issue = message.tran.c_full.issue_code,
                  //         Floor = message.tran.c_full.floor_number
                  //     };
                  //     await publisher.CardScanNotifyStatus(status);
                  //     isWaitingCardScan = false;
                  //     ScanAcrNo = -1;
                  //     ScanScpId = -1;
                  // }
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                  break;
                case (short)tranType.tranTypeDblCardFull:
                  // if (isWaitingCardScan && ScanScpId == message.ScpId && ScanAcrNo == message.tran.source_number)
                  // {
                  //     var status = new CardScanStatus
                  //     {
                  //         Mac = await qhw.GetMacFromComponentAsync((short)message.ScpId),
                  //         FormatNumber = message.tran.c_fulldbl.format_number,
                  //         Fac = message.tran.c_fulldbl.facility_code,
                  //         CardId = message.tran.c_fulldbl.cardholder_id,
                  //         Issue = message.tran.c_fulldbl.issue_code,
                  //         Floor = message.tran.c_fulldbl.floor_number
                  //     };
                  //     await publisher.CardScanNotifyStatus(status);
                  //     isWaitingCardScan = false;
                  //     ScanAcrNo = -1;
                  //     ScanScpId = -1;
                  // }
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                  break;
                case (short)tranType.tranTypeI64CardFull:
                  // if (isWaitingCardScan && ScanScpId == message.ScpId && ScanAcrNo == message.tran.source_number)
                  // {
                  //     var status = new CardScanStatus
                  //     {
                  //         Mac = await qhw.GetMacFromComponentAsync((short)message.ScpId),
                  //         FormatNumber = message.tran.c_fulli64.format_number,
                  //         Fac = message.tran.c_fulli64.facility_code,
                  //         CardId = message.tran.c_fulli64.cardholder_id,
                  //         Issue = message.tran.c_fulli64.issue_code,
                  //         Floor = message.tran.c_fulli64.floor_number
                  //     };
                  //     await publisher.CardScanNotifyStatus(status);
                  //     isWaitingCardScan = false;
                  //     ScanAcrNo = -1;
                  //     ScanScpId = -1;
                  // }
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                  break;
                case (short)tranType.tranTypeI64CardFullIc32:
                  // if (isWaitingCardScan && ScanScpId == message.ScpId && ScanAcrNo == message.tran.source_number)
                  // {
                  //     var status = new CardScanStatus
                  //     {
                  //         Mac = await qhw.GetMacFromComponentAsync((short)message.ScpId),
                  //         FormatNumber = message.tran.c_fulli64i32.format_number,
                  //         Fac = message.tran.c_fulli64i32.facility_code,
                  //         CardId = message.tran.c_fulli64i32.cardholder_id,
                  //         Issue = message.tran.c_fulli64i32.issue_code,
                  //         Floor = message.tran.c_fulli64i32.floor_number
                  //     };
                  //     await publisher.CardScanNotifyStatus(status);
                  //     isWaitingCardScan = false;
                  //     ScanAcrNo = -1;
                  //     ScanScpId = -1;
                  // }
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                  break;
                case (short)tranType.tranTypeCardID:
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                  break;
                case (short)tranType.tranTypeDblCardID:
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                  break;
                case (short)tranType.tranTypeI64CardID:
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                  break;
                case (short)tranType.tranTypeCoS:
                  switch (message.tran.source_type)
                  {
                    case (short)tranSrc.tranSrcACR:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                      // bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
                      // var notifierrr = scope.ServiceProvider.GetRequiredService<INotifier>();
                      // var guiddd = await bus.QueryAsync(new DoorGuidByExternalIdAndDeviceExternalIdAndVendorQuery(message.tran.source_number,(short)message.SCPId,Vendor.aero),ct);
                      // // await s.HandleFoundSioAsync(message.SCPId,message.sts_sio);
                      // Console.WriteLine(message.tran.acr);
                      // await notifierrr.SendToTopic(NotifierTopic.DOOR_STATUS,StatusHelper.ParseDoorStatus(guiddd,message.sts_acr.mode,message.sts_acr.rdr_status,message.sts_acr.strk_status,message.sts_acr.door_status,message.sts_acr.ap_status,message.sts_acr.rex_status0,message.sts_acr.rex_status1,message.sts_acr.led_mode,message.sts_acr.actl_flags,message.sts_acr.altrdr_status,message.sts_acr.actl_flags_extd,message.sts_acr.nExtFeatureType,message.sts_acr.nHardwareType,message.sts_acr.nExtFeatureStatus.ToString() ?? ""),ct);
                      break;
                    case (short)tranSrc.tranSrcSioCom:
                      // moduleService.TriggerDeviceStatus(message.SCPId, message.tran.source_number, DecodeHelper.TypeSioCommStatusDecode(message.tran.cos.status), null, null, null);
                      // publisher
                      // var module = scope.ServiceProvider.GetRequiredService<IDeviceModule>();
                      // name = await module.GetModuleNameByMacAndComponentIdAsync(mac,message.tran.source_number);
                      name = await bus.QueryAsync(new DeviceModuleNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                      break;
                    case (short)tranSrc.tranSrcMP:
                      name = await bus.QueryAsync(new InputNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                      var notifiers = scope.ServiceProvider.GetRequiredService<INotifier>();
                      var guids = await bus.QueryAsync(new InputGuidByExternalIdAndDeviceExternalIdAndVendorQuery(message.tran.source_number,(short)message.SCPId,Vendor.aero),ct);
                      await notifiers.SendToTopic(NotifierTopic.INPUT_STATUS,StatusHelper.ParseInputStatus(guids,message.tran.cos.status),ct);
                      break;
                    case (short)tranSrc.tranSrcCP:
                      name = await bus.QueryAsync(new OutputNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                      notifiers = scope.ServiceProvider.GetRequiredService<INotifier>();
                      guids = await bus.QueryAsync(new OutputGuidByExternalIdAndDeviceExternalIdAndVendorQuery(message.tran.source_number,(short)message.SCPId,Vendor.aero),ct);
                      await notifiers.SendToTopic(NotifierTopic.OUTPUT_STATUS,StatusHelper.ParseOutputStatus(guids,message.tran.cos.status),ct);
                      break;
                    default:
                      break;
                  }
                  break;
                case (short)tranType.tranTypeREX:
                  // door = scope.ServiceProvider.GetRequiredService<IDoor>();
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                  break;
                case (short)tranType.tranTypeCoSDoor:
                  // var doorstatus = new AcrStatus((short)message.ScpId, message.tran.source_number, "", DescriptionHelper.GetAccessPointStatusFlagResult(message.tran.door.ap_status));
                  // await publisher.AcrNotifyStatus(doorstatus);
                  // door = scope.ServiceProvider.GetRequiredService<IDoor>();
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                  break;
                case (short)tranType.tranTypeProcedure:

                  break;
                case (short)tranType.tranTypeUserCmnd:

                  break;
                case (short)tranType.tranTypeActivate:

                  break;
                case (short)tranType.tranTypeAcr:
                  // var modestatus = new AcrStatus((short)message.ScpId, message.tran.source_number, DescriptionHelper.GetAcrModeForStatus(message.tran.tran_code), "");
                  // await publisher.AcrNotifyStatus(modestatus);
                  // door = scope.ServiceProvider.GetRequiredService<IDoor>();
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                  break;
                case (short)tranType.tranTypeMpg:
                  name = await bus.QueryAsync(new MonitorGroupNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero), ct);
                  //bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
                  var notifierr = scope.ServiceProvider.GetRequiredService<INotifier>();
                  var guidd = await bus.QueryAsync(new MonitorGroupGuidByExternalIdAndDeviceExternalIdAndVendorQuery(message.tran.source_number,(short)message.SCPId,Vendor.aero),ct);
                  var componentss = await bus.QueryAsync(new ComponentExternalIdMapGuidByMonitorGroupExternalIdAndDeviceExternalIdAndVendorQuery(message.tran.source_number,(short)message.SCPId,Vendor.aero),ct);
                  var doorss = componentss.Where(x => x.Type == MonitorType.FORCED_OPEN || x.Type == MonitorType.HELD_OPEN).ToDictionary(x => x.ExternalId,x => x.Guid);
                  var monitorss = componentss.Where(x => x.Type == MonitorType.MONITOR_POINT).ToDictionary(x => x.ExternalId,x => x.Guid);
                  await notifierr.SendToTopic(NotifierTopic.MONITOR_GROUP_STATUS,StatusHelper.ParseMonitorGroupStatus(guidd,message.tran.mpg.mask_count,message.tran.mpg.nActiveMps, [.. message.tran.mpg.nMpList], monitorss,doorss),ct);
                  break;
                case (short)tranType.tranTypeArea:
                  break;
                case (short)tranType.tranTypeUseLimit:
                  break;
                case (short)tranType.tranTypeWebActivity:
                  break;
                case (short)tranType.tranTypeOperatingMode:
                  break;
                case (short)tranType.tranTypeCoSElevator:
                  break;
                case (short)tranType.tranTypeFileDownloadStatus:
                  break;
                case (short)tranType.tranTypeCoSElevatorAccess:
                  break;
                case (short)tranType.tranTypeAcrExtFeatureStls:
                  break;
                case (short)tranType.tranTypeAcrExtFeatureCoS:
                  break;
                case (short)tranType.tranTypeAsci:
                  break;
                case (short)tranType.tranTypeSioDiag:
                  break;
                default:
                  break;
              }
              var @event = scope.ServiceProvider.GetRequiredService<Core.Contract.Interfaces.IEvent>();
              var eve = new CreateEventDto(
                DateTimeHelper.IntToDateTimeUTC(message.tran.time),
                actor,
                TranEventHelper.GetEventModuleFromTranType((tranSrc)message.tran.source_type),
                DescriptionHelper.GetTranTypeDesc(message.tran.tran_type),
                image,
                mac,
                name,
                TranEventHelper.GetCode((tranSrc)message.tran.source_type, (tranType)message.tran.tran_type, message.tran.tran_code),
                TranEventHelper.GetRemark(message),
                string.Empty,
                Vendor.aero,
                locationGuid
              );
              await @event.CreateAsync(eve, ct);
              var notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              await notifier.TriggerToTopic(NotifierTopic.EVENT, ct);
              break;
            case (int)enSCPReplyType.enSCPReplyIDReport:
              // Handle it here
              var scp = scope.ServiceProvider.GetRequiredService<IIdReportService>();
              await scp.HandleInCommingDeviceAsync(message.id, ct);
              break;
            case (int)enSCPReplyType.enSCPReplyCommStatus:
              var noti = scope.ServiceProvider.GetRequiredService<INotifier>();
              bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              var guid = await bus.QueryAsync(new DeviceGuidByExternalIdAndVendorQuery((short)message.SCPId, Vendor.aero),ct);
              var status = new StatusDto(
                guid,
                message.comm.current_primary_comm == 3 ? Status.Online : Status.Offline
              );
              await noti.SendToTopic(DeviceNotifierTopic.STATUS, status);
              //  @event = scope.ServiceProvider.GetRequiredService<Core.Contract.Interfaces.IEvent>();
              // var noti = scope.ServiceProvider.GetRequiredService<INotifier>();
              // var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              // idevice = scope.ServiceProvider.GetRequiredService<IDevice>();
              // imap = scope.ServiceProvider.GetRequiredService<IComponentMapping>();
              // mac = await imap.GetMacByExternalIdAndEntityAndVendorAsync(message.SCPId, EntityType.Device, Vendor.aero, ct);
              // hw = await idevice.GetByMacAsync(mac, ct);
              // var status = new StatusDto(
              //   hw.Guid,
              //   message.comm.current_primary_comm == 3 ? Status.Online : Status.Offline
              // );
              // await noti.SendToTopic(DeviceNotifierTopic.STATUS, status);
              // eve = new CreateEventDto(
              //   DateTimeHelper.IntToDateTimeUTC(message.tran.time),
              //   string.Empty,
              //   TranEventHelper.GetEventModuleFromTranType((tranSrc)message.tran.source_type),
              //   DescriptionHelper.GetTranTypeDesc(message.tran.tran_type),
              //   string.Empty,
              //   mac,
              //   hw.Name,
              //   TranEventHelper.GetCode((tranSrc)message.tran.source_type, (tranType)message.tran.tran_type, message.tran.tran_code),
              //   TranEventHelper.GetRemark(message),
              //   string.Empty,
              //   Vendor.aero,
              //   hw.LocationGuid
              // );
              // await @event.CreateAsync(eve, ct);
              // await noti.TriggerToTopic(NotifierTopic.EVENT, ct);

              break;
            case (int)enSCPReplyType.enSCPReplyTranStatus:
              bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              idevice = scope.ServiceProvider.GetRequiredService<IDevice>();
              noti = scope.ServiceProvider.GetRequiredService<INotifier>();
              dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId,Vendor.aero));
              Console.WriteLine(message.tran_sts.disabled == 0 ? true : false);
              await noti.SendToTopic(DeviceNotifierTopic.DEVICE_EVENT_STATUS, new
              {
                guid = dev.Guid,
                isEnabled = message.tran_sts.disabled == 0 ? true : false
              });
              // TranStatus t = new TranStatus(
              //     message.SCPId,
              //     message.tran_sts.capacity,
              //     message.tran_sts.oldest,
              //      message.tran_sts.last_loggd,
              //      message.tran_sts.last_rprtd,
              //      message.tran_sts.disabled,
              //      message.tran_sts.disabled == 0 ? "Enable" : "Disable"
              //     );
              break;
            case (int)enSCPReplyType.enSCPReplySrSio:
              // var siostatus = new SioStatus(message.ScpId, message.sts_sio.number, DecodeHelper.TypeSioCommTranCodeDecode(message.sts_sio.com_status), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[4])), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[5])), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[6])));
              // await publisher.SioNotifyStatus(siostatus);
              bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              guid = await bus.QueryAsync(new DeviceModuleGuidByExternalIdAndDeviceExternalIdAndVendorQuery(message.sts_sio.number,(short)message.SCPId,Vendor.aero),ct);
              // await s.HandleFoundSioAsync(message.SCPId,message.sts_sio);
              Console.WriteLine(message.sts_sio.GetDebuggerDisplay());
              await notifier.SendToTopic(NotifierTopic.MODULE_STATUS,
              new StatusDto(
                  guid,
                 message.sts_sio.com_status == 5 ? Status.Online : message.sts_sio.com_status == 1 ? Status.Disabled : Status.Offline,
                  CabinetTamper:DescriptionHelper.DecodeStatusTypeCoS(message.sts_sio.ct_stat),
                  PowerMonitor:DescriptionHelper.DecodeStatusTypeCoS(message.sts_sio.pw_stat)
              ), ct);
              break;
            case (int)enSCPReplyType.enSCPReplySrMp:
              bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              notifier = notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              guid = await bus.QueryAsync(new InputGuidByExternalIdAndDeviceExternalIdAndVendorQuery(message.sts_mp.first,(short)message.SCPId,Vendor.aero),ct);
              await notifier.SendToTopic(NotifierTopic.INPUT_STATUS,StatusHelper.ParseInputStatus(guid,message.sts_mp.status[0]),ct);
              break;
            case (int)enSCPReplyType.enSCPReplySrCp:
              bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              notifier = notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              guid = await bus.QueryAsync(new OutputGuidByExternalIdAndDeviceExternalIdAndVendorQuery(message.sts_cp.first,(short)message.SCPId,Vendor.aero),ct);
              await notifier.SendToTopic(NotifierTopic.OUTPUT_STATUS,StatusHelper.ParseOutputStatus(guid,message.sts_cp.status[0]),ct);
              break;
            case (int)enSCPReplyType.enSCPReplySrAcr:
              bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              guid = await bus.QueryAsync(new DoorGuidByExternalIdAndDeviceExternalIdAndVendorQuery(message.sts_acr.number,(short)message.SCPId,Vendor.aero),ct);
              // await s.HandleFoundSioAsync(message.SCPId,message.sts_sio);
              Console.WriteLine(message.sts_acr.GetDebuggerDisplay());
              await notifier.SendToTopic(NotifierTopic.DOOR_STATUS,StatusHelper.ParseDoorStatus(guid,message.sts_acr.mode,message.sts_acr.rdr_status,message.sts_acr.strk_status,message.sts_acr.door_status,message.sts_acr.ap_status,message.sts_acr.rex_status0,message.sts_acr.rex_status1,message.sts_acr.led_mode,message.sts_acr.actl_flags,message.sts_acr.altrdr_status,message.sts_acr.actl_flags_extd,message.sts_acr.nExtFeatureType,message.sts_acr.nHardwareType,message.sts_acr.nExtFeatureStatus.ToString() ?? ""),ct);
              break;
            case (int)enSCPReplyType.enSCPReplySrTz:
              break;
            case (int)enSCPReplyType.enSCPReplySrTv:
              break;
            case (int)enSCPReplyType.enSCPReplySrMpg:
              bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              guid = await bus.QueryAsync(new MonitorGroupGuidByExternalIdAndDeviceExternalIdAndVendorQuery(message.sts_mpg.number,(short)message.SCPId,Vendor.aero),ct);
              var components = await bus.QueryAsync(new ComponentExternalIdMapGuidByMonitorGroupExternalIdAndDeviceExternalIdAndVendorQuery(message.sts_mpg.number,(short)message.SCPId,Vendor.aero),ct);
              var doors = components.Where(x => x.Type == MonitorType.FORCED_OPEN || x.Type == MonitorType.HELD_OPEN).ToDictionary(x => x.ExternalId,x => x.Guid);
              var monitors = components.Where(x => x.Type == MonitorType.MONITOR_POINT).ToDictionary(x => x.ExternalId,x => x.Guid);
              await notifier.SendToTopic(NotifierTopic.MONITOR_GROUP_STATUS,StatusHelper.ParseMonitorGroupStatus(guid,message.sts_mpg.mask_count,message.sts_mpg.num_active,[.. message.sts_mpg.active_mp_list],monitors,doors),ct);
              break;
            case (int)enSCPReplyType.enSCPReplySrArea:
              break;
            case (int)enSCPReplyType.enSCPReplySioRelayCounts:
              break;
            case (int)enSCPReplyType.enSCPReplyStrStatus:
              noti = scope.ServiceProvider.GetRequiredService<INotifier>();
              var device = scope.ServiceProvider.GetRequiredService<IDeviceAdapter>();
              var repo = scope.ServiceProvider.GetRequiredService<IDeviceRepository>();
              bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId,Vendor.aero), ct);
              var spec = await bus.QueryAsync(new AeroDriverSettingQuery(), ct);
              var data = ReplyMessageHelper.BuildStructureStatus(message.str_sts, spec);
              await noti.SendToTopic(DeviceNotifierTopic.CONFIG, data, ct);
              if (await repo.VerifyMemoryAllocateAsync(dev.Mac, data, ct))
                await device.UploadAllConfigurationAsync(dev.Mac, string.Empty, ct);
              break;
            case (int)enSCPReplyType.enSCPReplyCmndStatus:
              @event = scope.ServiceProvider.GetRequiredService<Core.Contract.Interfaces.IEvent>();
              bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              var temp = scope.ServiceProvider.GetRequiredService<Core.Contract.Interfaces.ITempDevice>();
              Console.WriteLine("Tag >> " + message.cmnd_sts.sequence_number);
              Console.WriteLine(message.cmnd_sts.status);
              Console.WriteLine(message.cmnd_sts.nak.reason);
              dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId,Vendor.aero),ct);
              if (string.IsNullOrEmpty(dev.Mac))
                mac = temp.TryGetMacById(message.SCPId);

              await @event.UpdateAdapterEventStatusAsync(
                dev.Mac,
                  message.SCPId,
                  message.cmnd_sts.sequence_number,
                  message.cmnd_sts.status == 1 ? CommandStatus.SUCCESSED : CommandStatus.FAILED,
                  message.cmnd_sts.nak != null && message.cmnd_sts.status != 1 ? DescriptionHelper.GetNakReasonDescription(message.cmnd_sts.nak.reason) : string.Empty
              );
              // var cstatus = new CmndStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId), message.cmnd_sts.sequence_number);
              // await publisher.CmndNotifyStatus(cstatus);
              notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              await notifier.TriggerToTopic(NotifierTopic.ADAPTER_EVENT, ct);
              break;
            case (int)enSCPReplyType.enSCPReplyWebConfigNetwork:
              //     bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              //     await bus.PublishAsync(new AssignIpEvent(message.SCPId, UtilitiesHelper.IntegerToIp(message.web_network.cIpAddr)), ct);
              temp = scope.ServiceProvider.GetRequiredService<ITempDevice>();
              temp.TryUpdateIp(message.SCPId, UtilitiesHelper.IntegerToIp(message.web_network.cIpAddr));
              break;
            case (int)enSCPReplyType.enSCPReplyWebConfigNotes:
              break;
            case (int)enSCPReplyType.enSCPReplyWebConfigSessionTmr:
              break;
            case (int)enSCPReplyType.enSCPReplyWebConfigWebConn:
              break;
            case (int)enSCPReplyType.enSCPReplyWebConfigAutoSave:
              break;
            case (int)enSCPReplyType.enSCPReplyWebConfigNetDiag:
              break;
            case (int)enSCPReplyType.enSCPReplyWebConfigTimeServer:
              break;
            case (int)enSCPReplyType.enSCPReplyWebConfigDiagnostics:
              break;
            case (int)enSCPReplyType.enSCPReplyWebConfigHostCommPrim:
              temp = scope.ServiceProvider.GetRequiredService<ITempDevice>();
              temp.TryUpdatePort(message.SCPId, message.web_host_comm_prim.ipclient.nPort);
              // await temp.PublishAsync(new AssignPortEvent(message.SCPId, message.web_host_comm_prim.ipclient.nPort), ct);
              break;
            default:
              break;
          }
        }
        catch (Exception ex)
        {
          ///
          var @event = scope.ServiceProvider.GetRequiredService<Core.Contract.Interfaces.IEvent>();
          await @event.InsertExceptionEventAsync(
            string.Empty,
            "Background Work",
            ex.Message,
            ex.InnerException is null ? string.Empty : ex.InnerException.ToString(),
            ex.StackTrace is null ? string.Empty : ex.StackTrace
          );
          logger.LogError(ex.Message);
        }


      }
    }


  }
}
