using System.Threading.Channels;
using Core.Contract.DTOs.Alert;
using Core.Contract.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Workers;


public sealed class AlertWorker(
      Channel<AlertDetectDto> queue, 
      ILogger<AlertWorker> logger,
      IServiceScopeFactory scopeFactory) : BackgroundService
{
      protected async override Task ExecuteAsync(CancellationToken ct)
      {
            Console.WriteLine("Alert Background worker started.");
             while (!ct.IsCancellationRequested)
            {
                  await foreach (var message in queue.Reader.ReadAllAsync(ct))
                  {
                        using var scope = scopeFactory.CreateScope();
                        var @event = scope.ServiceProvider.GetRequiredService<IEvent>();
                        var alert = scope.ServiceProvider.GetRequiredService<IAlert>();

                        try
                        {
                              
                              // await @event.CreateAsync(
                              //       message.,
                              //       ct
                              // );
                              

                        }catch(Exception ex)
                        {
                              
                              await @event.InsertExceptionEventAsync(
                                    "Alert Worker",
                                    "Alert Worker",
                                    ex.Message,
                                    ex.InnerException == null ? string.Empty : ex.InnerException.ToString(),
                                    ex.StackTrace ?? string.Empty
                              );
                              logger.LogError(ex.Message);

                        }
                        
                  }
                  
            }
      }
}
