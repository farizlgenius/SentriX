// using System.Threading.Channels;
// using Adapter.Amico.Helper;
// using Adapter.Amico.Interface;
// using Adapter.Amico.Interfaces;
// using Adapter.Amico.Model.Objects;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;
// using Notifier.Contract.Constants;
// using Notifier.Contract.Interfaces;
// using SharedKernel.Helpers;
// using Storage.Contract.Interfaces;

// namespace Adapter.Amico.Worker;

// public sealed class AmicoWorker(Channel<WebhookRequest> queue, ILogger<AmicoWorker> logger, IServiceScopeFactory scopeFactory) : BackgroundService
// {
//       protected async override Task ExecuteAsync(CancellationToken ct)
//       {
//             while (!ct.IsCancellationRequested)
//             {
//                   await foreach (var message in queue.Reader.ReadAllAsync(ct))
//                   {
//                         using var scope = scopeFactory.CreateScope();
//                         try
//                         {
//                               foreach (var change in message!.ObjectChanges)
//                               {
//                                     var model = change.Object switch
//                                     {
//                                           "access_logs" => JsonHelper.Deserialize<AccessLog>(change.Values),
//                                           _ => null
//                                     };

//                                     if (model is null)
//                                     {
//                                           logger.LogWarning("Unsupported webhook object '{Object}'", change.Object);
//                                           Console.WriteLine("Unsupported webhook object '{Object}'", change.Object);
//                                           continue;
//                                     }

//                                     switch (model)
//                                     {
//                                           case AccessLog accessLog:
//                                                 var device = scope.ServiceProvider.GetRequiredService<IDevice>();
//                                                 var e = scope.ServiceProvider.GetRequiredService<IEvent>();
//                                                 var noti = scope.ServiceProvider.GetRequiredService<INotifier>();
//                                                 var s = scope.ServiceProvider.GetRequiredService<IStorage>();
//                                                 var cmnd = scope.ServiceProvider.GetRequiredService<IAmicoCameraAdapter>();
//                                                 var d = await device.GetDeviceByDeviceIdAsync(accessLog.DeviceId.ToString());
//                                                 string path = "";
//                                                 if (accessLog.Event.Equals("3"))
//                                                 {
//                                                       var amico = scope.ServiceProvider.GetRequiredService<IAmicoRepository>();
//                                                       var am = await amico.GetAmicoByMacAsync(d.Mac);
//                                                       await s.SaveCaptureAsync(await cmnd.CaptureAsync(d.Mac,d.Ip),$"{accessLog.Time}");
//                                                       path=accessLog.Time;
//                                                 }
//                                                 await e.AddEventAsync(
//                                                       DateTimeHelper.IntToDateTimeUTC(int.Parse(accessLog.Time)),
//                                                       string.Empty,
//                                                       "Amico",
//                                                       "Access Log",
//                                                       string.Empty,
//                                                       d.Mac,
//                                                       d.Name,
//                                                       EventHelper.EventMapper(int.Parse(accessLog.Event)),
//                                                       string.Empty,
//                                                       d.LocationId,
//                                                       path
//                                                 );
//                                                 await noti.TriggerToTopic(NotifierTopic.EVENT);
//                                                 break;

//                                           default:
//                                                 break;
//                                     }
//                               }
//                         }
//                         catch(Exception ex)
//                         {
//                               logger.LogError(ex.Message);
//                         }

//                   }
//             }
//       }

      
// }