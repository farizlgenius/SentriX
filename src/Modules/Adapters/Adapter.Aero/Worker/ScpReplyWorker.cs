using System;
using System.Threading.Channels;
using Adapter.Abstraction.Events;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Mapper;
using Adapter.Aero.Model;
using AeroAdapter.Application.Interfaces;
using Device.Contract.Interfaces;
using Device.Contract.Queries;
using Events.Contract.Constants;
using Events.Contract.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notifier.Contract.Constants;
using Notifier.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Messaging;

namespace Adapter.Aero.Worker;


public sealed class ScpReplyWorker(Channel<SCPReplyMessageDto> queue, ILogger<ScpReplyWorker> logger, IServiceScopeFactory scopeFactory) : BackgroundService
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
                            break;
                        case (int)enSCPReplyType.enSCPReplyTransaction:
                            switch (message.tran.tran_type)
                            {
                                case (short)tranType.tranTypeSioComm:
                                    var @e = scope.ServiceProvider.GetRequiredService<Events.Contract.Interfaces.IEvent>();
                                    var b = scope.ServiceProvider.GetRequiredService<IMessageBus>();
                                    var r = scope.ServiceProvider.GetRequiredService<IScpRepository>();
                                    var d = await r.GetMacAndLocationIdByScpIdAsync(message.SCPId);
                                    var (mac,loc) = d == null ? (string.Empty,0) : d.Value;
                                    await @e.AddEventAsync(
                                        DateTimeOffset.FromUnixTimeSeconds(message.tran.time).UtcDateTime,
                                        string.Empty,
                                        EventModule.MODULE,
                                        DescriptionHelper.GetTranTypeDesc(message.tran.tran_type),
                                        string.Empty,
                                        mac,
                                        string.Empty,
                                        string.Empty,
                                        loc
                                    );
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

                                    break;
                                case (short)tranType.tranTypeCardID:
                                    break;
                                case (short)tranType.tranTypeDblCardID:
                                    break;
                                case (short)tranType.tranTypeI64CardID:
                                    break;
                                case (short)tranType.tranTypeCoS:
                                    switch (message.tran.source_type)
                                    {
                                        case (short)tranSrc.tranSrcSioCom:
                                            // moduleService.TriggerDeviceStatus(message.SCPId, message.tran.source_number, DecodeHelper.TypeSioCommStatusDecode(message.tran.cos.status), null, null, null);
                                            // publisher
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
                                    
                                    break;
                                case (short)tranType.tranTypeCoSDoor:
                                    // var doorstatus = new AcrStatus((short)message.ScpId, message.tran.source_number, "", DescriptionHelper.GetAccessPointStatusFlagResult(message.tran.door.ap_status));
                                    // await publisher.AcrNotifyStatus(doorstatus);
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
                            // await publisher.EventNotifyRecieve();
                            break;
                        case (int)enSCPReplyType.enSCPReplyIDReport:
                            var scp = scope.ServiceProvider.GetRequiredService<IScp>();
                            await scp.HandleIdReport(message.id);
                            break;
                        case (int)enSCPReplyType.enSCPReplyCommStatus:
                            var @event = scope.ServiceProvider.GetRequiredService<Events.Contract.Interfaces.IEvent>();
                            var repo = scope.ServiceProvider.GetRequiredService<IScpRepository>();
                            var data = await repo.GetMacAndLocationIdByScpIdAsync(message.SCPId);
                            var (m, l) = data == null ? (string.Empty, 0) : data.Value;
                            await @event.AddEventAsync(
                                DateTime.UtcNow,
                                string.Empty,
                                EventModule.DEVICE,
                                DescriptionHelper.GetMessageTypeDesc(message.ReplyType),
                                string.Empty,
                                m,
                                string.Empty,
                                DescriptionHelper.GetCommStatusDesc(message.comm.status),
                                l
                            );
                            break;
                        case (int)enSCPReplyType.enSCPReplyTranStatus:
                            // TranStatus t = new TranStatus(
                            //     message.ScpId,
                            //     message.tran_sts.capacity,
                            //     message.tran_sts.oldest,
                            //      message.tran_sts.last_loggd,
                            //      message.tran_sts.last_rprtd,
                            //      message.tran_sts.disabled,
                            //      message.tran_sts.disabled == 0 ? "Enable" : "Disable"
                            //     );
                            // await publisher.ScpNotifyTranStatus(t);
                            break;
                        case (int)enSCPReplyType.enSCPReplySrSio:
                            // var siostatus = new SioStatus(message.ScpId, message.sts_sio.number, DecodeHelper.TypeSioCommTranCodeDecode(message.sts_sio.com_status), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[4])), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[5])), DecodeHelper.TypeCosStatusDecode(Convert.ToByte(message.sts_sio.ip_stat[6])));
                            // await publisher.SioNotifyStatus(siostatus);
                            var s = scope.ServiceProvider.GetRequiredService<ISio>();
                            var srepo = scope.ServiceProvider.GetRequiredService<ISioRepository>();
                            var notifier = scope.ServiceProvider.GetRequiredService<INotifier>();
                            await s.HandleFoundSioAsync(message.SCPId,message.sts_sio);
                            await notifier.SendToTopic(NotifierTopic.MODULE_STATUS,new StatusDto(
                                await srepo.GetModuleIdByScpIdAndSioIdAsync(message.SCPId,message.sts_sio.number),
                                message.sts_sio.number,
                                TranCodeHelper.GetDesc(tranType.tranTypeSioComm,message.sts_sio.com_status),
                                DescriptionHelper.DecodeStatusTypeCoS(message.sts_sio.ct_stat),
                                DescriptionHelper.DecodeStatusTypeCoS(message.sts_sio.pw_stat),
                                string.Empty
                            ),ct);
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
                            scp = scope.ServiceProvider.GetRequiredService<IScp>();
                            if(await scp.VerifySCPStructureMemoryAllocate(message.SCPId, message.str_sts))
                            {
                                await scp.InitialScpConfigurationAsync((short)message.SCPId);
                                await scp.VerifyScpComponentAsync(message.SCPId);
                            }
                            break;
                        case (int)enSCPReplyType.enSCPReplyCmndStatus:
                            var writer = scope.ServiceProvider.GetRequiredService<IWriterRepository>();
                            repo = scope.ServiceProvider.GetRequiredService<IScpRepository>();
                            await writer.UpdateWriterAuditAsync(
                                message.SCPId,
                                await repo.MacByScpIdAsync(message.SCPId,ct),
                                message.cmnd_sts.sequence_number,
                                message.cmnd_sts
                            );
                            // var cstatus = new CmndStatus(await qhw.GetMacFromComponentAsync((short)message.ScpId), message.cmnd_sts.sequence_number);
                            // await publisher.CmndNotifyStatus(cstatus);
                            break;
                        case (int)enSCPReplyType.enSCPReplyWebConfigNetwork:
                            var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
                            repo = scope.ServiceProvider.GetRequiredService<IScpRepository>();
                            await bus.PublishAsync(new AssignIpEvent(await repo.MacByScpIdAsync(message.SCPId,ct), UtilitiesHelper.IntegerToIp(message.web_network.cIpAddr)), ct);
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
                            bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
                            repo = scope.ServiceProvider.GetRequiredService<IScpRepository>();
                            await bus.PublishAsync(new AssignPortEvent(await repo.MacByScpIdAsync(message.SCPId,ct), message.web_host_comm_prim.ipclient.nPort), ct);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ///
                    logger.LogError(ex.Message);
                }


            }
        }


    }
}
