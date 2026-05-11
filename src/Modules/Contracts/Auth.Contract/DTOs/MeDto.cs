using System;
using System.Net;
using Role.Contract.DTOs;
using SharedKernel.Domain;

namespace Auth.Contract.DTOs;

public sealed record MeDto(HttpStatusCode Code, string Message, DateTime Timestamp, List<int> Locations, List<PermissionDto> Permissions) : BaseResponse(Code, Message, Timestamp);