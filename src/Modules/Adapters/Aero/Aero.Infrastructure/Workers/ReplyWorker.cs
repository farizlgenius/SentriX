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
using Core.Contract.Queries.ComponentMapping;
using Core.Contract.Queries.Device;
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

namespace Aero.Infrastructure.Workers;

public sealed class ReplyWorker(Channel<ReplyMessage> queue, ILogger<ReplyWorker> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{
  protected async override Task ExecuteAsync(CancellationToken ct)
  {

    Console.WriteLine("Background worker started.");
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
                  // define mac , name , actor , image
                 
                  var idevice = scope.ServiceProvider.GetRequiredService<IDevice>();
                  var imap = scope.ServiceProvider.GetRequiredService<IComponentMapping>();
                  var mac = await imap.GetMacByExternalIdAndEntityAndVendorAsync(message.SCPId,EntityType.Device,Vendor.aero,ct);
                  var hw = await idevice.GetByMacAsync(mac,ct);
                  var name = string.Empty;
                  var actor = string.Empty;
                  var locationGuid = hw.LocationGuid;
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
                          // var door = scope.ServiceProvider.GetRequiredService<IDoor>();
                          // name = await door.GetNameByMacAndComponentIdAsync(mac,message.tran.source_number);
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
                          // door = scope.ServiceProvider.GetRequiredService<IDoor>();
                          // name = await door.GetNameByMacAndComponentIdAsync(mac,message.tran.source_number);
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
                          // door = scope.ServiceProvider.GetRequiredService<IDoor>();
                          // name = await door.GetNameByMacAndComponentIdAsync(mac,message.tran.source_number);
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
                          // door = scope.ServiceProvider.GetRequiredService<IDoor>();
                          // name = await door.GetNameByMacAndComponentIdAsync(mac,message.tran.source_number);
                          break;
                      case (short)tranType.tranTypeCardID:
                          // door = scope.ServiceProvider.GetRequiredService<IDoor>();
                          // name = await door.GetNameByMacAndComponentIdAsync(mac,message.tran.source_number);
                          break;
                      case (short)tranType.tranTypeDblCardID:
                          // door = scope.ServiceProvider.GetRequiredService<IDoor>();
                          // name = await door.GetNameByMacAndComponentIdAsync(mac,message.tran.source_number);
                          break;
                      case (short)tranType.tranTypeI64CardID:
                          // door = scope.ServiceProvider.GetRequiredService<IDoor>();
                          // name = await door.GetNameByMacAndComponentIdAsync(mac,message.tran.source_number);
                          break;
                      case (short)tranType.tranTypeCoS:
                          switch (message.tran.source_type)
                          {
                              case (short)tranSrc.tranSrcSioCom:
                                  // moduleService.TriggerDeviceStatus(message.SCPId, message.tran.source_number, DecodeHelper.TypeSioCommStatusDecode(message.tran.cos.status), null, null, null);
                                  // publisher
                                  // var module = scope.ServiceProvider.GetRequiredService<IDeviceModule>();
                                  // name = await module.GetModuleNameByMacAndComponentIdAsync(mac,message.tran.source_number);
                                  break;
                              case (short)tranSrc.tranSrcMP:
                                  // var mp = new MpStatus(message.ScpId, message.tran.source_number, DecodeHelper.TypeCosStatusDecode(message.tran.cos.status));
                                  // await publisher.MpNotifyStatus(mp);

                                  break;
                              case (short)tranSrc.tranSrcCP:
                                  // var cp = new CpStatus(message.ScpId, message.tran.source_number, DecodeHelper.TypeCosStatusDecode(message.tran.cos.status));
                                  // await publisher.CpNotifyStatus(cp);
                                  break;
                              default:
                                  break;
                          }
                          break;
                      case (short)tranType.tranTypeREX:
                          // door = scope.ServiceProvider.GetRequiredService<IDoor>();
                          // name = await door.GetNameByMacAndComponentIdAsync(mac,message.tran.source_number);
                          break;
                      case (short)tranType.tranTypeCoSDoor:
                          // var doorstatus = new AcrStatus((short)message.ScpId, message.tran.source_number, "", DescriptionHelper.GetAccessPointStatusFlagResult(message.tran.door.ap_status));
                          // await publisher.AcrNotifyStatus(doorstatus);
                          // door = scope.ServiceProvider.GetRequiredService<IDoor>();
                          // name = await door.GetNameByMacAndComponentIdAsync(mac,message.tran.source_number);
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
                          // name = await door.GetNameByMacAndComponentIdAsync(mac,message.tran.source_number);
                          break;
                      case (short)tranType.tranTypeMpg:

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
              await @event.CreateAsync(eve,ct);
              var notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              await notifier.TriggerToTopic(NotifierTopic.EVENT,ct);
              break;
            case (int)enSCPReplyType.enSCPReplyIDReport:
              // Handle it here
              var scp = scope.ServiceProvider.GetRequiredService<IIdReportService>();
              await scp.HandleInCommingDeviceAsync(message.id, ct);
              break;
            case (int)enSCPReplyType.enSCPReplyCommStatus:
              // var @event = scope.ServiceProvider.GetRequiredService<Events.Contract.Interfaces.IEvent>();
              // var repo = scope.ServiceProvider.GetRequiredService<IScpRepository>();
              // var data = await repo.GetMacAndLocationIdByScpIdAsync(message.SCPId);
              // await @event.AddEventAsync(
              //     DateTime.UtcNow,
              //     string.Empty,
              //     EventModule.DEVICE,
              //     DescriptionHelper.GetMessageTypeDesc(message.ReplyType),
              //     string.Empty,
              //     m,
              //     string.Empty,
              //     DescriptionHelper.GetCommStatusDesc(message.comm.status),
              //     l
              // );
              // Status Query
              var noti = scope.ServiceProvider.GetRequiredService<INotifier>();
              var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              var id = await bus.QueryAsync(new InternalIdByExternalIdAndEntityAndVendorQuery((short)message.SCPId,EntityType.Device,Vendor.aero));
              var guid = await bus.QueryAsync(new DeviceGuidByIdQuery(id));
              var status = new StatusDto(
                guid,
                message.comm.current_primary_comm == 3 ? Status.Online : Status.Offline
              );
              await noti.SendToTopic(DeviceNotifierTopic.STATUS,status);
              break;
            case (int)enSCPReplyType.enSCPReplyTranStatus:
              //     notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              //     var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              //     // TranStatus t = new TranStatus(
              //     //     message.SCPId,
              //     //     message.tran_sts.capacity,
              //     //     message.tran_sts.oldest,
              //     //      message.tran_sts.last_loggd,
              //     //      message.tran_sts.last_rprtd,
              //     //      message.tran_sts.disabled,
              //     //      message.tran_sts.disabled == 0 ? "Enable" : "Disable"
              //     //     );
              //     await notifier.SendToTopic(
              //         NotifierTopic.EVENT_STATUS,
              //         new EventStatusDto(
              //             await bus.QueryAsync(new IdByComponentIdQuery(message.SCPId)),
              //             message.tran_sts.disabled == 0 ? true : false
              //             )
              //         );
              break;
            case (int)enSCPReplyType.enSCPReplySrSio:
              //     // var siostatus = new SioStatus(message.ScpId, message.sts_sio.number, DecodeHelper.TypeSioCommTranCodeDecode(message.sts_sio.com_status), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[4])), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[5])), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[6])));
              //     // await publisher.SioNotifyStatus(siostatus);
              //     // var s = scope.ServiceProvider.GetRequiredService<IModule>();
              //     notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              //     // await s.HandleFoundSioAsync(message.SCPId,message.sts_sio);
              //     await notifier.SendToTopic(NotifierTopic.MODULE_STATUS, new StatusDto(
              //         message.SCPId,
              //         message.sts_sio.number,
              //         TranHelper.GetCode((tranSrc)message.tran.source_type, tranType.tranTypeSioComm, message.sts_sio.com_status),
              //         DescriptionHelper.DecodeStatusTypeCoS(message.sts_sio.ct_stat),
              //         DescriptionHelper.DecodeStatusTypeCoS(message.sts_sio.pw_stat),
              //         string.Empty
              //     ), ct);
              break;
            case (int)enSCPReplyType.enSCPReplySrMp:
              // var mpstatus = new MpStatus(message.ScpId, message.sts_mp.first, DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_mp.status[0])));
              // await publisher.MpNotifyStatus(mpstatus);
              break;
            case (int)enSCPReplyType.enSCPReplySrCp:
              // var cpstatus = new CpStatus(message.ScpId, message.sts_cp.first, DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_cp.status[0])));
              // await publisher.CpNotifyStatus(cpstatus);
              break;
            case (int)enSCPReplyType.enSCPReplySrAcr:
              // var acrstatus = new AcrStatus((short)message.ScpId, message.sts_acr.number, DescriptionHelper.GetAcrModeForStatus(message.sts_acr.door_status), DescriptionHelper.GetAccessPointStatusFlagResult((byte)message.sts_acr.ap_status));
              // await publisher.AcrNotifyStatus(acrstatus);
              break;
            case (int)enSCPReplyType.enSCPReplySrTz:
              break;
            case (int)enSCPReplyType.enSCPReplySrTv:
              break;
            case (int)enSCPReplyType.enSCPReplySrMpg:
              break;
            case (int)enSCPReplyType.enSCPReplySrArea:
              break;
            case (int)enSCPReplyType.enSCPReplySioRelayCounts:
              break;
            case (int)enSCPReplyType.enSCPReplyStrStatus:
              noti = scope.ServiceProvider.GetRequiredService<INotifier>();
              var mapping = scope.ServiceProvider.GetRequiredService<IComponentMapping>();
              var repo = scope.ServiceProvider.GetRequiredService<IDeviceRepository>();
              bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
              mac = await mapping.GetMacByExternalIdAndEntityAndVendorAsync(message.SCPId,EntityType.Device,Vendor.aero,ct);
              var spec = await bus.QueryAsync(new AeroDriverSettingQuery(),ct);
              var data = ReplyMessageHelper.BuildStructureStatus(message.str_sts,spec);
              await noti.SendToTopic(DeviceNotifierTopic.CONFIG, data, ct);
              await repo.VerifyMemoryAllocateAsync(mac,data,ct);
              break;
            case (int)enSCPReplyType.enSCPReplyCmndStatus:
                  @event = scope.ServiceProvider.GetRequiredService<Core.Contract.Interfaces.IEvent>();
                  mapping = scope.ServiceProvider.GetRequiredService<Core.Contract.Interfaces.IComponentMapping>();
                  var temp = scope.ServiceProvider.GetRequiredService<Core.Contract.Interfaces.ITempDevice>();
                  Console.WriteLine("Tag >> " + message.cmnd_sts.sequence_number);
                  Console.WriteLine(message.cmnd_sts.status);
                  Console.WriteLine(message.cmnd_sts.nak.reason);
                  mac = await mapping.GetMacByExternalIdAndEntityAndVendorAsync(message.SCPId,EntityType.Device,Vendor.aero,ct);
                  if(string.IsNullOrEmpty(mac))
                    mac = temp.TryGetMacById(message.SCPId);

                  await @event.UpdateAdapterEventStatusAsync(
                    mac,
                      message.SCPId,
                      message.cmnd_sts.sequence_number,
                      message.cmnd_sts.status == 1 ? CommandStatus.SUCCESSED : CommandStatus.FAILED,
                      message.cmnd_sts.nak != null && message.cmnd_sts.status != 1 ? DescriptionHelper.GetNakReasonDescription(message.cmnd_sts.nak.reason) : string.Empty
                  );
                  // var cstatus = new CmndStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId), message.cmnd_sts.sequence_number);
                  // await publisher.CmndNotifyStatus(cstatus);
                  notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
              await notifier.TriggerToTopic(NotifierTopic.ADAPTER_EVENT,ct);
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
            "Aero Background Work",
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
