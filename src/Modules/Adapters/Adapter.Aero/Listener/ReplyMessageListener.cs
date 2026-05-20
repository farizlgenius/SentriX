using System;
using System.Threading.Channels;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model;
using Adapter.Aero.Services;
using Device.Contract.Queries;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedKernel.Messaging;

namespace Adapter.Aero.Listener;


public sealed class ReplyMessageListener(ILogger<ReplyMessageListener> logger,Channel<SCPReplyMessageDto> queue,IServiceScopeFactory factory)
{
      private volatile bool _shutdownFlag;

      public void SetShutDownFlag()
      {
            SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugToFile);
            Thread.Sleep(100);
            _shutdownFlag = true;
      }

      public void TurnOffDebug()
      {
            bool flag = SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugOff);
            if (flag)
            {
                  Console.WriteLine("Debug to file off");
            }
            else
            {
                  Console.WriteLine("Debug to file on");
            }
      }

      //////
      // Method: Turn on debug to file
      //////
      public void TurnOnDebug()
      {
            bool flag = SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugToFile);
            if (flag)
            {
                  Console.WriteLine("Debug to file on");
            }
            else
            {
                  Console.WriteLine("Debug to file off");
            }

      }

      public async Task GetTransactionUntilShutDownAsync()
      {
            while (!_shutdownFlag)
            {
                  try
                  {
                        var gotMessage = await GetTransaction();

                        if (!gotMessage)
                        Thread.Sleep(50); // prevents CPU burn
                  }
                  catch (Exception ex)
                  {
                        logger.LogCritical(ex, "Fatal error in Aero DLL listener loop");
                        Thread.Sleep(1000);
                  }
            }
      }

      private async Task<bool> GetTransaction()
      {
            using var scope = factory.CreateScope();
            var mapper = scope.ServiceProvider.GetRequiredService<IObjectMapper>();

            SCPReplyMessage message = new SCPReplyMessage();
            if (!message.GetMessage())
                  return false;

            await ProcessMessageAsync(mapper.Map<SCPReplyMessageDto>(message));
            return true;
      }

      private async Task ProcessMessageAsync(SCPReplyMessageDto message)
      {
            using var scope = factory.CreateScope();
            switch (message.ReplyType)
            {
                  // Occur when command to SCP not success
                  case (int)enSCPReplyType.enSCPReplyNAK:
                        //await queue.Writer.WriteAsync(message);
                        logger.LogError(ScpReplyMessageBuilder.BuildNakMessage(message));
                        break;
                  case (int)enSCPReplyType.enSCPReplyTransaction:
                        TransactionHandlerHelper.SCPReplyTransactionHandler(message, queue, logger);
                        break;
                  case (int)enSCPReplyType.enSCPReplyCommStatus:
                  switch (message.comm.status)
                        {
                              case 2:
                                    logger.LogInformation(ScpReplyMessageBuilder.CommStatusMessage(message));
                                    break;
                              default:
                                    var idReportService = scope.ServiceProvider.GetRequiredService<IIdReportService>();
                                    idReportService.RemoveIdReportById(message.SCPId);
                                    logger.LogError(ScpReplyMessageBuilder.CommStatusMessage(message));
                                    break;
                        }
                        await queue.Writer.WriteAsync(message);
                        break;
                  case (int)enSCPReplyType.enSCPReplyIDReport:
                        logger.LogInformation(ScpReplyMessageBuilder.IdReportMessage(message));
                        await queue.Writer.WriteAsync(message);
                        break;
                  case (int)enSCPReplyType.enSCPReplyTranStatus:
                        await queue.Writer.WriteAsync(message);
                        switch (message.tran_sts.disabled)
                        {
                              case 0:
                                    logger.LogInformation(ScpReplyMessageBuilder.TranStatusMessage(message));
                                    break;
                              default:
                                    logger.LogError(ScpReplyMessageBuilder.TranStatusMessage(message));
                                    break;
                        }
                        break;
                  case (int)enSCPReplyType.enSCPReplySrMsp1Drvr:
                  switch (message.sts_drvr.mode)
                        {
                              case 0:
                                    logger.LogError(ScpReplyMessageBuilder.Msp1DrvrMessage(message));
                                    break;
                              default:
                                    logger.LogInformation(ScpReplyMessageBuilder.Msp1DrvrMessage(message));
                                    break;
                        }
                        break;
                  case (int)enSCPReplyType.enSCPReplySrSio:
                        await queue.Writer.WriteAsync(message);
                        logger.LogInformation(LogMessageHelper.ToString(message.sts_sio));
                        break;
                  case (int)enSCPReplyType.enSCPReplySrMp:
                        await queue.Writer.WriteAsync(message);
                        // MessageHandlerHelper.SCPReplySrMp(message);
                        break;
                  case (int)enSCPReplyType.enSCPReplySrCp:
                        await queue.Writer.WriteAsync(message);
                        // MessageHandlerHelper.SCPReplySrCp(message);
                        break;
                  case (int)enSCPReplyType.enSCPReplySrAcr:
                        await queue.Writer.WriteAsync(message);
                        // MessageHandlerHelper.SCPReplySrAcr(message);
                        break;
                  case (int)enSCPReplyType.enSCPReplySrTz:
                        // MessageHandlerHelper.SCPReplySrTz(message);
                        break;
                  case (int)enSCPReplyType.enSCPReplySrTv:
                        // MessageHandlerHelper.SCPReplySrTv(message);
                        break;
                  case (int)enSCPReplyType.enSCPReplySrMpg:
                        // MessageHandlerHelper.SCPReplySrMpg(message);
                        break;
                  case (int)enSCPReplyType.enSCPReplySrArea:
                        // MessageHandlerHelper.SCPReplySrArea(message);
                        break;
                  case (int)enSCPReplyType.enSCPReplySioRelayCounts:
                        // MessageHandlerHelper.SCPReplySioRelayCounts(message);
                        break;
                  case (int)enSCPReplyType.enSCPReplyStrStatus:
                        await queue.Writer.WriteAsync(message);
                        // MessageHandlerHelper.SCPReplyStrStatus(message);
                        Console.WriteLine(LogMessageHelper.ToString(message.str_sts));
                        break;
                  case (int)enSCPReplyType.enSCPReplyCmndStatus:
                        await queue.Writer.WriteAsync(message);
                        break;
                  case (int)enSCPReplyType.enSCPReplyWebConfigNetwork:
                        // MessageHandlerHelper.SCPReplyWebConfigNetwork(message);
                        await queue.Writer.WriteAsync(message);
                        Console.WriteLine(LogMessageHelper.ToString(message.web_network));
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
                        await queue.Writer.WriteAsync(message);
                        Console.WriteLine(LogMessageHelper.ToString(message.web_host_comm_prim));
                        break;

                  default:
                        break;
            }
      }

}

