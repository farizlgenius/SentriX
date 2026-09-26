using System.Threading.Channels;
using Core.Contract.DTOs.Events.Event;
using Core.Contract.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Workers;

public sealed class EventWorker(Channel<CreateEventDto> queue, ILogger<EventWorker> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{
      protected async override Task ExecuteAsync(CancellationToken ct)
      {
            Console.WriteLine("Event Background worker started.");

             while (!ct.IsCancellationRequested)
            {
                  await foreach (var message in queue.Reader.ReadAllAsync(ct))
                  {
                        using var scope = scopeFactory.CreateScope();
                        var @event = scope.ServiceProvider.GetRequiredService<IEvent>();

                        try
                        {
                              
                              await @event.CreateAsync(
                                    message,
                                    ct
                              );

                        }catch(Exception ex)
                        {
                              
                              await @event.InsertExceptionEventAsync(
                                    "Event Worker",
                                    "Event Worker",
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