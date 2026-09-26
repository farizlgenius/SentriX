using System.Threading.Channels;
using Core.Contract.DTOs.Events.Audit;
using Core.Contract.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Workers;

public sealed class AuditTrailWorker(Channel<AuditTrailInsert> queue, ILogger<AuditTrailWorker> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{
      protected async override Task ExecuteAsync(CancellationToken ct)
      {
            Console.WriteLine("Audit Background worker started.");

             while (!ct.IsCancellationRequested)
            {
                  await foreach (var message in queue.Reader.ReadAllAsync(ct))
                  {
                        using var scope = scopeFactory.CreateScope();
                        var @event = scope.ServiceProvider.GetRequiredService<IEvent>();

                        try
                        {
                              
                              await @event.InsertAuditAsync(
                                    message.Entity,
                                    message.Action,
                                    message.Username,
                                    message.Ip,
                                    message.ObjectGuid,
                                    message.ObjectName,
                                    message.Detail,
                                    message.LocationId,
                                    ct
                              );

                        }catch(Exception ex)
                        {
                              await @event.InsertExceptionEventAsync(
                                    "Audit Worker",
                                    "Audit Worker",
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