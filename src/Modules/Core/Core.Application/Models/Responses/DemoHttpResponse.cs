namespace Core.Application.Models.Responses;

public sealed record DemoHttpResponse(
      string SessionId,
    string Payload,
    string Signature,
    string ServerSingPublic
);