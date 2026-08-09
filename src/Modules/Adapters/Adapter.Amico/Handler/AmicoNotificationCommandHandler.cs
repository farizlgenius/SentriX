// using System.Text.Json;
// using System.Threading.Channels;
// using Adapter.Abstraction.Command;
// using Adapter.Amico.Enums;
// using Adapter.Amico.Helper;
// using Adapter.Amico.Interfaces;
// using Adapter.Amico.Model.Objects;
// using Device.Contract.Interfaces;
// using Microsoft.Extensions.Logging;
// using Notifier.Contract.Constants;
// using Notifier.Contract.Interfaces;
// using SharedKernel.Helpers;
// using SharedKernel.Messaging;

// namespace Adapter.Amico.Handler;

// public sealed class AmicoNotificationCommandHandler(
//       Channel<WebhookRequest> queue
//       ) : ICommandHandler<AmicoNotificationCommand>
// {
//       public async Task HandleAsync(AmicoNotificationCommand command, CancellationToken ct)
//       {
//             Console.WriteLine("Suck!!");
//             var webhook = command.@event.Deserialize<WebhookRequest>();
//             Console.WriteLine(webhook.ObjectChanges);
            
//             await queue.Writer.WriteAsync(webhook);

      
//       }
// }