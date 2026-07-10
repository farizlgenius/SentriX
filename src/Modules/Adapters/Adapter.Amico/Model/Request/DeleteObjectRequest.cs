using System.Text.Json;

namespace Adapter.Amico.Model.Request;

public sealed record DeleteObjectRequest(
      string Objects,
      object Where
      );