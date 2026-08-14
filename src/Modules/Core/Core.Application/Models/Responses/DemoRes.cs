namespace Core.Application.Models.Responses;

public sealed record DemoRes(
      string SessionId,
    string Payload,
    string Signature,
    string ServerSingPublic
);