using System;

namespace Output.Contract.DTOs;

public sealed record OutputDto(int Id,string Name,string Mode,int PulseTime);
