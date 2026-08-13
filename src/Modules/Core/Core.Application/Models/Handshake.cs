namespace Core.Application.Models;

public sealed record Handshake(
  string SessionId,
  byte[] SharedKey
);