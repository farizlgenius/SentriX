using System.Text.Json;
using SharedKernel.Messaging;
using SharedKernel.Model;

namespace Adapter.Abstraction.Command;

public sealed record AmicoNotificationCommand(JsonElement @event) : ICommand; 