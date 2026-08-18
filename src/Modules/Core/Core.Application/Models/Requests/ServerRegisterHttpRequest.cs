namespace Core.Application.Models.Requests;

public sealed record ServerRegisterHttpRequest(
  Guid BackendGuid,
  string Customer,
  string EndUser,


);