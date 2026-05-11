using System;

namespace Notifier.Contract.DTOs;

public sealed record NotifierDto(string Key,object? Data = default!);