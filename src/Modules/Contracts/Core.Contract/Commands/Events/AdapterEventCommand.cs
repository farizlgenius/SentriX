using Core.Contract.DTOs.AdapterEvent;
using Core.Contract.DTOs.Event;
using SharedKernel.Messaging;
using SharedKernel.Model;

namespace Core.Contract.Commands.Events;

public sealed record AdapterEventCommand(CommandResponse @event) : ICommand;