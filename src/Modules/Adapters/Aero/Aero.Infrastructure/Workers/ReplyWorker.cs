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
using Core.Contract.Queries.Area;
using Core.Contract.Queries.Device;
using Core.Contract.Queries.DeviceModule;
using Core.Contract.Queries.Door;
using Core.Contract.Queries.Input;
using Core.Contract.Queries.MonitorGroup;
using Core.Contract.Queries.Output;
using Core.Contract.Queries.Time;
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
          var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
          var notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
          switch (message.ReplyType)
          {

            case (int)enSCPReplyType.enSCPReplyNAK:
              Console.WriteLine(message.nak.description_code);
              break;
            case (int)enSCPReplyType.enSCPReplyTransaction:
              Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<< Tran Type: " + DescriptionHelper.GetTranTypeDesc(message.tran.tran_type));
              // define mac , name , actor , image
              var dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId, Vendor.aero), ct);
              var mac = dev.Mac;
              var name = dev.Name;
              var actor = string.Empty;
              var locationGuid = dev.LocationGuid;
              var image = string.Empty;
              
              #region News Switch Transaction Type
              switch (message.tran.source_type)
              {
                case (short)tranSrc.tranSrcScpDiag:
                  switch (message.tran.tran_type)
                  {
                    case (short)tranType.tranTypeSys:
                      break;
                    case (short)tranType.tranTypeWebActivity:
                      break;
                    case (short)tranType.tranTypeOperatingMode:
                      break;
                    case (short)tranType.tranTypeFileDownloadStatus:
                      break;
                    default:
                      break;
                  }
                  break;
                case (short)tranSrc.tranSrcScpLcl:
                  // TypeCos
                  await notifier.SendToTopic(NotifierTopic.DEVICE_POWER_STATUS,StatusHelper.ParseDevicePowerStatus(dev.Guid,message.tran.cos.status),ct);
                  break;
                case (short)tranSrc.tranSrcSioDiag:
                  // TypeSioDiag
                  name = await bus.QueryAsync(new DeviceModuleNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                  break;
                case (short)tranSrc.tranSrcSioCom:
                  // TypeSioComm
                  name = await bus.QueryAsync(new DeviceModuleNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                  var guidd = await bus.QueryAsync(new DeviceModuleGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct); 
                  await notifier.SendToTopic(NotifierTopic.MODULE_STATUS,StatusHelper.ParseModuleCommStatus(guidd,message.tran.s_comm),ct);
                  break;
                case (short)tranSrc.tranSrcSioTmpr:
                  // TypeCos
                  name = await bus.QueryAsync(new DeviceModuleNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                  guidd = await bus.QueryAsync(new DeviceModuleGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct); 

                  await notifier.SendToTopic(NotifierTopic.MODULE_TAMPER_STATUS,StatusHelper.ParseModuleTamperStatus(guidd,message.tran.cos.status),ct);
                  break;
                case (short)tranSrc.tranSrcSioPwr:
                  // TypeCos
                  name = await bus.QueryAsync(new DeviceModuleNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                  guidd = await bus.QueryAsync(new DeviceModuleGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct); 
                  await notifier.SendToTopic(NotifierTopic.MODULE_POWER_STATUS,StatusHelper.ParseModulePowerStatus(guidd,message.tran.cos.status),ct);
                  break;
                case (short)tranSrc.tranSrcMP:
                  // TypeCos
                  name = await bus.QueryAsync(new InputNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                  guidd = await bus.QueryAsync(new InputGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct); 
                  await notifier.SendToTopic(NotifierTopic.INPUT_STATUS,StatusHelper.ParseInputStatus(guidd,message.tran.cos.status),ct);
                  break;
                case (short)tranSrc.tranSrcCP:
                  // TypeCos
                  name = await bus.QueryAsync(new OutputNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                  guidd = await bus.QueryAsync(new OutputGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct); 
                  await notifier.SendToTopic(NotifierTopic.OUTPUT_STATUS,StatusHelper.ParseOutputStatus(guidd,message.tran.cos.status),ct);
                  break;
                case (short)tranSrc.tranSrcACR:
                  switch (message.tran.tran_type)
                  {
                    case (short)tranType.tranTypeCardBin:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeCardBcd:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeCardFull:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeCardID:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeREX:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeUserCmnd:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeAcr:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeDblCardFull:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeDblCardID:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeUseLimit:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeOAL:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeCoSElevator:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeCoSElevatorAccess:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeI64CardFull:
                      name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeI64CardID:
                    name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeI64CardFullIc32:
                    name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeAcrExtFeatureStls:
                    name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    case (short)tranType.tranTypeAcrExtFeatureCoS:
                    name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                      break;
                    default:
                      break;
                  }
                  break;
                case (short)tranSrc.tranSrcAcrTmpr:
                  // TypeCos
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                  guidd = await bus.QueryAsync(new DoorGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct); 
                  await notifier.SendToTopic(NotifierTopic.DOOR_READER_STATUS,StatusHelper.ParseReaderStatus(guidd,message.tran.cos.status),ct);
                  break;
                case (short)tranSrc.tranSrcAcrDoor:
                  // TypeCosDoor
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                  guidd = await bus.QueryAsync(new DoorGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct); 
                  await notifier.SendToTopic(NotifierTopic.DOOR_COS_STATUS,StatusHelper.ParseDoorChangeOfStateStatus(guidd,message.tran.tran_code,message.tran.door.ap_status),ct);
                  break;
                case (short)tranSrc.tranSrcAcrRex0:
                  // TypeCos
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                  guidd = await bus.QueryAsync(new DoorGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct); 
                  await notifier.SendToTopic(NotifierTopic.DOOR_COS_STATUS,StatusHelper.ParseRexChangeOfStateStatus(guidd,message.tran.cos.status),ct);
                  break;
                case (short)tranSrc.tranSrcAcrRex1:
                  // TypeCos
                  // TypeCos
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                  guidd = await bus.QueryAsync(new DoorGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct); 
                  await notifier.SendToTopic(NotifierTopic.DOOR_COS_STATUS,StatusHelper.ParseRexChangeOfStateStatus(guidd,message.tran.cos.status),ct);
                  break;
                case (short)tranSrc.tranSrcTimeZone:
                  // TypeActivate
                  name = await bus.QueryAsync(new TimeZoneNameByExternalIdQuery(message.tran.source_number),ct);
                  guidd = await bus.QueryAsync(new DoorGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct); 
                  await notifier.SendToTopic(NotifierTopic.DOOR_COS_STATUS,StatusHelper.ParseRexChangeOfStateStatus(guidd,message.tran.cos.status),ct);
                  break;
                case (short)tranSrc.tranSrcProcedure:
                  // TypeProcedure
                  break;
                case (short)tranSrc.tranSrcTrigger:
                  // TypeActivate
                  break;
                case (short)tranSrc.tranSrcTrigVar:
                  // TypeActivate
                  break;
                case (short)tranSrc.tranSrcMPG:
                  // TypeMPG
                  name = await bus.QueryAsync(new MonitorGroupNameByExternalIdAndMacAndVendorQuery(message.tran.source_number, mac, Vendor.aero), ct);
                  //bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
                  guidd = await bus.QueryAsync(new MonitorGroupGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number, mac, Vendor.aero), ct);
                  var componentss = await bus.QueryAsync(new ComponentExternalIdMapGuidByMonitorGroupExternalIdAndDeviceExternalIdAndVendorQuery(message.tran.source_number, (short)message.SCPId, Vendor.aero), ct);
                  var doorss = componentss.Where(x => x.Type == MonitorType.FORCED_OPEN || x.Type == MonitorType.HELD_OPEN).ToDictionary(x => x.ExternalId, x => x.Guid);
                  var monitorss = componentss.Where(x => x.Type == MonitorType.MONITOR_POINT).ToDictionary(x => x.ExternalId, x => x.Guid);
                  await notifier.SendToTopic(NotifierTopic.MONITOR_GROUP_STATUS, StatusHelper.ParseMonitorGroupStatus(guidd, message.tran.mpg.mask_count, message.tran.mpg.nActiveMps, [.. message.tran.mpg.nMpList], monitorss, doorss), ct);
                  break;
                case (short)tranSrc.tranSrcArea:
                  // TypeArea
                  name = await bus.QueryAsync(new AreaNameByExternalIdAndMacAndVendorQuery(message.tran.source_number, mac, Vendor.aero), ct);
                  guidd = await bus.QueryAsync(new AreaGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct); 
                  await notifier.SendToTopic(NotifierTopic.AREA_STATUS,StatusHelper.ParseAreaChangeOfStateStatus(guidd,message.tran.area.status,message.tran.area.occupancy),ct);
                  break;
                case (short)tranSrc.tranSrcAcrTmprAlt:
                  // TypeCos
                  // TypeCos
                  name = await bus.QueryAsync(new DoorNameByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct);
                  guidd = await bus.QueryAsync(new DoorGuidByExternalIdAndMacAndVendorQuery(message.tran.source_number,mac,Vendor.aero),ct); 
                  await notifier.SendToTopic(NotifierTopic.DOOR_READER_STATUS,StatusHelper.ParseReaderStatus(guidd,message.tran.cos.status),ct);
                  break;
                case (short)tranSrc.tranSrcLoginService:
                  // TypeCos
                  break;
                default:
                  break;
              }
              #endregion
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
              await notifier.TriggerToTopic(NotifierTopic.EVENT, ct);
              break;
            case (int)enSCPReplyType.enSCPReplyIDReport:
              // Handle it here
              var scp = scope.ServiceProvider.GetRequiredService<IIdReportService>();
              await scp.HandleInCommingDeviceAsync(message.id, ct);
              break;
            case (int)enSCPReplyType.enSCPReplyCommStatus:
              var noti = scope.ServiceProvider.GetRequiredService<INotifier>();
              var guid = await bus.QueryAsync(new DeviceGuidByExternalIdAndVendorQuery((short)message.SCPId, Vendor.aero), ct);
              var status = new StatusDto(
                guid,
                message.comm.current_primary_comm == 3 ? Status.Online : Status.Offline
              );
              await noti.SendToTopic(DeviceNotifierTopic.STATUS, status,ct);
              break;
            case (int)enSCPReplyType.enSCPReplyTranStatus:
              noti = scope.ServiceProvider.GetRequiredService<INotifier>();
              dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId, Vendor.aero));
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
              notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId, Vendor.aero), ct);
              guid = await bus.QueryAsync(new DeviceModuleGuidByExternalIdAndMacAndVendorQuery(message.sts_sio.number, dev.Mac, Vendor.aero), ct);
              // await s.HandleFoundSioAsync(message.SCPId,message.sts_sio);
              Console.WriteLine(message.sts_sio.GetDebuggerDisplay());
              await notifier.SendToTopic(NotifierTopic.MODULE_STATUS,
              new StatusDto(
                  guid,
                 message.sts_sio.com_status == 5 ? Status.Online : message.sts_sio.com_status == 1 ? Status.Disabled : Status.Offline,
                  CabinetTamper: DescriptionHelper.DecodeStatusTypeCoS(message.sts_sio.ct_stat),
                  PowerMonitor: DescriptionHelper.DecodeStatusTypeCoS(message.sts_sio.pw_stat)
              ), ct);
              break;
            case (int)enSCPReplyType.enSCPReplySrMp:
              notifier = notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId, Vendor.aero), ct);
              guid = await bus.QueryAsync(new InputGuidByExternalIdAndMacAndVendorQuery(message.sts_mp.first, dev.Mac, Vendor.aero), ct);
              await notifier.SendToTopic(NotifierTopic.INPUT_STATUS, StatusHelper.ParseInputStatus(guid, message.sts_mp.status[0]), ct);
              break;
            case (int)enSCPReplyType.enSCPReplySrCp:
              notifier = notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId, Vendor.aero), ct);
              guid = await bus.QueryAsync(new OutputGuidByExternalIdAndMacAndVendorQuery(message.sts_cp.first, dev.Mac, Vendor.aero), ct);
              await notifier.SendToTopic(NotifierTopic.OUTPUT_STATUS, StatusHelper.ParseOutputStatus(guid, message.sts_cp.status[0]), ct);
              break;
            case (int)enSCPReplyType.enSCPReplySrAcr:
              notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId, Vendor.aero), ct);
              guid = await bus.QueryAsync(new DoorGuidByExternalIdAndMacAndVendorQuery(message.sts_acr.number, dev.Mac, Vendor.aero), ct);
              // await s.HandleFoundSioAsync(message.SCPId,message.sts_sio);
              Console.WriteLine(message.sts_acr.GetDebuggerDisplay());
              await notifier.SendToTopic(NotifierTopic.DOOR_STATUS, StatusHelper.ParseDoorStatus(guid, message.sts_acr.mode, message.sts_acr.rdr_status, message.sts_acr.strk_status, message.sts_acr.door_status, message.sts_acr.ap_status, message.sts_acr.rex_status0, message.sts_acr.rex_status1, message.sts_acr.led_mode, message.sts_acr.actl_flags, message.sts_acr.altrdr_status, message.sts_acr.actl_flags_extd, message.sts_acr.nExtFeatureType, message.sts_acr.nHardwareType, message.sts_acr.nExtFeatureStatus.ToString() ?? ""), ct);
              break;
            case (int)enSCPReplyType.enSCPReplySrTz:
              break;
            case (int)enSCPReplyType.enSCPReplySrTv:
              break;
            case (int)enSCPReplyType.enSCPReplySrMpg:
              notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId, Vendor.aero), ct);
              guid = await bus.QueryAsync(new MonitorGroupGuidByExternalIdAndMacAndVendorQuery(message.sts_mpg.number, dev.Mac, Vendor.aero), ct);
              var components = await bus.QueryAsync(new ComponentExternalIdMapGuidByMonitorGroupExternalIdAndDeviceExternalIdAndVendorQuery(message.sts_mpg.number, (short)message.SCPId, Vendor.aero), ct);
              var doors = components.Where(x => x.Type == MonitorType.FORCED_OPEN || x.Type == MonitorType.HELD_OPEN).ToDictionary(x => x.ExternalId, x => x.Guid);
              var monitors = components.Where(x => x.Type == MonitorType.MONITOR_POINT).ToDictionary(x => x.ExternalId, x => x.Guid);
              await notifier.SendToTopic(NotifierTopic.MONITOR_GROUP_STATUS, StatusHelper.ParseMonitorGroupStatus(guid, message.sts_mpg.mask_count, message.sts_mpg.num_active, [.. message.sts_mpg.active_mp_list], monitors, doors), ct);
              break;
            case (int)enSCPReplyType.enSCPReplySrArea:
              break;
            case (int)enSCPReplyType.enSCPReplySioRelayCounts:
              break;
            case (int)enSCPReplyType.enSCPReplyStrStatus:
              noti = scope.ServiceProvider.GetRequiredService<INotifier>();
              var device = scope.ServiceProvider.GetRequiredService<IDeviceAdapter>();
              var repo = scope.ServiceProvider.GetRequiredService<IDeviceRepository>();
              dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId, Vendor.aero), ct);
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
              dev = await bus.QueryAsync(new DeviceByExternalIdAndVendorQuery((short)message.SCPId, Vendor.aero), ct);
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
          var @event = scope.ServiceProvider.GetRequiredService<Core.Contract.Interfaces.IEvent>();
          await @event.InsertExceptionEventAsync(
            string.Empty,
            "AERO WORKER",
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
