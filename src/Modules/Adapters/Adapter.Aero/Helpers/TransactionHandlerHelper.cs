using System;
using System.Threading.Channels;
using Adapter.Aero.Listener;
using Adapter.Aero.Model;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;

namespace Adapter.Aero.Helpers;

public sealed class TransactionHandlerHelper
{
      public static void SCPReplyTransactionHandler(SCPReplyMessageDto message, Channel<SCPReplyMessageDto> queue, ILogger<ReplyMessageListener> logger)
      {
            switch (message.tran.tran_type)
            {      
                case (short)tranType.tranTypeSys:
                  //   ProcessAeroTransactionHelper.ProcessTypeSys(message);
                    break;
                case (short)tranType.tranTypeSioComm:
                  //   {
                  //       using var scope = scopeFactory.CreateScope();
                  //       var moduleService = scope.ServiceProvider.GetRequiredService<IModuleService>();
                  //       moduleService.GetSioStatus(message.SCPId, message.tran.source_number);
                  //       ProcessAeroTransactionHelper.ProcessTypeSioComm(message);
                  //       break;
                  //   }
                case (short)tranType.tranTypeCardBin:
                  //   ProcessAeroTransactionHelper.ProcessTypeCardBin(message);
                    queue.Writer.TryWrite(message);
                    break;
                case (short)tranType.tranTypeCardBcd:
                  //   ProcessAeroTransactionHelper.ProcessTypeCardBcd(message);
                    break;
                case (short)tranType.tranTypeCardFull:
                  //   ProcessAeroTransactionHelper.ProcessTypeCardFull(message);
                    queue.Writer.TryWrite(message);
                    break;
                case (short)tranType.tranTypeDblCardFull:
                  //   ProcessAeroTransactionHelper.ProcessTypeDblCardFull(message);
                    queue.Writer.TryWrite(message);
                    break;
                case (short)tranType.tranTypeI64CardFull:
                  //   ProcessAeroTransactionHelper.ProcessTypei64CardFull(message);
                    queue.Writer.TryWrite(message);
                    break;
                case (short)tranType.tranTypeI64CardFullIc32:
                  //   ProcessAeroTransactionHelper.ProcessTypei64CardFullc32(message);
                    queue.Writer.TryWrite(message);
                    break;
                case (short)tranType.tranTypeCardID:
                  //   ProcessAeroTransactionHelper.ProcessTypeCardID(message);
                    queue.Writer.TryWrite(message);
                    break;
                case (short)tranType.tranTypeDblCardID:
                  //   ProcessAeroTransactionHelper.ProcessTypeDblCardID(message);
                    queue.Writer.TryWrite(message);
                    break;
                case (short)tranType.tranTypeI64CardID:
                  //   ProcessAeroTransactionHelper.tranTypei64CardID(message);
                    queue.Writer.TryWrite(message);
                    break;
                case (short)tranType.tranTypeCoS:
                    switch (message.tran.source_type)
                    {
                        case (short)tranSrc.tranSrcSioCom:
                            queue.Writer.TryWrite(message);
                            break;
                        case (short)tranSrc.tranSrcMP:
                            queue.Writer.TryWrite(message);
                            break;
                        case (short)tranSrc.tranSrcCP:
                            queue.Writer.TryWrite(message);
                            break;
                        default:
                            break;
                    }
                  //   ProcessAeroTransactionHelper.tranTypeCos(message);
                    queue.Writer.TryWrite(message);
                    break;
                case (short)tranType.tranTypeREX:
                    //   ProcessAeroTransactionHelper.tranTypeRex(message);
                    break;
                case (short)tranType.tranTypeCoSDoor:
                    queue.Writer.TryWrite(message);
                    //   ProcessAeroTransactionHelper.tranTypeCosDoor(message);
                    break;
                case (short)tranType.tranTypeProcedure:
                  //   ProcessAeroTransactionHelper.tranTypeProcedure(message);
                    break;
                case (short)tranType.tranTypeUserCmnd:
                  //   ProcessAeroTransactionHelper.tranTypeUserCmnd(message);
                    break;
                case (short)tranType.tranTypeActivate:
                  //   ProcessAeroTransactionHelper.tranTypeActivate(message);
                    break;
                case (short)tranType.tranTypeAcr:
                    queue.Writer.TryWrite(message);
                  //   ProcessAeroTransactionHelper.tranTypeAcr(message);
                    break;
                case (short)tranType.tranTypeMpg:
                  //   ProcessAeroTransactionHelper.tranTypeMpg(message);
                    break;
                case (short)tranType.tranTypeArea:
                  //   ProcessAeroTransactionHelper.tranTypeArea(message);
                    break;
                case (short)tranType.tranTypeUseLimit:
                  //   ProcessAeroTransactionHelper.tranTypeUseLimit(message);
                    break;
                case (short)tranType.tranTypeWebActivity:
                  //   ProcessAeroTransactionHelper.tranTypeWebActivity(message);
                    break;
                case (short)tranType.tranTypeOperatingMode:
                  //   ProcessAeroTransactionHelper.tranTypeOperatingMode(message);
                    break;
                case (short)tranType.tranTypeCoSElevator:
                  //   ProcessAeroTransactionHelper.tranTypeCoSElevator(message);
                    break;
                case (short)tranType.tranTypeFileDownloadStatus:
                  //   ProcessAeroTransactionHelper.tranTypeFileDownloadStatus(message);
                    break;
                case (short)tranType.tranTypeCoSElevatorAccess:
                  //   ProcessAeroTransactionHelper.tranTypeCoSElevatorAccess(message);
                    break;
                case (short)tranType.tranTypeAcrExtFeatureStls:
                  //   ProcessAeroTransactionHelper.tranTypeAcrExtFeatureStls(message);
                    break;
                case (short)tranType.tranTypeAcrExtFeatureCoS:
                  //   ProcessAeroTransactionHelper.tranTypeAcrExtFeatureCoS(message);
                    break;
                case (short)tranType.tranTypeAsci:
                  //   ProcessAeroTransactionHelper.tranTypeAsci(message);
                    break;
                case (short)tranType.tranTypeSioDiag:
                  //   ProcessAeroTransactionHelper.tranTypeSioDiag(message);
                    break;
                default:
                    break;
            }
      }
}
